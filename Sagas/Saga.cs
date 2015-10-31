namespace SystemDot.EventSourcing.Sagas
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SystemDot.EventSourcing.Aggregation;
    using SystemDot.Reflection;

    public abstract class Saga : EventSourcedEntity
    {
        readonly List<object> sentCommands;

        public IEnumerable<object> SentCommands { get { return sentCommands; } }
        public int StartedAt { get; set; }

        protected Saga()
        {
            sentCommands = new List<object>();
        }

        protected void Then<TCommand>(TCommand command)
        {
            sentCommands.Add(command);
        }

        protected void Then<TCommand>(Action<TCommand> initaliseEvent) where TCommand : new()
        {
            var @event = new TCommand();
            initaliseEvent(@event);
            Then(@event);
        }

        protected override void ReplayEvent(object toReplay, int index)
        {
            if (GetType().GetInterfaces().Any(i => i.GetGenericTypeDefinition() == typeof(IStartSaga<>) && i.GenericTypeArguments.First() == toReplay.GetType()))
            {
                StartedAt = index;
            }
            base.ReplayEvent(toReplay, index);
        }
    }
}