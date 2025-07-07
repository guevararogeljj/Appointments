using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

class Program
{
    public static async Task Main(string[] args)
    {
        var consumer = new RabbitMQConsumer();
        await consumer.StartAsync();
    }
}

public class RabbitMQConsumer
{
    public async Task StartAsync()
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            Port = 5672,
            UserName = "miusuario",
            Password = "miclave"
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "Donouts", durable: false, exclusive: false, autoDelete: false, arguments: null);
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
}