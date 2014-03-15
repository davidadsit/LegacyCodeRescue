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
            return genreVm.Albums.Select(album => CatalogFacade.GetAlbumById(album.AlbumId)).All(albumVm => albumVm.GenreId == genreVm.GenreId) ? "YES" : "NO";
        }
    }
}