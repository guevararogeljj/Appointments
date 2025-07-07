using Appointments.Application.Events;

namespace Appointments.API.Messaging;

public class InMemoryEventPublisher : IEventPublisher
{
    private readonly ILogger<InMemoryEventPublisher> _logger;

    public InMemoryEventPublisher(ILogger<InMemoryEventPublisher> logger)
    {
        _logger = logger;
    }

    public void Publish<T>(T @event) where T : class
    {
        _logger.LogInformation($"Publishing event {typeof(T).Name}: {@event.GetType().Name}");
        // In a real application, you would dispatch this to handlers or a message bus
    }
}
