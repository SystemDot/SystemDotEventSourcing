using System;
using System.Collections.Generic;
using System.Linq;
using SystemDot.Core.Collections;
using SystemDot.EventSourcing.Aggregation;
using SystemDot.EventSourcing.Sessions;
using SystemDot.EventSourcing.Streams;

namespace SystemDot.EventSourcing
{
    public class DomainRepository : IDomainRepository
    {
        readonly IEventSessionFactory eventSessionFactory;

        public DomainRepository(IEventSessionFactory eventSessionFactory)
        {
            this.eventSessionFactory = eventSessionFactory;
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