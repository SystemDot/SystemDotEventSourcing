using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemDot.Core.Collections;
using SystemDot.EventSourcing.Aggregation;
using SystemDot.EventSourcing.Sessions;

namespace SystemDot.EventSourcing
{
    public class DomainRepository : IDomainRepository
    {
        public async Task<bool> ExistsAsync(string aggregateRootId)
        {
            IEnumerable<SourcedEvent> events = await GetEventsAsync(aggregateRootId);
            return events.Any();
        }

        public async Task<TAggregateRoot> GetAsync<TAggregateRoot>(string aggregateRootId)
            where TAggregateRoot : AggregateRoot, new()
        {
            List<SourcedEvent> events = await GetEventsAsync(aggregateRootId);

            if (events.Count == 0)
                throw new AggregateRootDoesNotExistException();
            
            var aggregateRoot = new TAggregateRoot();

            AggregateRoot.SetId(aggregateRoot, aggregateRootId);

            events
                .Select(e => e.Body)
                .ForEach(aggregateRoot.ReplayEvent);

            return aggregateRoot;
        }

        static async Task<List<SourcedEvent>> GetEventsAsync(string aggregateRootId)
        {
            IEnumerable<SourcedEvent> sourcedEvents = await EventSessionProvider.Session.GetEventsAsync(aggregateRootId);
            return sourcedEvents.ToList();
        }
    }
}