using Microsoft.EntityFrameworkCore;

namespace Stock.API.Models
{
    public class MainDbContext :DbContext
    {
        public MainDbContext(DbContextOptions<MainDbContext> options) :base(options)
        {
            
        }
        public DbSet<StockItem> StockItems { get; set; }
    }
   
}
