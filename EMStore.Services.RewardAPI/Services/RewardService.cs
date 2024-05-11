using EMStore.Services.RewardAPI.Data;
using EMStore.Services.RewardAPI.Message;
using EMStore.Services.RewardAPI.Repository.IRepository;
using EMStore.Services.RewardAPI.Services.IServices;
using EMStores.MessageBus;

namespace EMStore.Services.RewardAPI.Services
{
    public class RewardService (IRewardRepository rewardRepository) : IRewardService
    {
        private readonly IRewardRepository _rewardRepository = rewardRepository;
        public async Task UpdateRewards(RewardsMessage message)
        {
            try
            {
                await _rewardRepository.UpdateReward(message);
            }catch (Exception ex)
            {

            }
            
        }
    }
}
