namespace SystemDot.EventSourcing.Synchronisation
{
    using System.Collections.Generic;
    using System.Linq;
    using SystemDot.Core;
    using SystemDot.EventSourcing.Commits;
    using SystemDot.EventSourcing.Headers;
    using SystemDot.Serialisation;

    public static class CommitExtensions
    {
        public static List<SynchronisableSourcedEvent> GetSynchronisableEvents(this Commit commit)
        {
            return commit.Events
                .Select(e => new SynchronisableSourcedEvent { Body = new JsonSerialiser().Serialise(e.Body) })
                .ToList();
        }

        public static List<SynchronisableCommitHeader> GetCommitHeaders(this Commit commit)
        {
            return commit.Headers
                .Select(h => new SynchronisableCommitHeader { Key = h.Key, Value = new JsonSerialiser().Serialise(h.Value) })
                .ToList();
        }

        public static bool OriginatesOnMachineNamed(this Commit commit, string name)
        {
            return commit.Headers[EventOriginHeader.Key].As<EventOriginHeader>().MachineName == name;
        }
    }
}