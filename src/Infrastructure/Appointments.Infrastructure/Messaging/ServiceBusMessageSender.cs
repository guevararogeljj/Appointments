using Appointments.Application.Services;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Appointments.Infrastructure.Messaging
{
    public class ServiceBusMessageSender : IMessageSender
    {
        private readonly ServiceBusClient _client;
        private readonly string _queueName;

        public ServiceBusMessageSender(IConfiguration configuration)
        {
            var connectionString = configuration.GetSection("ServiceBus:ConnectionString");
            _queueName = configuration["ServiceBus:QueueName"];
            _client = new ServiceBusClient(connectionString.Value);
        }

        public async Task SendMessageAsync(string messageBody)
        {
            try
            {
                await using var sender = _client.CreateSender(_queueName);
                var message = new ServiceBusMessage(messageBody);
                await sender.SendMessageAsync(message);
            }
            catch (ServiceBusException ex)
            {
                // Handle Service Bus specific exceptions
                // Log the exception or rethrow as needed
                throw new Exception("An error occurred while sending the message to Service Bus.", ex);
            }
            catch (Exception ex)
            {
                // Handle general exceptions
                // Log the exception or rethrow as needed
                throw new Exception("An unexpected error occurred while sending the message.", ex);
            }
        }
    }
}

