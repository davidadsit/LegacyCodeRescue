using System.Collections.Generic;
using MvcMusicStore.Models;

namespace MvcMusicStore.Application
{
    public interface IStoreRepository
    {
        Genre FindGenreByName(string genre);
        Album GetAlbumById(int albumId);
        IEnumerable<Genre> GetAllGenres();
    }
}