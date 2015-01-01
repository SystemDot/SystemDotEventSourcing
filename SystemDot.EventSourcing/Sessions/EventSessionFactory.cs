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
            EventSessionProvider.Session = new EventSession(store);
            return EventSessionProvider.Session;
        }
    }
}