using System;
using System.Collections.Generic;
using SystemDot.Core;

namespace SystemDot.EventSourcing.Sessions
{
    public class SourcedEvent
    {
        public Dictionary<string, object> Headers { get; private set; }

        public object Body { get; set; }

        public SourcedEvent()
        {
            Headers = new Dictionary<string, object>();
        }

        public void AddHeader(string key, object value)
        {
            Headers.Add(key, value);
        }

        public T GetHeader<T>(string key)
        {
            return Headers[key].As<T>();
        }
    }
}