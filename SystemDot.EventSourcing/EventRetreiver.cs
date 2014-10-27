using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemDot.EventSourcing.Sessions;

namespace SystemDot.EventSourcing
{
    public class EventRetreiver
    {
        readonly IEventSessionFactory factory;

        public EventRetreiver(IEventSessionFactory factory)
        {
            this.factory = factory;
        }

        public async Task<IEnumerable<SourcedEvent>> GetAllEventsAsync()
        {
            var allEvents = await factory.Create().AllEventsAsync();
            return allEvents.ToList();
        }
    }
}