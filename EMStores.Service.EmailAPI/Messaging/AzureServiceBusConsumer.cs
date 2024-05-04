using Azure.Messaging.ServiceBus;

namespace EMStore.Services.EmailAPI.Messaging
{
    public class AzureServiceBusConsumer 
    {
        private readonly IConfiguration _config;

        private readonly string serviceBusConnectionString;
        private readonly string emailCartQueue;

        public AzureServiceBusConsumer(IConfiguration config)
        {
            _config = config;
            serviceBusConnectionString = _config.GetValue<string>("ServiceBusConnectionString") ?? string.Empty;
            emailCartQueue = _config.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue") ?? string.Empty;

            var client = new ServiceBusClient(serviceBusConnectionString);


        }
    }
}
