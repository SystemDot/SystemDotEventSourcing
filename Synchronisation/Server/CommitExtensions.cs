using System.Collections.Generic;
using System.Linq;
using SystemDot.EventSourcing.Commits;
using SystemDot.Serialisation;

namespace SystemDot.EventSourcing.Synchronisation.Server
{
    public static class CommitExtensions
    {
        public static List<SynchronisableSourcedEvent> GetSynchronisableEvents(this Commit commit)
        {
            return commit.Events
                .Select(e => new SynchronisableSourcedEvent { Body = new JsonSerialiser().Serialise(e.Body) })
                .ToList();
        }
    }
}