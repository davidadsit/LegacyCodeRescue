using System.Collections.Generic;
using System.Web.Mvc;
using MvcMusicStore.Application;
using MvcMusicStore.Models;

namespace MvcMusicStore.Controllers
{
	public class StoreController : Controller
	{
		private readonly IStoreRepository StoreRepository;

		public StoreController()
			: this(new StoreRepository())
		{
		}

		public StoreController(IStoreRepository StoreRepository)
		{
			this.StoreRepository = StoreRepository;
		}

		public ActionResult Browse(string genre)
		{
			return View(StoreRepository.FindGenreByName(genre));
		}

		public ActionResult Details(int id)
		{
			return View(StoreRepository.GetAlbumById(id));
		}

		[ChildActionOnly]
		public ActionResult GenreMenu()
		{
			return PartialView(StoreRepository.GetAllGenres());
		}

		public ActionResult Index()
		{
			return View(StoreRepository.GetAllGenres());
		}
	}
}