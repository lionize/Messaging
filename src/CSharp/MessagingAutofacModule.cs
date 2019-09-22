using Autofac;
using TIKSN.Integration.Correlation;
using TIKSN.Lionize.Messaging.Providers;
using TIKSN.Lionize.Messaging.Services;
using TIKSN.Serialization;
using TIKSN.Serialization.Bond;

namespace TIKSN.Lionize.Messaging
{
    public class MessagingAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<MessageTypeLookupService>()
                .As<IMessageTypeLookupService>()
                .SingleInstance();

            builder
                .RegisterType<CachedConnectionProvider>()
                .As<ICachedConnectionProvider>()
                .SingleInstance();

            builder
                .RegisterType<Base62CorrelationService>()
                .As<ICorrelationService>()
                .SingleInstance();

            builder
                .RegisterType<PublisherService>()
                .As<IPublisherService>()
                .SingleInstance();

            builder
                .RegisterType<PublisherInitializerService>()
                .As<IPublisherInitializerService>()
                .SingleInstance();

            builder
                .RegisterType<CompactBinaryBondDeserializer>()
                .As<IDeserializer<byte[]>>()
                .SingleInstance();

            builder
                .RegisterType<CompactBinaryBondSerializer>()
                .As<ISerializer<byte[]>>()
                .SingleInstance();
        }
    }
}