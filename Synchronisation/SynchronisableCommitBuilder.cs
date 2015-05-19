namespace SystemDot.EventSourcing.Synchronisation
{
    using System;
    using System.Linq;
    using SystemDot.EventSourcing.Commits;
    using SystemDot.EventSourcing.Sessions;

    public class SynchronisableCommitBuilder
    {
        readonly IEventSessionFactory factory;

        public SynchronisableCommitBuilder(IEventSessionFactory factory)
        {
            this.factory = factory;
        }

        public CommitSynchronisation Build(string clientId, DateTime @from, Func<Commit, bool> commitsWhereClause)
        {
            return new CommitSynchronisation
            {
                Commits = factory.Create()
                    .GetSynchronisableCommits(clientId, @from, commitsWhereClause)
                    .ToList()
            };
        }
    }
}