using System;
using System.Collections.Generic;
using System.Linq;
using SystemDot.Core.Collections;
using SystemDot.EventSourcing.Streams;

namespace SystemDot.EventSourcing.Aggregation
{
    public abstract class AggregateRoot
    {
        readonly ConventionEventToHandlerRouter eventRouter;
        public EventHandler<EventSourceEventArgs> EventReplayed;
        AggregateRootId id;

        internal List<SourcedEvent> EventsAdded { get; private set; }

        internal EventStreamId GetEventStreamId()
        {
            return id.ToEventStreamId();
        }
        
        protected AggregateRoot()
        {
            EventsAdded = new List<SourcedEvent>();
            eventRouter = new ConventionEventToHandlerRouter(this, "ApplyEvent");
        }

        protected AggregateRoot(AggregateRootId id)
            : this()
        {
            this.id = id;
        }

        protected internal void AddEvent(object @event)
        {
            ReplayEvent(@event);
            StoreEvent(CreateSourcedEvent(@event));
        }

        void StoreEvent(SourcedEvent @event)
        {
            EventsAdded.Add(@event);
        }

        SourcedEvent CreateSourcedEvent(object body)
        {
            var sourcedEvent = new SourcedEvent
            {
                Body = body
            };

            return sourcedEvent;
        }

        internal void Rehydrate(AggregateRootId id, IEnumerable<SourcedEvent> events)
        {
            this.id = id;
            events
                .Select(e => e.Body)
                .ForEach(ReplayEvent);
        }

        void ReplayEvent(object toReplay)
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

        protected internal void AddEvent<T>(Action<T> initaliseEvent) where T : new()
        {
            var @event = new T();
            initaliseEvent(@event);
            AddEvent(@event);
        }
    }
}