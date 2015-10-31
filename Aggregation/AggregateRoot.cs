using System;
using System.Collections.Generic;
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

        protected override void ReplayEvent(object toReplay, int index)
        {
            stateEventRouter.RouteEventToHandlers(toReplay);
            base.ReplayEvent(toReplay, index);
        }
    }

    public abstract class AggregateRoot : EventSourcedEntity
    {
        internal List<SourcedEvent> EventsAdded { get; private set; }

        protected AggregateRoot()
        {
            EventsAdded = new List<SourcedEvent>();
        }

        protected AggregateRoot(MultiSiteId multiSiteId) : base(multiSiteId)
        {
            EventsAdded = new List<SourcedEvent>();
        }

        protected internal void Then<TEvent>(TEvent @event)
        {
            ReplayEvent(@event, 0);
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
    }
}