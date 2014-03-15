using System.Collections.Generic;

namespace MvcMusicStore.Application
{
    public class GenreVm
    {
        public int GenreId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<AlbumVm> Albums { get; set; }
    }
}