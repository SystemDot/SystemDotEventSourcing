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
        public bool Exists(string aggregateRootId)
        {
            IEnumerable<SourcedEvent> events = GetEvents(aggregateRootId);
            return events.Any();
        }

        public TAggregateRoot Get<TAggregateRoot>(string aggregateRootId)
            where TAggregateRoot : AggregateRoot, new()
        {
            IEnumerable<SourcedEvent> events = GetEvents(aggregateRootId);
            
            var aggregateRoot = new TAggregateRoot();

            AggregateRoot.SetId(aggregateRoot, aggregateRootId);

            events
                .Select(e => e.Body)
                .ForEach(aggregateRoot.ReplayEvent);

            return aggregateRoot;
        }

        static IEnumerable<SourcedEvent> GetEvents(string aggregateRootId)
        {
            IEnumerable<SourcedEvent> sourcedEvents = EventSessionProvider.Session.GetEvents(aggregateRootId);
            return sourcedEvents.ToList();
        }
    }
}