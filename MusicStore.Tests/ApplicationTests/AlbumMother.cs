using System;
using MvcMusicStore.Models;

namespace MusicStore.Tests.ApplicationTests
{
    public class AlbumMother
    {
        public static Album GetDefault(int id = 1)
        {
            return new Album
                   {
                       AlbumId = id,
                       GenreId = 123,
                       ArtistId = 4,
                       Title = "album-" + id,
                       Price = 10.01m,
                       AlbumArtUrl = Guid.NewGuid().ToString()
                   };
        }
    }
}