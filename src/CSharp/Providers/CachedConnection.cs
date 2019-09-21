using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;

namespace TIKSN.Lionize.Messaging.Providers
{
    public class CachedConnection : IDisposable
    {
        private readonly IDisposable _disposable;
        private readonly ILogger _logger;

        public CachedConnection(IConnection connection, IDisposable disposable, ILogger logger)
        {
            Connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _disposable = disposable ?? throw new ArgumentNullException(nameof(disposable));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <value>
        /// The connection.
        /// </value>
        /// <remarks>Do not dispose explicitly</remarks>
        public IConnection Connection { get; }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}