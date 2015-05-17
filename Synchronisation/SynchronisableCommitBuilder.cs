namespace SystemDot.EventSourcing.Synchronisation
{
    using System;
    using System.Collections.Generic;
    using SystemDot.Environment;
    using SystemDot.EventSourcing.Commits;
    using SystemDot.EventSourcing.Sessions;

    public class SynchronisableCommitBuilder
    {
        readonly IEventSessionFactory factory;

        public SynchronisableCommitBuilder(IEventSessionFactory factory)
        {
            this.factory = factory;
        }

        public IEnumerable<SynchronisableCommit> Build(string clientId, DateTime @from, Func<Commit, bool> commitsWhereClause)
        {
            return factory.Create().GetSynchronisableCommits(clientId, @from, commitsWhereClause);
        }
    }
}