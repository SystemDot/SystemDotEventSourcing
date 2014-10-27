using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SystemDot.EventSourcing.Sessions
{
    public interface IEventSession : IDisposable
    {
        Task<IEnumerable<SourcedEvent>> GetEventsAsync(string streamId);

        void StoreEvent(SourcedEvent @event, string aggregateRootId);

        Task CommitAsync();

        Task<IEnumerable<SourcedEvent>> AllEventsAsync();
    }
}