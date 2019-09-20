using Lionize.IntegrationMessages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TIKSN.Lionize.Messaging.Services
{
    public class MessageTypeLookupServices
    {
        private readonly Dictionary<string, Type> _nameToType;
        private readonly Dictionary<Type, string> _typeToName;

        public MessageTypeLookupServices()
        {
            _typeToName = new Dictionary<Type, string>();

            Add<TaskUpserted>("task_upserted");
            Add<TaskCompletionChnaged>("task_completion_chnaged");
            Add<SubtaskCompletionChnaged>("subtask_completion_chnaged");

            _nameToType = _typeToName.ToDictionary(k => k.Value, v => v.Key);
        }

        public string GetMessageName(Type type) => _typeToName[type];

        public Type GetMessageType(string name) => _nameToType[name];

        private void Add<T>(string name)
        {
            _typeToName.Add(typeof(T), name);
        }
    }
}