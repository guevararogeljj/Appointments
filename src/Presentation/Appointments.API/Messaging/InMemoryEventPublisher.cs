using System.Text;
using Appointments.Application.Events;
using RabbitMQ.Client;
using System.Text.Json;
using Appointments.Infrastructure.Services;

namespace Appointments.API.Messaging;

public class InMemoryEventPublisher : IEventPublisher
{
    private readonly ILogger<InMemoryEventPublisher> _logger;
    private readonly IConnection _connection;

    public InMemoryEventPublisher(ILogger<InMemoryEventPublisher> logger, IConfiguration configuration)
    {
        _logger = logger;
        _connection = RabbitMQConnection.GetInstance(configuration).GetConnection();

    }

    public void Publish<T>(T @event) where T : class
    {
        using (var channel = _connection.CreateModel())
        {
            channel.QueueDeclare(queue: "Donouts", durable: false, exclusive: false, autoDelete: false, arguments: null);
            var message = JsonSerializer.Serialize(@event);
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: "", routingKey: "Donouts", basicProperties: null, body: body);
        }
        _logger.LogInformation($"Evento publicado en RabbitMQ: {typeof(T).Name}");
    }
}
