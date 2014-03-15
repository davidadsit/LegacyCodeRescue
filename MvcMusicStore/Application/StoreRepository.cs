using System.Collections.Generic;
using System.Linq;
using MvcMusicStore.Models;

namespace MvcMusicStore.Application
{
    public class StoreRepository : IStoreRepository
	{
		private readonly MusicStoreEntities musicStoreEntities;

		public StoreRepository()
		{
			musicStoreEntities = new MusicStoreEntities();
		}

		public Genre FindGenreByName(string genre)
		{
			return musicStoreEntities
				.Genres
				.Include("Albums")
				.Single(g => g.Name == genre);
		}

		public Album GetAlbumById(int albumId)
		{
			return musicStoreEntities.Albums.Find(albumId);
		}

		public IEnumerable<Genre> GetAllGenres()
		{
			return musicStoreEntities.Genres.ToList();
		}

        public Genre SaveGenre(Genre genre)
        {
            musicStoreEntities.Genres.Add(genre);
            musicStoreEntities.SaveChanges();
            return genre;
        }

        public void DeleteGenre(int id)
        {
            var entity = musicStoreEntities.Genres.Find(id);
            if (entity == null) return;

            musicStoreEntities.Genres.Remove(entity);
            musicStoreEntities.SaveChanges();
        }
	}
}