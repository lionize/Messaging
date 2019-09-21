using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TIKSN.Integration.Correlation;
using TIKSN.Lionize.Messaging.Options;
using TIKSN.Lionize.Messaging.Providers;
using TIKSN.Serialization;

namespace TIKSN.Lionize.Messaging.Services
{
    public class PublisherService
    {
        private readonly IOptions<ApplicationOptions> _applicationOptions;
        private readonly ICachedConnectionProvider _cachedConnectionProvider;
        private readonly ILogger<PublisherService> _logger;
        private readonly IMessageTypeLookupService _messageTypeLookupService;
        private readonly ISerializer<byte[]> _serializer;

        public PublisherService(
            ICachedConnectionProvider cachedConnectionProvider,
            ISerializer<byte[]> serializer,
            IMessageTypeLookupService messageTypeLookupService,
            IOptions<ApplicationOptions> applicationOptions,
            ILogger<PublisherService> logger)
        {
            _cachedConnectionProvider = cachedConnectionProvider ?? throw new ArgumentNullException(nameof(cachedConnectionProvider));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            _messageTypeLookupService = messageTypeLookupService ?? throw new ArgumentNullException(nameof(messageTypeLookupService));
            _applicationOptions = applicationOptions ?? throw new ArgumentNullException(nameof(applicationOptions));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task ProduceAsync<TMessage>(TMessage message, CorrelationID correlationID, CancellationToken cancellationToken)
        {
            using (var connection = _cachedConnectionProvider.GetConnection())
            {
                using (var channel = connection.Connection.CreateModel())
                {
                    using (_logger.BeginScope(new Dictionary<string, string> { { nameof(CorrelationID), correlationID } }))
                    {
                        var exchangeName = _messageTypeLookupService.GetMessageExchange(message.GetType());

                        try
                        {
                            var basicProperties = channel.CreateBasicProperties();
                            basicProperties.AppId = _applicationOptions.Value.ApplictionId;
                            basicProperties.CorrelationId = correlationID;
                            basicProperties.DeliveryMode = 2;
                            basicProperties.Persistent = true;
                            basicProperties.Type = message.GetType().FullName;

                            var body = _serializer.Serialize(message);

                            channel.BasicPublish(exchangeName, routingKey: "", mandatory: true, basicProperties, body);
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