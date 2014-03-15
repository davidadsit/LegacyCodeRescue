using System.Linq;
using MvcMusicStore.Application;

namespace MusicStore.Acceptance.Fixtures
{
    public class CatalogFixture
    {
        readonly CatalogFacade CatalogFacade;

        public CatalogFixture()
        {
            CatalogFacade = new CatalogFacade(new StoreRepository());
        }

        public string GenreName;

        public string GenreAlbums()
        {
            var genreVm = CatalogFacade.FindGenreByName(GenreName);
            return string.Join(", ", genreVm.Albums.Select(x => x.Title));
        }
    }
}