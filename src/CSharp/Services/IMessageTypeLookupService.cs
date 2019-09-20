using System;

namespace TIKSN.Lionize.Messaging.Services
{
    public interface IMessageTypeLookupService
    {
        string GetMessageName(Type type);
        Type GetMessageType(string name);
    }
}