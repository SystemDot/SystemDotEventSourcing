using System;
using System.Collections.Generic;
using System.Linq;
using SystemDot.Core.Collections;
using SystemDot.EventSourcing.Streams;

namespace SystemDot.EventSourcing.Aggregation
{
    using SystemDot.Domain;

    public abstract class AggregateRoot<TState> : AggregateRoot
    {
        ConventionEventToHandlerRouter stateEventRouter;
        protected TState State { get; private set; }

        protected AggregateRoot(MultiSiteId multiSiteId)
            : base(multiSiteId)
        {
            InitialseState();
        }

        protected AggregateRoot()
        {
            InitialseState();
        }

        void InitialseState()
        {
            State = CreateState();
            stateEventRouter = new ConventionEventToHandlerRouter(State, "ApplyEvent");
        }

        protected abstract TState CreateState();

        protected override void ReplayEvent(object toReplay)
        {
            stateEventRouter.RouteEventToHandlers(toReplay);
            base.ReplayEvent(toReplay);
        }
    }

    public abstract class AggregateRoot
    {
        readonly ConventionEventToHandlerRouter eventRouter;
        public EventHandler<EventSourceEventArgs> EventReplayed;
        MultiSiteId multiSiteId;

        internal List<SourcedEvent> EventsAdded { get; private set; }

        internal EventStreamId GetEventStreamId()
        {
            return multiSiteId.ToEventStreamId();
        }
        
        protected AggregateRoot()
        {
            EventsAdded = new List<SourcedEvent>();
            eventRouter = new ConventionEventToHandlerRouter(this, "ApplyEvent");
        }

        protected AggregateRoot(MultiSiteId multiSiteId) : this()
        {
            this.multiSiteId = multiSiteId;
        }

        protected internal void Then<TEvent>(TEvent @event)
        {
            ReplayEvent(@event);
            StoreEvent(CreateSourcedEvent(@event));
        }

        protected internal void Then<TEvent>(Action<TEvent> initaliseEvent) where TEvent : new()
        {
            var @event = new TEvent();
            initaliseEvent(@event);
            Then(@event);
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

        internal void Rehydrate(MultiSiteId id, IEnumerable<SourcedEvent> events)
        {
            multiSiteId = id;

            events
                .Select(e => e.Body)
                .ForEach(ReplayEvent);
        }

        protected virtual void ReplayEvent(object toReplay)
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