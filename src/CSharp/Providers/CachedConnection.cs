using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;

namespace TIKSN.Lionize.Messaging.Providers
{
    public class CachedConnection : IDisposable
    {
        private readonly ILogger _logger;

        public CachedConnection(IConnection connection, ILogger logger)
        {
            Connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IConnection Connection { get; }

        public void Dispose()
        {
            Connection.Dispose();
            _logger.LogInformation("Connection disposed");
        }
    }
}