using Azure.Messaging.ServiceBus;
using EMStore.Services.EmailAPI.Dtos;
using EMStore.Services.EmailAPI.Dtos.Cart;
using EMStore.Services.EmailAPI.Message;
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

        private readonly string userRegistrationQueue;
        private ServiceBusProcessor _emailUserRegistrationProcessor;

        private readonly string orderCreatedTopic;
        private readonly string orderCreatedEmailSubscription;
        private ServiceBusProcessor _orderValidatedProcessor;

        private readonly IEmailService _emailService;

        public AzureServiceBusConsumer(IConfiguration config, EmailService emailService)
        {
            _config = config;

            // Create the client
            serviceBusConnectionString = _config.GetValue<string>("ServiceBusConnectionString") ?? string.Empty;
            var client = new ServiceBusClient(serviceBusConnectionString);

            // Create the emailCartProcessor
            emailCartQueue = _config.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue") ?? string.Empty;
            _emailCartProcessor = client.CreateProcessor(emailCartQueue);

            // Create the user registration processor
            userRegistrationQueue = _config.GetValue<string>("TopicAndQueueNames:EmailUserRegistrationQueue") ?? string.Empty;
            _emailUserRegistrationProcessor = client.CreateProcessor(userRegistrationQueue);

            // Create the order notification
            orderCreatedTopic = _config.GetValue<string>("TopicAndQueueNames:OrderCreatedTopic") ?? string.Empty;
            orderCreatedEmailSubscription = _config.GetValue<string>("TopicAndQueueNames:OrderCreatedEmailSubscription") ?? string.Empty;
            _orderValidatedProcessor = client.CreateProcessor(orderCreatedTopic, orderCreatedEmailSubscription);

            _emailService = emailService;

        }

        public async Task Start()
        {
            _emailCartProcessor.ProcessMessageAsync += OnEmailCartRequestReceived;
            _emailCartProcessor.ProcessErrorAsync += ErrorHandler;

            _emailUserRegistrationProcessor.ProcessMessageAsync += OnEmailUserRegistrationRequestReceived;
            _emailUserRegistrationProcessor.ProcessErrorAsync += ErrorHandler;

            _orderValidatedProcessor.ProcessMessageAsync += OnEmailOrderPlacedRequestReceived;
            _orderValidatedProcessor.ProcessErrorAsync += ErrorHandler;

            await _emailCartProcessor.StartProcessingAsync();
            await _emailUserRegistrationProcessor.StartProcessingAsync();
            await _orderValidatedProcessor.StartProcessingAsync();
        }

        public async Task Stop()
        {
            // Stop processing
            await _emailCartProcessor.StopProcessingAsync();
            await _emailUserRegistrationProcessor.StopProcessingAsync();
            await _orderValidatedProcessor.StopProcessingAsync();

            await _emailCartProcessor.DisposeAsync();
            await _emailUserRegistrationProcessor.DisposeAsync();
            await _orderValidatedProcessor.DisposeAsync();
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

        private async Task OnEmailUserRegistrationRequestReceived(ProcessMessageEventArgs args)
        {
            // Receive the message
            var message = args.Message;

            // Get the body of the message as string
            var body = Encoding.UTF8.GetString(message.Body);

            // Deserialize message
            UserDTO userDto = JsonConvert.DeserializeObject<UserDTO>(body) ?? new UserDTO();

            try
            {
                //TODO Try to send out or log the email
                _emailService.EmailUserRegistrationAndLog(userDto);

                // This line tells the service bus that the message has been successfully processed and can be removed from the queue
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task OnEmailOrderPlacedRequestReceived(ProcessMessageEventArgs args)
        {
            // Receive the message
            var message = args.Message;

            // Get the body of the message as string
            var body = Encoding.UTF8.GetString(message.Body);

            // Deserialize message
            RewardMessage rewardMessage = JsonConvert.DeserializeObject<RewardMessage>(body) ?? new RewardMessage();

            try
            {
                //TODO Try to send out or log the email
                _emailService.LogAndEmailPlacedOrder(rewardMessage);

                // This line tells the service bus that the message has been successfully processed and can be removed from the queue
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
