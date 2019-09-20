using System;

namespace TIKSN.Lionize.Messaging.Services
{
    public interface IMessageTypeLookupService
    {
        string GetMessageName(Type type);

        string GetMessageName<TMessage>();

        string GetMessageQueue(Type type);

        string GetMessageQueue<TMessage>();

        Type GetMessageType(string name);
    }
}