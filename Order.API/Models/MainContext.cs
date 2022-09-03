using Microsoft.EntityFrameworkCore;

namespace Order.API.Models
{
    public class MainContext :DbContext
    {
        public MainContext(DbContextOptions<MainContext> options):base(options)
        {
            
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        
    }
}
