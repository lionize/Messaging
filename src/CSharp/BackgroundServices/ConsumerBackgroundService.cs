using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using TIKSN.Lionize.Messaging.Options;
using TIKSN.Lionize.Messaging.Providers;

namespace TIKSN.Lionize.Messaging.BackgroundServices
{
    public class ConsumerBackgroundService<TMessage> : BackgroundService
    {
        private readonly IOptions<ApplicationOptions> _applicationOptions;
        private readonly ICachedConnectionProvider _cachedConnectionProvider;

        public ConsumerBackgroundService(ICachedConnectionProvider cachedConnectionProvider, IOptions<ApplicationOptions> applicationOptions)
        {
            _cachedConnectionProvider = cachedConnectionProvider ?? throw new ArgumentNullException(nameof(cachedConnectionProvider));
            _applicationOptions = applicationOptions ?? throw new ArgumentNullException(nameof(applicationOptions));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var connection = _cachedConnectionProvider.GetConnection())
            {
            }
        }
    }
}