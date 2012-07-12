using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcMusicStore.Application;
using MvcMusicStore.Models;

namespace MvcMusicStore.Controllers
{
    public class StoreController : Controller
    {
		private readonly IStoreService storeService;
		private MusicStoreEntities storeDb;

		public StoreController()
			: this(new StoreService())
		{
		}

		public StoreController(IStoreService storeService)
		{
			this.storeService = storeService;
			this.storeDb = new MusicStoreEntities();
		}

    	//
        // GET: /Store/

        public ActionResult Index()
        {
            var genres = storeDb.Genres.ToList();

            return View(genres);
        }

        //
        // GET: /Store/Browse?genre=Disco

        public ActionResult Browse(string genre)
        {
			var genreModel = storeService.FindGenreByName(genre);
            return View(genreModel);
        }

        //
        // GET: /Store/Details/5

        public ActionResult Details(int id)
        {
            var album = storeDb.Albums.Find(id);

            return View(album);
        }

        //
        // GET: /Store/GenreMenu

        [ChildActionOnly]
        public ActionResult GenreMenu()
        {
            var genres = storeDb.Genres.ToList();

            return PartialView(genres);
        }

    }
}