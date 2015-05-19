namespace SystemDot.EventSourcing.Synchronisation.Server
{
    using System.Threading.Tasks;
    using SystemDot.Core.Collections;
    using SystemDot.Domain.Commands;

    public class CommitSynchronisableCommitsHandler : IAsyncCommandHandler<CommitSynchronisableCommits>
    {
        readonly SynchronisableCommitSynchroniser synchronisableCommitSynchroniser;

        public CommitSynchronisableCommitsHandler(SynchronisableCommitSynchroniser synchronisableCommitSynchroniser)
        {
            this.synchronisableCommitSynchroniser = synchronisableCommitSynchroniser;
        }

        public Task Handle(CommitSynchronisableCommits message)
        {
            message.Synchronisation.Commits.ForEach(synchronisableCommitSynchroniser.SynchroniseCommit);
            return Task.FromResult(false);
        }
    }
}