using EMStore.Services.OrdersAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EMStore.Services.OrdersAPI.Data
{
    public class ApplicationDbContext (DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {

        public DbSet<OrderHeader> OrderHeaders { get; set; }

        public DbSet<OrderDetails> OrderDetails { get; set; }
    }
}
