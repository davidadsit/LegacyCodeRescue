using System.Collections.Generic;
using MusicStore.Domain;

namespace MusicStore.Application
{
	public interface IStoreService
	{
		Genre FindGenreByName(string genre);
		Album GetAlbumById(int albumId);
		IEnumerable<Genre> GetAllGenres();
	}
}