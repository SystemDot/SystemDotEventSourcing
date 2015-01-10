using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemDot.Core.Collections;
using SystemDot.Domain.Commands;
using SystemDot.EventSourcing.Sessions;
using SystemDot.EventSourcing.Synchronisation.Server;

namespace SystemDot.EventSourcing.Synchronisation.Client
{
    public class SynchroniseCommitsHandler : IAsyncCommandHandler<SynchroniseCommits>
    {
        readonly CommitRetrievalClient commitRetrievalClient;
        readonly IEventSessionFactory sessionFactory;

        public SynchroniseCommitsHandler(CommitRetrievalClient commitRetrievalClient, IEventSessionFactory sessionFactory)
        {
            this.commitRetrievalClient = commitRetrievalClient;
            this.sessionFactory = sessionFactory;
        }

        public async Task Handle(SynchroniseCommits message)
        {
            IEnumerable<SynchronisableCommit> commits = await commitRetrievalClient.GetCommitsAsync(new Uri(message.ServerUri));
            commits.ForEach(SynchroniseCommit);
        }

        void SynchroniseCommit(SynchronisableCommit toSynchronise)
        {
            using (var session = sessionFactory.Create())
            {
                toSynchronise.Events.ForEach(e => session.StoreEvent(e.ToSourcedEvent(), toSynchronise.StreamId));
                session.Commit(toSynchronise.CommitId);
            }
        }
    }
}