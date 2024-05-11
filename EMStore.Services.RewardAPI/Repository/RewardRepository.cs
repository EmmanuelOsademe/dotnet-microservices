using EMStore.Services.RewardAPI.Data;
using EMStore.Services.RewardAPI.Message;
using EMStore.Services.RewardAPI.Models;
using EMStore.Services.RewardAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace EMStore.Services.RewardAPI.Repository
{
    public class RewardRepository(DbContextOptions<ApplicationDbContext> dbOptions) : IRewardRepository
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbOptions = dbOptions;
        public async Task UpdateReward(RewardsMessage rewardsMessage)
        {
            try
            {
                Rewards rewards = new()
                {
                    UserId = rewardsMessage.UserId,
                    OrderId = rewardsMessage.OrderId,
                    RewardsActivity = rewardsMessage.RewardsActivity,
                    RewardsDate = DateTime.Now,
                };
                await using var _dbContext = new ApplicationDbContext(_dbOptions);
                var reward = await _dbContext.Rewards.AddAsync(rewards);
                await _dbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
