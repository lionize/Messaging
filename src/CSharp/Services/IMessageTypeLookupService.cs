using System;
using System.Collections.Generic;

namespace TIKSN.Lionize.Messaging.Services
{
    public interface IMessageTypeLookupService
    {
        string GetMessageExchange<TMessage>();

        string GetMessageExchange(Type type);

        string GetMessageExchange(string name);

        string GetMessageName(Type type);

        string GetMessageName<TMessage>();

        IEnumerable<string> GetMessageNames();

        string GetMessageQueue(Type type);

        string GetMessageQueue<TMessage>();

        Type GetMessageType(string name);
    }
}