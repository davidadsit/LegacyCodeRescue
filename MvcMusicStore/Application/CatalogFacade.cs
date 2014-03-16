using System.Collections.Generic;
using System.Linq;
using MvcMusicStore.Models;

namespace MvcMusicStore.Application
{
    public class CatalogFacade : ICatalogFacade
    {
        readonly IStoreRepository storeRepository;

        public CatalogFacade(IStoreRepository storeRepository)
        {
            this.storeRepository = storeRepository;
        }

        public GenreVm FindGenreByName(string genreName)
        {
            var genre = storeRepository.FindGenreByName(genreName);
            return GenreToGenreVm(genre);
        }

        public AlbumVm GetAlbumById(int albumId)
        {
            return AlbumToAlbumVm(storeRepository.GetAlbumById(albumId));
        }

        public IEnumerable<GenreVm> GetAllGenres()
        {
            return storeRepository.GetAllGenres().Select(GenreToGenreVm).ToArray();
        }

        AlbumVm AlbumToAlbumVm(Album album)
        {
            if (album == null) return null;
            return new AlbumVm
                   {
                       AlbumId = album.AlbumId,
                       GenreId = album.GenreId,
                       GenreName = album.Genre.Name,
                       ArtistId = album.ArtistId,
                       ArtistName = album.Artist.Name,
                       Title = album.Title,
                       Price = album.Price,
                       AlbumArtUrl = album.AlbumArtUrl,
                   };
        }

        GenreVm GenreToGenreVm(Genre genre)
        {
            if (genre == null) return null;
            var albumVms = new AlbumVm[] {};
            if (genre.Albums != null)
            {
                albumVms = genre.Albums.Select(AlbumToAlbumVm).ToArray();
            }
            return new GenreVm
                   {
                       GenreId = genre.GenreId,
                       Name = genre.Name,
                       Description = genre.Description,
                       Albums = albumVms,
                   };
        }
    }
}