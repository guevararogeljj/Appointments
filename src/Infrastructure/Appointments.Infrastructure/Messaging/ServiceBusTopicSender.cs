using Appointments.Application.Services;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Appointments.Infrastructure.Messaging
{
    public class ServiceBusTopicSender : ITopicSender
    {
        private readonly ServiceBusClient _client;
        private readonly string _topicName;

        public ServiceBusTopicSender(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("ServiceBus");
            _topicName = configuration["ServiceBus:TopicName"];
            _client = new ServiceBusClient(connectionString);
        }

        public async Task SendMessageAsync(string messageBody)
        {
            await using var sender = _client.CreateSender(_topicName);
            var message = new ServiceBusMessage(messageBody);
            await sender.SendMessageAsync(message);
        }
    }
}

