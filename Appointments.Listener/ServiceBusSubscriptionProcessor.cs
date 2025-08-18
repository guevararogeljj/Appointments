using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Appointments.Listener
{
    public class ServiceBusSubscriptionProcessor : IHostedService
    {
        private readonly ServiceBusProcessor _processor;
        private readonly ILogger<ServiceBusSubscriptionProcessor> _logger;
        private readonly int _maxRetries = 3;

        public ServiceBusSubscriptionProcessor(IConfiguration configuration, ILogger<ServiceBusSubscriptionProcessor> logger)
        {
            _logger = logger;
            var connectionString = configuration["ServiceBus:ConnectionString"];
            var topicName = configuration["ServiceBus:TopicName"];
            var subscriptionName = configuration["ServiceBus:SubscriptionName"];

            var client = new ServiceBusClient(connectionString);
            _processor = client.CreateProcessor(topicName, subscriptionName, new ServiceBusProcessorOptions
            {
                AutoCompleteMessages = false,
                MaxConcurrentCalls = 1,
                ReceiveMode = ServiceBusReceiveMode.PeekLock,
                SubQueue = SubQueue.None,
                MaxAutoLockRenewalDuration = TimeSpan.FromMinutes(5),
                PrefetchCount = 10,
                Identifier = String.Empty,
                
            });
            _processor.ProcessMessageAsync += MessageHandler;
            _processor.ProcessErrorAsync += ErrorHandler;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Iniciando el consumidor de Service Bus.");
            await _processor.StartProcessingAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deteniendo el consumidor de Service Bus.");
            await _processor.StopProcessingAsync(cancellationToken);
        }

        private async Task MessageHandler(ProcessMessageEventArgs args)
        {
            string body = args.Message.Body.ToString();
            string messageId = args.Message.MessageId;
            _logger.LogInformation($"Mensaje recibido: {body} | MessageId: {messageId}");

            try
            {
                // Manejo de duplicados usando MessageId
                // Aquí podrías consultar una base de datos o caché para saber si el MessageId ya fue procesado
                // Ejemplo simple:
                // if (YaProcesado(messageId)) { await args.CompleteMessageAsync(args.Message); return; }

                // Procesar el mensaje
                // ... lógica de negocio ...

                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error procesando el mensaje: {body}");
                if (args.Message.DeliveryCount >= _maxRetries)
                {
                    _logger.LogWarning($"El mensaje ha alcanzado el número máximo de reintentos. Enviando a la DLQ.");
                    await args.DeadLetterMessageAsync(args.Message, "Máximo de reintentos alcanzado");
                }
                else
                {
                    await args.AbandonMessageAsync(args.Message);
                }
            }
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            _logger.LogError(args.Exception, "Error en el consumidor de Service Bus.");
            return Task.CompletedTask;
        }
    }
}

