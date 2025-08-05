using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Appointments.Application;

public static class HealthCheckConfiguration
{
    public static IServiceCollection AddHealthCheckServices(this IServiceCollection services, IConfiguration configuration)
    {
        try
        {
            var rabbitUser = configuration["RabbitMQ:UserName"];
            var rabbitPass = configuration["RabbitMQ:Password"];
            var rabbitHost = configuration["RabbitMQ:HostName"];
            var rabbitPort = configuration["RabbitMQ:Port"];
            var rabbitVHost = configuration["RabbitMQ:VirtualHost"];

            var rabbitConnectionString = $"amqp://{rabbitUser}:{rabbitPass}@{rabbitHost}:{rabbitPort}/{rabbitVHost}";

            services.AddHealthChecks()
                .AddCheck("Database", () =>
                {
                    return HealthCheckResult.Healthy("Database is healthy");
                })
                .AddRabbitMQ(
                    rabbitConnectionString,
                    name: "RabbitMQ",
                    tags: new[] { "rabbitmq" },
                    failureStatus: HealthStatus.Unhealthy);

            return services;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error configuring health checks: {ex.Message}", ex);
        }
    }
}