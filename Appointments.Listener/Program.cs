using System.Text;
using Appointments.Infrastructure.Services;
using Appointments.Listener;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

class Program
{
    public static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(config =>
            {
                config.AddJsonFile("appsettings.json", optional: true)
                    .AddEnvironmentVariables();
            })
            .ConfigureServices((context, services) =>
            {
                services.AddLogging();
                services.AddSingleton<RabbitMQConnection>(provider =>
                {
                    var configuration = provider.GetRequiredService<IConfiguration>();
                    return new RabbitMQConnection(configuration);
                });
                services.AddSingleton<RabbitMQConsumer>();
                services.AddHostedService<ServiceBusSubscriptionProcessor>();
            })
            .Build();

        // Inicia el host (ServiceBusSubscriptionProcessor)
        await host.StartAsync();

        Console.WriteLine("Presiona una tecla para salir...");
        Console.ReadLine();

        await host.StopAsync();
        // Inicia el consumidor de RabbitMQ en paralelo
        var rabbitConsumer = host.Services.GetRequiredService<RabbitMQConsumer>();
        var rabbitTask = rabbitConsumer.StartAsync();


    }
}

public class RabbitMQConsumer
{
    private readonly IConnection _connection;
    private readonly ILogger<RabbitMQConsumer> _logger;
    public RabbitMQConsumer(IConfiguration configuration, ILogger<RabbitMQConsumer> logger)
    {
        _connection = RabbitMQConnection.GetInstance(configuration).GetConnection();
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    public async Task StartAsync()
    {
        try
        {
            using var channel = _connection.CreateModel();
            channel.QueueDeclare(queue: "Donouts", durable: false, exclusive: false, autoDelete: false,
                arguments: null);
            Console.WriteLine("Esperando...");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.Write($"[x] Mensaje recibido {message}{Environment.NewLine}");
            };

            channel.BasicConsume(queue: "Donouts", autoAck: true, consumer: consumer);
            Console.WriteLine("Presiona una tecla para salir...");
            Console.ReadLine();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al iniciar el consumidor de RabbitMQ");
        }
    }
}