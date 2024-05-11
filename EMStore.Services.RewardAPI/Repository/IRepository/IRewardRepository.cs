using EMStore.Services.RewardAPI.Message;

namespace EMStore.Services.RewardAPI.Repository.IRepository
{
    public interface IRewardRepository
    {
        Task UpdateReward(RewardsMessage rewardsMessage);
    }
}
