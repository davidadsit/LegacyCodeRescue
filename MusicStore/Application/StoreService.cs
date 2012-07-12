using System.Collections.Generic;
using System.Linq;
using MusicStore.Domain;
using MusicStore.Infrastructure;

namespace MusicStore.Application
{
	public class StoreService : IStoreService
	{
		private readonly MusicStoreEntities musicStoreEntities;

		public StoreService()
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
	}
}