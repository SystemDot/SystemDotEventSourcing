namespace SystemDot.EventSourcing.Aggregation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SystemDot.Core.Collections;
    using SystemDot.Domain;
    using SystemDot.EventSourcing.Streams;

    public abstract class EventSourcedEntity
    {
        readonly ConventionEventToHandlerRouter eventRouter;
        public EventHandler<EventSourceEventArgs> EventReplayed;
        MultiSiteId multiSiteId;

        protected EventSourcedEntity()
        {
            eventRouter = new ConventionEventToHandlerRouter(this, "ApplyEvent");
        }

        protected EventSourcedEntity(MultiSiteId multiSiteId) : this()
        {
            this.multiSiteId = multiSiteId;
        }

        internal EventStreamId GetEventStreamId()
        {
            return multiSiteId.ToEventStreamId();
        }

        internal void Rehydrate(MultiSiteId id, IEnumerable<SourcedEvent> events)
        {
            multiSiteId = id;
            int index = 0;

            foreach (var sourcedEvent in events)
            {
                index++;
                ReplayEvent(sourcedEvent.Body, index);
            }
        }

        protected virtual void ReplayEvent(object toReplay, int index)
        {
            eventRouter.RouteEventToHandlers(toReplay);
            OnEventReplayed(toReplay);
        }

        void OnEventReplayed(object @event)
        {
            if (EventReplayed != null)
            {
                EventReplayed(this, new EventSourceEventArgs(@event));
            }
        }
    }
}