using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using TIKSN.Serialization;

namespace TIKSN.Lionize.Messaging
{
    public class Forge
    {
        private readonly IConnectionFactory _connectionFactory;
        //private readonly Channel<object> _consumingChannel;
        private readonly IDeserializer<byte[]> _deserializer;
        private readonly Channel<object> _producingChannel;
        private readonly ISerializer<byte[]> _serializer;

        public Forge(IConfigurationRoot configurationRoot, ISerializer<byte[]> serializer, IDeserializer<byte[]> deserializer)
        {
            ConnectionFactory factory = new ConnectionFactory
            {
                Uri = new Uri(configurationRoot.GetConnectionString("RabbitMQ"))
            };

            _connectionFactory = factory;
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            _deserializer = deserializer ?? throw new ArgumentNullException(nameof(deserializer));
            _producingChannel = Channel.CreateBounded<object>(1);
        }

        public async Task ConsumeAsync(CancellationToken cancellationToken)
        {
        }

        public void ProduceAsync(CancellationToken cancellationToken)
        {
            using (IConnection conn = _connectionFactory.CreateConnection())
            {
                using (IModel channel = conn.CreateModel())
                {
                    //channel.ExchangeDeclare();
                    //channel.BasicPublish()
                    //channel.BasicGet(, )
                }
            }
        }

        public async Task ProduceAsync(object message, CancellationToken cancellationToken)
        {
        }
    }
}