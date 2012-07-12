using System.Collections.Generic;
using System.Linq;
using MvcMusicStore.Models;

namespace MvcMusicStore.Application
{
	public interface IStoreService
	{
		Genre FindGenreByName(string genre);
	}

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
	}
}