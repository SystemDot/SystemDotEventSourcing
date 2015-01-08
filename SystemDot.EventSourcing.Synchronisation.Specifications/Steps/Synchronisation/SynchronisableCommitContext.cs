using SystemDot.EventSourcing.Synchronisation;

namespace SystemDot.Domain.Synchronisation.Specifications.Steps.Synchronisation
{
    public class SynchronisableCommitContext
    {
        public SynchronisableCommit CommitInUse { get; set; }
    }
}