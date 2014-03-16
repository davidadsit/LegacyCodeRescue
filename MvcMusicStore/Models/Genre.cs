using System.Collections.Generic;

namespace MvcMusicStore.Models
{
    public partial class Genre
    {
        public Genre()
        {
            Albums = new List<Album>();
        }

        public int GenreId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Album> Albums { get; set; }

        public void AddAlbum(Album album)
        {
            album.Genre = this;
            Albums.Add(album);
        }
    }
}