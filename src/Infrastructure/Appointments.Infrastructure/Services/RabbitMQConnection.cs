using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace Appointments.Infrastructure.Services;

public class RabbitMQConnection
{
    private static RabbitMQConnection _instance;
    private static readonly object _lock = new object();
    private readonly IConnection _connection;

    private RabbitMQConnection(IConfiguration configuration)
    {
        var factory = new ConnectionFactory()
        {
            HostName = configuration["RabbitMQ:HostName"],
            Port = int.Parse(configuration["RabbitMQ:Port"]),
            UserName = configuration["RabbitMQ:UserName"],
            Password = configuration["RabbitMQ:Password"],
            VirtualHost = configuration["RabbitMQ:VirtualHost"]
        };
        _connection = factory.CreateConnection();
    }

    public static RabbitMQConnection GetInstance(IConfiguration configuration)
    {
        if (_instance == null)
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new RabbitMQConnection(configuration);
                }
            }
        }

        return _instance;
    }

    public IConnection GetConnection() => _connection;
}