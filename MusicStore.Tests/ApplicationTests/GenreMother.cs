using System.Collections.Generic;
using MvcMusicStore.Models;

namespace MusicStore.Tests.ApplicationTests
{
    public class GenreMother
    {
        public static Genre GetDefault()
        {
            return new Genre
                   {
                       GenreId = 123,
                       Description = "description",
                       Name = "name",
                       Albums = new List<Album>
                                {
                                    AlbumMother.GetDefault(1),
                                    AlbumMother.GetDefault(2),
                                    AlbumMother.GetDefault(3),
                                }
                   };
        }
    }
}