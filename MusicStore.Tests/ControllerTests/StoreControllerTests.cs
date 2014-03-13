using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Moq;
using MvcMusicStore.Application;
using MvcMusicStore.Controllers;
using MvcMusicStore.Models;
using NUnit.Framework;

namespace MusicStore.Tests.ControllerTests
{
	[TestFixture]
	public class StoreControllerTests
	{
		private Mock<IStoreRepository> musicStoreService;
		private StoreController storeController;

		[SetUp]
		public void SetUp()
		{
			musicStoreService = new Mock<IStoreRepository>();
			storeController = new StoreController(musicStoreService.Object);
		}
		
		[Test]
		public void Browse_displays_the_albums_in_a_genre()
		{
			var genre = new Genre();
			musicStoreService.Setup(x => x.FindGenreByName(It.IsAny<string>())).Returns(genre);

			ViewResult browseResult = (ViewResult)storeController.Browse("Rock");

			Assert.AreEqual(genre, browseResult.Model);
		}

		[Test]
		public void Details_shows_one_album_by_id()
		{
			var albumFromService = new Album();
			musicStoreService.Setup(x => x.GetAlbumById(It.IsAny<int>())).Returns(albumFromService);

			ViewResult detailsResult = (ViewResult)storeController.Details(5);

			Assert.AreEqual(albumFromService, detailsResult.Model);
		}

		[Test]
		public void Index_displays_the_list_of_genres()
		{
			//Arrange
			var genres = new[] { new Genre() };
			musicStoreService.Setup(x => x.GetAllGenres()).Returns(genres);
			//Act
			ViewResult indexResult = (ViewResult)storeController.Index();
			//Assert
			Assert.AreEqual(genres, indexResult.Model);
		}

		[Test]
		public void GenreMenu_displays_the_list_of_genres()
		{
			Genre[] genres = new[] { new Genre() };
			musicStoreService.Setup(x => x.GetAllGenres()).Returns(genres);

			PartialViewResult menuResult = (PartialViewResult)storeController.GenreMenu();

			Assert.AreEqual(genres, menuResult.Model);
		}

	}
}