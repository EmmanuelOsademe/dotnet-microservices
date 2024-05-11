namespace EMStore.Services.EmailAPI.Message
{
    public class RewardMessage
    {
        public string UserId { get; set; } = string.Empty;

        public int RewardsActivity { get; set; }

        public int OrderId { get; set; }
    }
}
