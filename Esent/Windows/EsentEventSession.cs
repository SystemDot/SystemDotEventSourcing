using System;
using System.Collections.Generic;
using SystemDot.EventSourcing.Commits;
using SystemDot.EventSourcing.Sessions;
using SystemDot.EventSourcing.Streams;

namespace SystemDot.EventSourcing.Esent.Windows
{
    public class EsentEventSession : IEventSession
    {
        public void Dispose()
        {
        }

        public IEnumerable<SourcedEvent> GetEvents(string streamId)
        {
            yield break;
        }

        public void StoreEvent(SourcedEvent @event, string id)
        {
        }

        public void Commit(Guid commitId)
        {
        }

        public IEnumerable<Commit> AllCommitsFrom(DateTime @from)
        {
            yield break;
        }
    }
}