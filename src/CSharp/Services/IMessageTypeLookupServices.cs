using System;

namespace TIKSN.Lionize.Messaging.Services
{
    public interface IMessageTypeLookupServices
    {
        string GetMessageName(Type type);
        Type GetMessageType(string name);
    }
}