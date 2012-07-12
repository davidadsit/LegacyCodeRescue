using System.Collections.Generic;
using System.Web.Mvc;
using MvcMusicStore.Application;
using MvcMusicStore.Models;

namespace MvcMusicStore.Controllers
{
	public class StoreController : Controller
	{
		private readonly IStoreService storeService;

		public StoreController()
			: this(new StoreService())
		{
		}

		public StoreController(IStoreService storeService)
		{
			this.storeService = storeService;
		}

		public ActionResult Browse(string genre)
		{
			return View(storeService.FindGenreByName(genre));
		}

		public ActionResult Details(int id)
		{
			return View(storeService.GetAlbumById(id));
		}

		[ChildActionOnly]
		public ActionResult GenreMenu()
		{
			return PartialView(storeService.GetAllGenres());
		}

		public ActionResult Index()
		{
			return View(storeService.GetAllGenres());
		}
	}
}