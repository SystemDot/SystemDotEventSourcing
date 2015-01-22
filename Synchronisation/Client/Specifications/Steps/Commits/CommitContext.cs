using SystemDot.EventSourcing.Commits;

namespace SystemDot.Domain.Synchronisation.Client.Specifications.Steps.Commits
{
    public class CommitContext
    {
        public Commit CommitInUse { get; set; }
    }
}