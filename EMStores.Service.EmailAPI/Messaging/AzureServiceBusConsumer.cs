using Azure.Messaging.ServiceBus;
using EMStore.Services.EmailAPI.Dtos.Cart;
using EMStore.Services.EmailAPI.Services;
using EMStore.Services.EmailAPI.Services.Interfaces;
using Newtonsoft.Json;
using System.Text;

namespace EMStore.Services.EmailAPI.Messaging
{
    public class AzureServiceBusConsumer: IAzureServiceBusConsumer
    {
        private readonly IConfiguration _config;

        private readonly string serviceBusConnectionString;
        private readonly string emailCartQueue;
        private ServiceBusProcessor _emailCartProcessor;
        private readonly IEmailService _emailService;

        public AzureServiceBusConsumer(IConfiguration config, EmailService emailService)
        {
            _config = config;
            serviceBusConnectionString = _config.GetValue<string>("ServiceBusConnectionString") ?? string.Empty;
            emailCartQueue = _config.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue") ?? string.Empty;

            var client = new ServiceBusClient(serviceBusConnectionString);

            // Processor is used to listen to queues or topics
            _emailCartProcessor = client.CreateProcessor(emailCartQueue);

            _emailService = emailService;

        }

        public async Task Start()
        {
            _emailCartProcessor.ProcessMessageAsync += OnEmailCartRequestReceived;
            _emailCartProcessor.ProcessErrorAsync += ErrorHandler;
            await _emailCartProcessor.StartProcessingAsync();
        }

        public async Task Stop()
        {
            // Stop processing
            await _emailCartProcessor.StopProcessingAsync();

            await _emailCartProcessor.DisposeAsync();
        }

        private async Task OnEmailCartRequestReceived(ProcessMessageEventArgs args)
        {
            // Receive the message
            var message = args.Message;

            // Get the body of the message as string
            var body = Encoding.UTF8.GetString(message.Body);

            // Deserialize message
            CartDto cartDto = JsonConvert.DeserializeObject<CartDto>(body) ?? new CartDto();

            try
            {
                //TODO Try to send out or log the email
                _emailService.EmailCartAndLog(cartDto);

                // This line tells the service bus that the message has been successfully processed and can be removed from the queue
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
