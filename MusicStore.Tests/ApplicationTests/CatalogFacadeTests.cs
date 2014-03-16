using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using MusicStore.Tests.ObjectMothers;
using MvcMusicStore.Application;
using MvcMusicStore.Models;
using NUnit.Framework;

namespace MusicStore.Tests.ApplicationTests
{
    [TestFixture]
    public class CatalogFacadeTests
    {
        [SetUp]
        public void SetUp()
        {
            storeRepository = new Mock<IStoreRepository>();
            catalogFacade = new CatalogFacade(storeRepository.Object);
        }

        Mock<IStoreRepository> storeRepository;
        CatalogFacade catalogFacade;

        static void AssertAlbumVmMatchesAlbum(Album album, AlbumVm albumVm)
        {
            Assert.That(album.AlbumId, Is.EqualTo(albumVm.AlbumId));
            Assert.That(album.GenreId, Is.EqualTo(albumVm.GenreId));
            if (album.Genre != null) Assert.That(album.Genre.Name, Is.EqualTo(albumVm.GenreName));
            Assert.That(album.ArtistId, Is.EqualTo(albumVm.ArtistId));
            if (album.Artist != null) Assert.That(album.Artist.Name, Is.EqualTo(albumVm.ArtistName));
            Assert.That(album.Title, Is.EqualTo(albumVm.Title));
            Assert.That(album.Price, Is.EqualTo(albumVm.Price));
            Assert.That(album.AlbumArtUrl, Is.EqualTo(albumVm.AlbumArtUrl));
        }

        static void AssertGenreVmMatchesGenre(Genre genre, GenreVm result)
        {
            Assert.That(genre.GenreId, Is.EqualTo(result.GenreId));
            Assert.That(genre.Name, Is.EqualTo(result.Name));
            Assert.That(genre.Description, Is.EqualTo(result.Description));
        }

        [Test]
        public void FindGenreByName_builds_the_vm_correctly()
        {
            var genre = GenreMother.GetDefault();
            storeRepository.Setup(x => x.FindGenreByName(It.IsAny<string>())).Returns(genre);

            var result = catalogFacade.FindGenreByName("Rock");

            AssertGenreVmMatchesGenre(genre, result);
            for (var i = 0; i < genre.Albums.Count; i++)
            {
                AssertAlbumVmMatchesAlbum(genre.Albums.ElementAt(i), result.Albums.ElementAt(i));
            }
        }

        [Test]
        public void FindGenreByName_loads_the_correct_genre()
        {
            storeRepository.Setup(x => x.FindGenreByName(It.IsAny<string>())).Returns(GenreMother.GetDefault());

            catalogFacade.FindGenreByName("Rock");

            storeRepository.Verify(x => x.FindGenreByName("Rock"));
        }
    }
}