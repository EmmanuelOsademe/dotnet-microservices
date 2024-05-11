using EMStore.Services.RewardAPI.Message;

namespace EMStore.Services.RewardAPI.Services.IServices
{
    public interface IRewardService
    {
        Task UpdateRewards(RewardsMessage message);
    }
}
