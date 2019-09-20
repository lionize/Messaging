using Autofac;
using TIKSN.Lionize.Messaging.Services;

namespace TIKSN.Lionize.Messaging
{
    public class MessagingAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<MessageTypeLookupServices>()
                .As<IMessageTypeLookupServices>()
                .SingleInstance();
        }
    }
}