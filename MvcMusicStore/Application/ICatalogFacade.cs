using System.Collections.Generic;
using MvcMusicStore.Models;

namespace MvcMusicStore.Application
{
    public interface ICatalogFacade
    {
        GenreVm FindGenreByName(string genreName);
        AlbumVm GetAlbumById(int albumId);
        IEnumerable<GenreVm> GetAllGenres();
    }
}