using System.Text;
using Appointments.Infrastructure.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

class Program
{
    public static async Task Main(string[] args)
    {
        try
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            using var serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddSingleton<IConfiguration>(configuration)
                .AddSingleton<RabbitMQConnection>()
                .BuildServiceProvider();

            var logger = serviceProvider.GetRequiredService<ILogger<RabbitMQConsumer>>();
            var consumer = new RabbitMQConsumer(configuration, logger);

            Console.WriteLine("Iniciando el consumidor de RabbitMQ...");
            await consumer.StartAsync();
            Console.WriteLine("Consumidor finalizado.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error al iniciar el consumidor: {ex.Message}");
        }
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