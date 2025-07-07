using Appointments.Application.Events;
using Appointments.API.Messaging;

namespace Appointments.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddSingleton<IEventPublisher, InMemoryEventPublisher>();
        return services;
    }
}
