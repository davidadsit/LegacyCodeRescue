using System.Collections.Generic;
using System.Web.Mvc;
using MvcMusicStore.Application;
using MvcMusicStore.Models;

namespace MvcMusicStore.Controllers
{
	public class StoreController : Controller
	{
        private readonly ICatalogFacade catalogFacade;

		public StoreController()
            : this(new CatalogFacade(new StoreRepository()))
		{
		}

        public StoreController(ICatalogFacade catalogFacade)
		{
			this.catalogFacade = catalogFacade;
		}

		public ActionResult Browse(string genre)
		{
			return View(catalogFacade.FindGenreByName(genre));
		}

		public ActionResult Details(int id)
		{
			return View(catalogFacade.GetAlbumById(id));
		}

		[ChildActionOnly]
		public ActionResult GenreMenu()
		{
			return PartialView(catalogFacade.GetAllGenres());
		}

		public ActionResult Index()
		{
			return View(catalogFacade.GetAllGenres());
		}
	}
}