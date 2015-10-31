using System;
using System.Collections.Generic;
using System.Linq;
using SystemDot.EventSourcing.Aggregation;
using SystemDot.EventSourcing.Sessions;
using SystemDot.EventSourcing.Streams;
using SystemDot.Environment;
using SystemDot.EventSourcing.Headers;

namespace SystemDot.EventSourcing
{
    using SystemDot.Domain;

    public class DomainRepository : IDomainRepository
    {
        readonly IEventSessionFactory eventSessionFactory;
        private readonly ILocalMachine localMachine;

        public DomainRepository(IEventSessionFactory eventSessionFactory, ILocalMachine localMachine)
        {
            this.eventSessionFactory = eventSessionFactory;
            this.localMachine = localMachine;
        }

        public bool Exists(MultiSiteId id)
        {
            return GetEvents(id).Any();
        }

        public TEventSourcedEntity Get<TEventSourcedEntity>(MultiSiteId id)
            where TEventSourcedEntity : EventSourcedEntity, new()
        {
            var aggregateRoot = new TEventSourcedEntity();
            aggregateRoot.Rehydrate(id, GetEvents(id));
            return aggregateRoot;
        }

        public void Save<TAggregateRoot>(TAggregateRoot aggregateRoot)
            where TAggregateRoot : AggregateRoot
        {
            using (IEventSession eventSession = eventSessionFactory.Create())
            {
                foreach (SourcedEvent @event in aggregateRoot.EventsAdded)
                {
                    EventStreamId eventStreamId = aggregateRoot.GetEventStreamId();

                    eventSession.StoreEvent(@event, eventStreamId);
                    eventSession.StoreHeader(eventStreamId, EventOriginHeader.Key, EventOriginHeader.ForMachine(localMachine));
                }

                eventSession.Commit(Guid.NewGuid());
            }
        }

        IEnumerable<SourcedEvent> GetEvents(MultiSiteId aggregateRootId)
        {
            return eventSessionFactory.Create().GetEvents(aggregateRootId.ToEventStreamId()).ToList();
        }
    }
}