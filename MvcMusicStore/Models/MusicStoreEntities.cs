using System.Data.Entity;

namespace MvcMusicStore.Models
{
	public interface IMusicStoreEntities
	{
		DbSet<Album> Albums { get; }
		DbSet<Genre> Genres { get; }
	}

	public class MusicStoreEntities : DbContext, IMusicStoreEntities
	{
        public DbSet<Album> Albums { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
    }
}