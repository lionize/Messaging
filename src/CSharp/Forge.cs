using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<Forge> _logger;
        private readonly ISerializer<byte[]> _serializer;

        public Forge(
            IConfigurationRoot configurationRoot,
            ILogger<Forge> logger,
            ISerializer<byte[]> serializer,
            IDeserializer<byte[]> deserializer)
        {
            ConnectionFactory factory = new ConnectionFactory
            {
                Uri = new Uri(configurationRoot.GetConnectionString("RabbitMQ"))
            };

            _connectionFactory = factory;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            _deserializer = deserializer ?? throw new ArgumentNullException(nameof(deserializer));
            _producingChannel = Channel.CreateBounded<object>(1);
        }

        public async Task ConsumeAsync(CancellationToken cancellationToken)
        {
        }

        public async Task ProduceAsync(CancellationToken cancellationToken)
        {
            using (IConnection conn = _connectionFactory.CreateConnection())
            {
                using (IModel channel = conn.CreateModel())
                {
                    //channel.ExchangeDeclare();
                    //channel.BasicPublish()
                    //channel.BasicGet(, )

                    var message = await _producingChannel.Reader.ReadAsync(cancellationToken);

                    try
                    {
                        var basicProperties = channel.CreateBasicProperties();
                        basicProperties.AppId = "appid";
                        basicProperties.CorrelationId = "";
                        basicProperties.DeliveryMode = 2;
                        basicProperties.Persistent = true;
                        basicProperties.Type = message.GetType().FullName;

                        var body = _serializer.Serialize(message);

                        channel.BasicPublish("", "", mandatory: true, basicProperties, body);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, ex.Message);
                    }
                }
            }
        }

        public ValueTask ProduceAsync(object message, CancellationToken cancellationToken)
        {
            return _producingChannel.Writer.WriteAsync(message, cancellationToken);
        }
    }
}