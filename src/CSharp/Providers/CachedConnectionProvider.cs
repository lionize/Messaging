using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Reactive.Disposables;

namespace TIKSN.Lionize.Messaging.Providers
{
    public class CachedConnectionProvider
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly object _connectionLocker;
        private readonly ILogger<CachedConnectionProvider> _logger;
        private CachedConnection _connection;
        private RefCountDisposable _connectionDisposable;

        public CachedConnectionProvider(IConfigurationRoot configurationRoot, ILogger<CachedConnectionProvider> logger)
        {
            _connectionLocker = new object();

            ConnectionFactory factory = new ConnectionFactory
            {
                Uri = new Uri(configurationRoot.GetConnectionString("RabbitMQ"))
            };

            _connectionFactory = factory;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public CachedConnection GetConnection()
        {
            if (_connection == null || _connectionDisposable.IsDisposed)
            {
                lock (_connectionLocker)
                {
                    if (_connection == null || _connectionDisposable.IsDisposed)
                    {
                        _connection = new CachedConnection(_connectionFactory.CreateConnection(), _logger);
                        _connectionDisposable = new RefCountDisposable(_connection, throwWhenDisposed: true);
                    }
                }
            }

            return _connection;
        }
    }
}