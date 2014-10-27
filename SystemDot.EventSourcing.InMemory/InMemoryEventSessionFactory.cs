using SystemDot.EventSourcing.Sessions;

namespace SystemDot.EventSourcing.InMemory
{
    public class InMemoryEventSessionFactory : IEventSessionFactory
    {
        readonly InMemoryEventSession eventSession;

        public InMemoryEventSessionFactory(InMemoryEventSession eventSession)
        {
            this.eventSession = eventSession;
        }

        public IEventSession Create()
        {
            EventSessionProvider.Session = eventSession;

            return eventSession;
        }
    }
}