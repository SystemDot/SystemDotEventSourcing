using System;
using System.Reflection;

namespace SystemDot.EventSourcing.Aggregation
{
    public class SourcedEventHandler
    {        
        readonly Type eventType;
        readonly Action<object> handler;

        public SourcedEventHandler(Type eventType, Action<object> handler)
        {
            this.eventType = eventType;
            this.handler = handler;
        }

        public bool Handle(object toHandle)
        {
            if (eventType.GetTypeInfo().IsAssignableFrom(toHandle.GetType().GetTypeInfo()))
            {
                handler.Invoke(toHandle);
                return true;
            }

            return false;
        }
    }
}