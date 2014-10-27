using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SystemDot.Core.Collections;
using SystemDot.Reflection;

namespace SystemDot.EventSourcing.Aggregation
{
    public class ConventionEventToHandlerRouter
    {
        private readonly List<SourcedEventHandler> eventHandlers;
        private readonly string methodName;
        private readonly object target;

        public ConventionEventToHandlerRouter(object target, string methodName)
        {
            this.target = target;
            this.methodName = methodName;
            eventHandlers = new List<SourcedEventHandler>(GetEventHandlers());
        }

        public void RouteEventToHandlers(object @event)
        {
            foreach (SourcedEventHandler handler in eventHandlers)
            {
                if (handler.Handle(@event)) break;
            }
        }

        private IEnumerable<SourcedEventHandler> GetEventHandlers()
        {
            var handlers = new List<SourcedEventHandler>();

            GetEventHandlersFromTarget().ForEach(m => handlers.Add(CreateSourcedEventHandler(m)));

            return handlers;
        }

        private IEnumerable<MethodInfo> GetEventHandlersFromTarget()
        {
            return target.GetType()
                .GetMethods()
                .Where(m => m.Name == methodName && m.GetParameters().Length == 1);
        }

        private SourcedEventHandler CreateSourcedEventHandler(MethodInfo method)
        {
            return new SourcedEventHandler(GetFirstParameterType(method), GetHandler(method));
        }

        private Action<object> GetHandler(MethodInfo method)
        {
            return e => method.Invoke(
                target,
                new[]
                {
                    e
                });
        }

        private static Type GetFirstParameterType(MethodInfo method)
        {
            return method.GetParameters().First().ParameterType;
        }
    }
}