using EMStore.Services.OrderAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EMStore.Services.OrderAPI.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {

        public DbSet<OrderHeader> OrderHeaders { get; set; }

        public DbSet<OrderDetails> OrderDetails { get; set; }   
    }
}
