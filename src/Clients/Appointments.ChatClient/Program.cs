using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

Console.WriteLine("Appointments Chat Client");

Console.Write("Enter your JWT Token: ");
var jwtToken = Console.ReadLine();

Console.Write("Enter Chat Room ID (GUID): ");
var chatRoomId = Console.ReadLine();

Console.Write("Enter your User ID (Sender ID): ");
var senderId = Console.ReadLine();

Console.Write("Enter Receiver User ID: ");
var receiverId = Console.ReadLine();

var connection = new HubConnectionBuilder()
    .WithUrl("https://localhost:7001/chatHub", options =>
    {
        options.AccessTokenProvider = () => Task.FromResult(jwtToken);
    })
    .ConfigureLogging(logging =>
    {
        logging.AddConsole();
        logging.SetMinimumLevel(LogLevel.Information);
    })
    .Build();

connection.On<Guid, string, string, string, DateTime>("ReceiveMessage", (room, sender, receiver, message, timestamp) =>
{
    Console.WriteLine($"[{timestamp:HH:mm:ss}] From {sender} to {receiver} in Room {room}: {message}");
});

connection.Closed += async (error) =>
{
    Console.WriteLine($"Connection closed: {error?.Message}");
    await Task.Delay(5000);
    await connection.StartAsync();
};

try
{
    await connection.StartAsync();
    Console.WriteLine("Connection started. Type your message and press Enter. Type 'exit' to quit.");

    while (true)
    {
        var message = Console.ReadLine();
        if (message.Equals("exit", StringComparison.OrdinalIgnoreCase))
        {
            break;
        }

        if (!string.IsNullOrEmpty(message))
        {
            await connection.InvokeAsync("SendMessage", Guid.Parse(chatRoomId), receiverId, message);
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
finally
{
    await connection.StopAsync();
    Console.WriteLine("Connection stopped.");
}
