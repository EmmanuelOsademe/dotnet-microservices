using Azure.Messaging.ServiceBus;
using EMStore.Services.RewardAPI.Message;
using EMStore.Services.RewardAPI.Services;
using Newtonsoft.Json;
using System.Text;

namespace EMStore.Services.RewardAPI.Messaging
{
    public class AzureServiceBusConsumer: IAzureServiceBusConsumer
    {
        private readonly IConfiguration _config;

        private readonly string serviceBusConnectionString;
        private readonly string orderCreatedTopic;
        private readonly string orderCreatedRewardsSubscription;
        private ServiceBusProcessor _updateOrderProcessor;

        private readonly RewardService _rewardService;

        public AzureServiceBusConsumer(IConfiguration config, RewardService rewardService)
        {
            _config = config;

            // Create the client
            serviceBusConnectionString = _config.GetValue<string>("ServiceBusConnectionString") ?? string.Empty;
            var client = new ServiceBusClient(serviceBusConnectionString);

            // Create the rewards processor
            orderCreatedTopic = _config.GetValue<string>("TopicAndQueueNames:OrderCreatedTopic") ?? string.Empty;
            orderCreatedRewardsSubscription = _config.GetValue<string>("TopicAndQueueNames:OrderCreatedRewardsSubscription") ?? string.Empty;
            _updateOrderProcessor = client.CreateProcessor(orderCreatedTopic, orderCreatedRewardsSubscription);

            _rewardService = rewardService;
        }

        public async Task Start()
        {
            _updateOrderProcessor.ProcessMessageAsync += OnOrderCreatedRequestReceived;
            _updateOrderProcessor.ProcessErrorAsync += ErrorHandler;

            await _updateOrderProcessor.StartProcessingAsync();
        }

        public async Task Stop()
        {
            // Stop processing
            await _updateOrderProcessor.StopProcessingAsync();

            await _updateOrderProcessor.DisposeAsync();
        }

        private async Task OnOrderCreatedRequestReceived(ProcessMessageEventArgs args)
        {
            // Receive the message
            var message = args.Message;

            // Get the body of the message as string
            var body = Encoding.UTF8.GetString(message.Body);

            // Deserialize message
            RewardsMessage rewardsMessage = JsonConvert.DeserializeObject<RewardsMessage>(body) ?? new RewardsMessage();

            try
            {
                await _rewardService.UpdateRewards(rewardsMessage);

                // This line tells the service bus that the message has been successfully processed and can be removed from the subscription
                await args.CompleteMessageAsync(args.Message);
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            // Typically you send out an email when an exception is thrown or an error is encounter
            // That way, the team knows and tries to tackle it.
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }
    }
}
