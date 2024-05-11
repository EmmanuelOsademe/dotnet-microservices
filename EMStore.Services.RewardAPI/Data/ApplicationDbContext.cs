using EMStore.Services.RewardAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EMStore.Services.RewardAPI.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {

        public DbSet<Rewards> Rewards { get; set; }
    }
}
