using System.Threading.Tasks;
using SystemDot.EventSourcing.Sessions;

namespace SystemDot.EventSourcing.Sql.Windows
{
    using NEventStore;

    public class EventStoreEventSessionFactory : IEventSessionFactory
    {
        readonly IStoreEvents eventStore;

        public EventStoreEventSessionFactory(IStoreEvents eventStore)
        {
            this.eventStore = eventStore;
        }

        public async Task<IEventSession> CreateAsync()
        {
            var session = new EventStoreEventSession(eventStore);
            EventSessionProvider.Session = session;

            return await Task.FromResult(session);
        }
    }
}