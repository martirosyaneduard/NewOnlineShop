using Microsoft.EntityFrameworkCore;

namespace OnlineShop.Models
{
    public class OnlineShopDbContext:DbContext
    {
        public DbSet<Customer>? Customers { get; set; }
        public DbSet<Order>? Orders { get; set; }
        public DbSet<Product>? Products { get; set; }
        public DbSet<Category>? Categories { get; set; }

        public OnlineShopDbContext(DbContextOptions<OnlineShopDbContext> options):base(options)
        {

        }

    }
}
