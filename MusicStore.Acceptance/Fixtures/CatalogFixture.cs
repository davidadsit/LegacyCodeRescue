using System;
using System.Linq;
using MvcMusicStore.Application;

namespace MusicStore.Acceptance.Fixtures
{
    public class CatalogFixture
    {
        readonly CatalogFacade CatalogFacade;

        public int AlbumId;
        public string GenreName;

        public CatalogFixture()
        {
            CatalogFacade = new CatalogFacade(new StoreRepository());
        }

        public string AllAlbumTitles()
        {
            var genreVm = CatalogFacade.FindGenreByName(GenreName);
            return string.Join(Environment.NewLine, genreVm.Albums.OrderBy(x => x.Title).Select(x => x.Title));
        }

        public string GenresToAlbumLinksAreConsistent()
        {
            var genreVm = CatalogFacade.FindGenreByName(GenreName);
            foreach (var album in genreVm.Albums)
            {
                var albumVm = CatalogFacade.GetAlbumById(album.AlbumId);
                if (albumVm.GenreId != genreVm.GenreId) return "NO";
            }
            return "YES";
        }
    }
}