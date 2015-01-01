using System;
using System.Collections.Generic;
using SystemDot.EventSourcing.Commits;

namespace SystemDot.EventSourcing.Streams
{
    public interface IEventStore
    {
        IEventStream OpenStream(string streamId);
        IEnumerable<Commit> GetCommitsFrom(DateTime @from);
    }
}