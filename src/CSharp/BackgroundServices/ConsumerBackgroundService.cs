using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TIKSN.Integration.Correlation;
using TIKSN.Lionize.Messaging.Handlers;
using TIKSN.Lionize.Messaging.Providers;
using TIKSN.Lionize.Messaging.Services;
using TIKSN.Serialization;

namespace TIKSN.Lionize.Messaging.BackgroundServices
{
    public class ConsumerBackgroundService<TMessage> : BackgroundService
    {
        private readonly ICachedConnectionProvider _cachedConnectionProvider;
        private readonly IConsumerMessageHandler<TMessage> _consumerMessageHandler;
        private readonly ICorrelationService _correlationService;
        private readonly IDeserializer<byte[]> _deserializer;
        private readonly ILogger<ConsumerBackgroundService<TMessage>> _logger;
        private readonly IMessageTypeLookupService _messageTypeLookupService;

        public ConsumerBackgroundService(
            ICachedConnectionProvider cachedConnectionProvider,
            IMessageTypeLookupService messageTypeLookupService,
            IDeserializer<byte[]> deserializer,
            IConsumerMessageHandler<TMessage> consumerMessageHandler,
            ICorrelationService correlationService,
            ILogger<ConsumerBackgroundService<TMessage>> logger)
        {
            _cachedConnectionProvider = cachedConnectionProvider ?? throw new ArgumentNullException(nameof(cachedConnectionProvider));
            _messageTypeLookupService = messageTypeLookupService ?? throw new ArgumentNullException(nameof(messageTypeLookupService));
            _deserializer = deserializer ?? throw new ArgumentNullException(nameof(deserializer));
            _consumerMessageHandler = consumerMessageHandler ?? throw new ArgumentNullException(nameof(consumerMessageHandler));
            _correlationService = correlationService ?? throw new ArgumentNullException(nameof(correlationService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var connection = _cachedConnectionProvider.GetConnection())
            {
                var queueName = _messageTypeLookupService.GetMessageQueue<TMessage>();
                _logger.LogInformation($"Queue name is estimated to be {queueName}");

                using (var channel = connection.Connection.CreateModel())
                {
                    //TODO: setup exchange and channel connection
                }

                using (var channel = connection.Connection.CreateModel())
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        var messageResult = channel.BasicGet(queueName, autoAck: false);

                        if (messageResult != null)
                        {
                            try
                            {
                                var message = _deserializer.Deserialize<TMessage>(messageResult.Body);
                                var correlationId = _correlationService.Create(messageResult.BasicProperties.CorrelationId);

                                await _consumerMessageHandler.HandleAsync(message, correlationId, stoppingToken);

                                channel.BasicAck(messageResult.DeliveryTag, multiple: false);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, ex.Message);
                            }
                        }
                    }
                }
            }
        }
    }
}