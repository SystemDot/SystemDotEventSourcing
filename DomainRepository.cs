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

    public class DomainRepository : IDomainRepository
    {
        readonly IEventSessionFactory eventSessionFactory;
        private readonly ILocalMachine localMachine;

        public DomainRepository(IEventSessionFactory eventSessionFactory, ILocalMachine localMachine)
        {
            this.eventSessionFactory = eventSessionFactory;
            this.localMachine = localMachine;
        }

        public bool Exists(AggregateRootId aggregateRootId)
        {
            return GetEvents(aggregateRootId.ToEventStreamId()).Any();
        }

        public TAggregateRoot Get<TAggregateRoot>(AggregateRootId aggregateRootId)
            where TAggregateRoot : AggregateRoot, new()
        {
            var aggregateRoot = new TAggregateRoot();
            aggregateRoot.Rehydrate(aggregateRootId, GetEvents(aggregateRootId.ToEventStreamId()));
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
                    eventSession.StoreHeader(eventStreamId, AggregateHeader.Key, AggregateHeader.FromType(aggregateRoot.GetType()));
                }

                eventSession.Commit(Guid.NewGuid());
            }
        }

        IEnumerable<SourcedEvent> GetEvents(EventStreamId aggregateRootId)
        {
            return eventSessionFactory.Create().GetEvents(aggregateRootId).ToList();
        }
    }
}