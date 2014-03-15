using System.Web.Mvc;
using Moq;
using MvcMusicStore.Application;
using MvcMusicStore.Controllers;
using NUnit.Framework;

namespace MusicStore.Tests.ControllerTests
{
    [TestFixture]
    public class StoreControllerTests
    {
        [SetUp]
        public void SetUp()
        {
            catalogFacade = new Mock<ICatalogFacade>();
            storeController = new StoreController(catalogFacade.Object);
        }

        Mock<ICatalogFacade> catalogFacade;
        StoreController storeController;

        [Test]
        public void Browse_displays_the_albums_in_a_genre()
        {
            var genre = new GenreVm();
            catalogFacade.Setup(x => x.FindGenreByName(It.IsAny<string>())).Returns(genre);

            var browseResult = (ViewResult) storeController.Browse("Rock");

            Assert.AreEqual(genre, browseResult.Model);
        }

        [Test]
        public void Details_shows_one_album_by_id()
        {
            var albumFromService = new AlbumVm();
            catalogFacade.Setup(x => x.GetAlbumById(It.IsAny<int>())).Returns(albumFromService);

            var detailsResult = (ViewResult) storeController.Details(5);

            Assert.AreEqual(albumFromService, detailsResult.Model);
        }

        [Test]
        public void GenreMenu_displays_the_list_of_genres()
        {
            var genres = new[] {new GenreVm()};
            catalogFacade.Setup(x => x.GetAllGenres()).Returns(genres);

            var menuResult = (PartialViewResult) storeController.GenreMenu();

            Assert.AreEqual(genres, menuResult.Model);
        }

        [Test]
        public void Index_displays_the_list_of_genres()
        {
            //Arrange
            var genres = new[] {new GenreVm()};
            catalogFacade.Setup(x => x.GetAllGenres()).Returns(genres);
            //Act
            var indexResult = (ViewResult) storeController.Index();
            //Assert
            Assert.AreEqual(genres, indexResult.Model);
        }
    }
}