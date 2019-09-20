using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;

namespace TIKSN.Lionize.Messaging.Providers
{
    public class CachedConnectionProvider
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly object _connectionLocker;

        public CachedConnectionProvider(IConfigurationRoot configurationRoot)
        {
            _connectionLocker = new object();

            ConnectionFactory factory = new ConnectionFactory
            {
                Uri = new Uri(configurationRoot.GetConnectionString("RabbitMQ"))
            };

            _connectionFactory = factory;
        }

        public CachedConnection GetConnection()
        {
        }
    }
}