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
            return new EventStoreEventSession(eventStore);
        }
    }
}