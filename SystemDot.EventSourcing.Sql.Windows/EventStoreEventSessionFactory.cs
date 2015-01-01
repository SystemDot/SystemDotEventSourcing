using System.Threading.Tasks;
using SystemDot.EventSourcing.Sessions;

namespace SystemDot.EventSourcing.Sql.Windows
{
 
    public class EventStoreEventSessionFactory : IEventSessionFactory
    {
        readonly NEventStore.IStoreEvents eventStore;

        public EventStoreEventSessionFactory(NEventStore.IStoreEvents eventStore)
        {
            this.eventStore = eventStore;
        }

        public IEventSession Create()
        {
            var session = new EventStoreEventSession(eventStore);
            EventSessionProvider.Session = session;

            return session;
        }
    }
}