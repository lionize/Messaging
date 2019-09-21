using Lionize.IntegrationMessages;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using TIKSN.Lionize.Messaging.Options;

namespace TIKSN.Lionize.Messaging.Services
{
    public class MessageTypeLookupService : IMessageTypeLookupService
    {
        private readonly IOptions<ApplicationOptions> _applicationOptions;
        private readonly Dictionary<string, Type> _nameToType;
        private readonly Dictionary<Type, string> _typeToName;

        public MessageTypeLookupService(IOptions<ApplicationOptions> applicationOptions)
        {
            _typeToName = new Dictionary<Type, string>();

            Add<TaskUpserted>("task_upserted");
            Add<TaskCompletionChnaged>("task_completion_chnaged");
            Add<SubtaskCompletionChnaged>("subtask_completion_chnaged");

            _nameToType = _typeToName.ToDictionary(k => k.Value, v => v.Key);
            _applicationOptions = applicationOptions ?? throw new ArgumentNullException(nameof(applicationOptions));
        }

        public string GetMessageExchange(Type type) => GetMessageExchange(GetMessageName(type));

        public string GetMessageExchange(string name) => $"{name}_exchange";

        public string GetMessageName(Type type) => _typeToName[type];

        public string GetMessageName<TMessage>() => GetMessageName(typeof(TMessage));

        public IEnumerable<string> GetMessageNames() => _nameToType.Keys;

        public string GetMessageQueue(Type type) => $"{GetMessageName(type)}_{_applicationOptions.Value.ApplictionQueuePart}_queue";

        public string GetMessageQueue<TMessage>() => GetMessageQueue(typeof(TMessage));

        public Type GetMessageType(string name) => _nameToType[name];

        private void Add<T>(string name)
        {
            _typeToName.Add(typeof(T), name);
        }
    }
}