using System.Collections.Generic;
using System.Threading.Tasks;
using SystemDot.Domain.Queries;
using SystemDot.EventSourcing.Sessions;

namespace SystemDot.EventSourcing.Synchronisation.Server
{
    public class CommitQueryHandler : IAsyncQueryHandler<CommitQuery, IEnumerable<SynchronisableCommit>>
    {
        readonly IEventSessionFactory factory;

        public CommitQueryHandler(IEventSessionFactory factory)
        {
            this.factory = factory;
        }

        public Task<IEnumerable<SynchronisableCommit>> Handle(CommitQuery message)
        {
            return Task.FromResult(factory.Create().GetSynchronisableCommits());
        }       
    }
}