using System;

namespace SystemDot.EventSourcing.Sessions
{
    public class EventContainer
    {
        public EventContainer(string aggregateRootId, SourcedEvent @event)
        {
            AggregateRootId = aggregateRootId;
            Event = @event;
        }

        public string AggregateRootId { get; private set; }

        public SourcedEvent Event { get; private set; }
    }
}