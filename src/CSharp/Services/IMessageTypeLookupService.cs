using System;
using System.Collections.Generic;

namespace TIKSN.Lionize.Messaging.Services
{
    public interface IMessageTypeLookupService
    {
        string GetMessageExchange(Type type);

        string GetMessageName(Type type);

        string GetMessageName<TMessage>();

        IEnumerable<string> GetMessageNames();

        string GetMessageQueue(Type type);

        string GetMessageQueue<TMessage>();

        Type GetMessageType(string name);
    }
}