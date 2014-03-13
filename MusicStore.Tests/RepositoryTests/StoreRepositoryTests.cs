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
        const string Name = "Viking";

        [SetUp]
        public void SetUp()
        {
            storeRepository = new StoreRepository();
            AssertThatGenreDoesNotExist(Name);
        }

        [TearDown]
        public void TearDown()
        {
            var genres = storeRepository.GetAllGenres().ToArray();

            foreach (var genre in genres.Where(x => x.Name == Name))
            {
                storeRepository.DeleteGenre(genre.GenreId);
            }
            AssertThatGenreDoesNotExist(Name);
        }

        [Test]
        public void When_creating_and_deleting_a_genre()
        {
            var newGenre = new Genre { Name = Name, Description = "For true warriors" };

            var savedGenre = storeRepository.SaveGenre(newGenre);
            AssertThatGenreExists(newGenre.Name);

            storeRepository.DeleteGenre(savedGenre.GenreId);
            AssertThatGenreDoesNotExist(newGenre.Name);
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