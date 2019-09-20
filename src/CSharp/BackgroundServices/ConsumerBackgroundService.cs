using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using TIKSN.Lionize.Messaging.Options;
using TIKSN.Lionize.Messaging.Providers;
using TIKSN.Lionize.Messaging.Services;

namespace TIKSN.Lionize.Messaging.BackgroundServices
{
    public class ConsumerBackgroundService<TMessage> : BackgroundService
    {
        private readonly IOptions<ApplicationOptions> _applicationOptions;
        private readonly ICachedConnectionProvider _cachedConnectionProvider;
        private readonly ILogger<ConsumerBackgroundService<TMessage>> _logger;
        private readonly IMessageTypeLookupService _messageTypeLookupService;

        public ConsumerBackgroundService(
            ICachedConnectionProvider cachedConnectionProvider,
            IMessageTypeLookupService messageTypeLookupService,
            IOptions<ApplicationOptions> applicationOptions,
            ILogger<ConsumerBackgroundService<TMessage>> logger)
        {
            _cachedConnectionProvider = cachedConnectionProvider ?? throw new ArgumentNullException(nameof(cachedConnectionProvider));
            _messageTypeLookupService = messageTypeLookupService ?? throw new ArgumentNullException(nameof(messageTypeLookupService));
            _applicationOptions = applicationOptions ?? throw new ArgumentNullException(nameof(applicationOptions));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var connection = _cachedConnectionProvider.GetConnection())
            {
                var queueName = $"{_messageTypeLookupService.GetMessageName<TMessage>()}_{_applicationOptions.Value.ApplictionQueuePart}_queue";
                _logger.LogInformation($"Queue name is extimated to be {queueName}");

                using (var channel = connection.Connection.CreateModel())
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        var messageResult = channel.BasicGet(queueName, autoAck: false);

                        if (messageResult != null)
                        {
                            try
                            {
                                //deserialize
                                //handle
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