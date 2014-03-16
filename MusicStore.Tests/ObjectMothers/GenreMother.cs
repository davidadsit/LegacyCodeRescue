using System.Collections.Generic;
using MvcMusicStore.Models;

namespace MusicStore.Tests.ObjectMothers
{
    public class GenreMother
    {
        public static Genre GetDefault()
        {
            var genre = new Genre
                        {
                            GenreId = 123,
                            Description = "description",
                            Name = "name",
                        };
            genre.AddAlbum(AlbumMother.GetDefault(1));
            genre.AddAlbum(AlbumMother.GetDefault(2));
            genre.AddAlbum(AlbumMother.GetDefault(3));
            return genre;
        }
    }
}