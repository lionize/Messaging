using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Threading;
using System.Threading.Tasks;
using TIKSN.Lionize.Messaging.Providers;

namespace TIKSN.Lionize.Messaging.Services
{
    public class PublisherInitializerService : IPublisherInitializerService
    {
        private readonly ICachedConnectionProvider _cachedConnectionProvider;
        private readonly ILogger<PublisherInitializerService> _logger;
        private readonly IMessageTypeLookupService _messageTypeLookupService;

        public PublisherInitializerService(
            IMessageTypeLookupService messageTypeLookupService,
            ICachedConnectionProvider cachedConnectionProvider,
            ILogger<PublisherInitializerService> logger)
        {
            _messageTypeLookupService = messageTypeLookupService ?? throw new ArgumentNullException(nameof(messageTypeLookupService));
            _cachedConnectionProvider = cachedConnectionProvider ?? throw new ArgumentNullException(nameof(cachedConnectionProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InitializeAsync(CancellationToken cancellationToken)
        {
            using (var connection = _cachedConnectionProvider.GetConnection())
            {
                using (var channel = connection.Connection.CreateModel())
                {
                    foreach (var name in _messageTypeLookupService.GetMessageNames())
                    {
                        var exchange = _messageTypeLookupService.GetMessageExchange(name);
                        _logger.LogInformation($"Ensure existance of {exchange}");

                        cancellationToken.ThrowIfCancellationRequested();
                        channel.ExchangeDeclare(exchange, ExchangeType.Fanout, durable: true, autoDelete: false);
                    }
                }
            }
        }
    }
}