using KAShop.Models.Category;
using Microsoft.EntityFrameworkCore;

namespace KAShop.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Category> categories { get; set; }
        public DbSet<CategoryTranslation> categoryTranslations { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
