using System.Linq;
using System.Web.Mvc;
using MvcMusicStore.Controllers;
using MvcMusicStore.Models;
using NUnit.Framework;

namespace MusicStore.Tests.ControllerTests
{
	[TestFixture]
	public class StoreControllerTests
	{
		[Test]
		public void Browse_displays_the_albums_in_a_genre()
		{
			StoreController storeController = new StoreController();
			MusicStoreEntities storeDb = new MusicStoreEntities();

			ViewResult browseResult = (ViewResult) storeController.Browse("Rock");

			Genre genre = storeDb.Genres.Include("Albums").Single(g => g.Name == "Rock");
			Assert.AreEqual(genre, browseResult.Model);
		}

		[Test]
		public void Details_shows_one_album_by_id()
		{
			StoreController storeController = new StoreController();
			MusicStoreEntities storeDb = new MusicStoreEntities();

			ViewResult detailsResult = (ViewResult) storeController.Details(5);

			Assert.AreEqual(storeDb.Albums.Find(5), detailsResult.Model);
		}

		[Test]
		public void Index_displays_the_list_of_genres()
		{
			//Arrange
			StoreController storeController = new StoreController();
			MusicStoreEntities storeDb = new MusicStoreEntities();
			//Act
			ViewResult indexResult = (ViewResult) storeController.Index();
			//Assert
			Assert.AreEqual(storeDb.Genres, indexResult.Model);
		}
	}
}