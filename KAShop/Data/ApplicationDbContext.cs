using KAShop.Models;
using Microsoft.EntityFrameworkCore;

namespace KAShop.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Category> categories { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=.;Database=KAShop_api;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }
}
