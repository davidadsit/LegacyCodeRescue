using MvcMusicStore.Models;

namespace MusicStore.Tests.ObjectMothers
{
    public static class ArtistMother
    {
        public static Artist GetDefault(int id = 1)
        {
            return new Artist() {ArtistId = id, Name = "artist-" + id};
        }
    }
}