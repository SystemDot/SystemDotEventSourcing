using System.Threading.Tasks;
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

        public async Task<IEventSession> CreateAsync()
        {
            EventSessionProvider.Session = eventSession;
            return await Task.FromResult(eventSession);
        }
    }
}