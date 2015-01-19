using System;
using System.Collections.Generic;
using System.Linq;
using SystemDot.EventSourcing.Aggregation;
using SystemDot.EventSourcing.Sessions;
using SystemDot.EventSourcing.Streams;

namespace SystemDot.EventSourcing
{
    using SystemDot.Environment;
    using SystemDot.EventSourcing.Headers;

    public class DomainRepository : IDomainRepository
    {
        readonly IEventSessionFactory eventSessionFactory;
        private readonly ILocalMachine localMachine;

        public DomainRepository(IEventSessionFactory eventSessionFactory, ILocalMachine localMachine)
        {
            this.eventSessionFactory = eventSessionFactory;
            this.localMachine = localMachine;
        }

        public bool Exists(string aggregateRootId)
        {
            return GetEvents(aggregateRootId).Any();
        }

        public TAggregateRoot Get<TAggregateRoot>(string aggregateRootId)
            where TAggregateRoot : AggregateRoot, new()
        {
            var aggregateRoot = new TAggregateRoot();
            aggregateRoot.Rehydrate(aggregateRootId, GetEvents(aggregateRootId));
            return aggregateRoot;
        }

        public void Save<TAggregateRoot>(TAggregateRoot aggregateRoot)
            where TAggregateRoot : AggregateRoot
        {
            using (IEventSession eventSession = eventSessionFactory.Create())
            {
                foreach (SourcedEvent @event in aggregateRoot.EventsAdded)
                {
                    eventSession.StoreEvent(@event, aggregateRoot.Id);
                    eventSession.StoreHeader(aggregateRoot.Id, EventOriginHeader.Key, EventOriginHeader.ForMachine(localMachine));
                    eventSession.StoreHeader(aggregateRoot.Id, AggregateHeader.Key, AggregateHeader.FromType(aggregateRoot.GetType()));
                }

                eventSession.Commit(Guid.NewGuid());
            }
        }

        IEnumerable<SourcedEvent> GetEvents(string aggregateRootId)
        {
            return eventSessionFactory.Create().GetEvents(aggregateRootId).ToList();
        }
    }
}