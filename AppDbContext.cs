using Microsoft.EntityFrameworkCore;

namespace Api_project
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public string DbPath { get; }
        public AppDbContext()
        {
            DbPath = "store.db";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
    }
}
