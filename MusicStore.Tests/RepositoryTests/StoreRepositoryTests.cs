using System.Linq;
using MvcMusicStore.Application;
using MvcMusicStore.Models;
using NUnit.Framework;

namespace MusicStore.Tests.RepositoryTests
{
    [TestFixture]
    public class StoreRepositoryTests
    {
        StoreRepository storeRepository;
        Genre NewGenre;

        [SetUp]
        public void SetUp()
        {
            storeRepository = new StoreRepository();
            NewGenre = new Genre() { Name = "Viking", Description = "For true warriors" };
            AssertThatGenreDoesNotExist(NewGenre.Name);
        }

        [TearDown]
        public void TearDown()
        {
            var genres = storeRepository.GetAllGenres().ToArray();

            foreach (var genre in genres.Where(x => x.Name == NewGenre.Name))
            {
                storeRepository.DeleteGenre(genre.GenreId);
            }
            AssertThatGenreDoesNotExist(NewGenre.Name);
        }

        [Test]
        public void When_creating_and_deleting_a_genre()
        {
            var savedGenre = storeRepository.SaveGenre(NewGenre);
            AssertThatGenreExists(NewGenre.Name);
            storeRepository.DeleteGenre(savedGenre.GenreId);
            AssertThatGenreDoesNotExist(NewGenre.Name);
        }

        void AssertThatGenreDoesNotExist(string name)
        {
            var genres = storeRepository.GetAllGenres().ToArray();
            Assert.That(genres.Any(x => x.Name == name), Is.False);
        }

        void AssertThatGenreExists(string name)
        {
            var genres = storeRepository.GetAllGenres().ToArray();
            Assert.That(genres.Any(x => x.Name == name), Is.True);
        }
    }
}