using SystemDot.EventSourcing.Sessions;
using SystemDot.EventSourcing.Sql.Windows.Lookups;
using EventStore;

namespace SystemDot.EventSourcing.Sql.Windows
{
    public class EventStoreEventSessionFactory : IEventSessionFactory
    {
        readonly IStoreEvents eventStore;
        readonly IAggregateLookup lookup;

        public EventStoreEventSessionFactory(IStoreEvents eventStore, IAggregateLookup lookup)
        {
            this.eventStore = eventStore;
            this.lookup = lookup;
        }

        public IEventSession Create()
        {
            var session = new EventStoreEventSession(eventStore, lookup);
            EventSessionProvider.Session = session;

            return session;
        }
    }
}