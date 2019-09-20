﻿using Lionize.IntegrationMessages;
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

            _nameToType = _typeToName.ToDictionary(k => k.Value, v => v.Key);
        }

        private void Add<T>(string name)
        {
            _typeToName.Add(typeof(T), name);
        }
    }
}