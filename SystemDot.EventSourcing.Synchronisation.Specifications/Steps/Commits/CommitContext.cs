using SystemDot.EventSourcing.Commits;

namespace SystemDot.Domain.Synchronisation.Specifications.Steps.Commits
{
    public class CommitContext
    {
        public Commit CommitInUse { get; set; }
    }
}