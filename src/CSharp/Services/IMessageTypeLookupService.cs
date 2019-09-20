using System;

namespace TIKSN.Lionize.Messaging.Services
{
    public interface IMessageTypeLookupService
    {
        string GetMessageName(Type type);
        string GetMessageName<TMessage>();
        Type GetMessageType(string name);
    }
}