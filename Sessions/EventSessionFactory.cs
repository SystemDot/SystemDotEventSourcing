using SystemDot.EventSourcing.Streams;

namespace SystemDot.EventSourcing.Sessions
{
    public class EventSessionFactory : IEventSessionFactory
    {
        readonly IEventStore store;
            
        public EventSessionFactory(IEventStore store)
        {
            this.store = store;
        }

        public IEventSession Create()
        {
            return new EventSession(store);
        }
    }
}