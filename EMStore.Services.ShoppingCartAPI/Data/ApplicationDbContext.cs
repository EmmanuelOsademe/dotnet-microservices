using EMStore.Services.ShoppingCartAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EMStore.Services.ShoppingCartAPI.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {

        public DbSet<CartHeader> CartHeaders { get; set; }
        public DbSet<CartDetails> CartDetails { get; set; }
    }
}
