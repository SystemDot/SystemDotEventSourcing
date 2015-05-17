using System.Collections.Generic;
using System.Threading.Tasks;
using SystemDot.Domain.Queries;

namespace SystemDot.EventSourcing.Synchronisation.Server
{
    using System;
    using SystemDot.Core;
    using SystemDot.Environment;
    using SystemDot.EventSourcing.Commits;
    using SystemDot.EventSourcing.Headers;

    public class CommitQueryHandler : IAsyncQueryHandler<CommitQuery, IEnumerable<SynchronisableCommit>>
    {
        readonly SynchronisableCommitBuilder builder;
        readonly ILocalMachine localMachine;

        public CommitQueryHandler(SynchronisableCommitBuilder builder, ILocalMachine localMachine)
        {
            this.builder = builder;
            this.localMachine = localMachine;
        }

        public Task<IEnumerable<SynchronisableCommit>> Handle(CommitQuery message)
        {
            if (message.From == DateTime.MinValue)
            {
                return Task.FromResult(builder.Build(message.ClientId, message.From, CheckCommitOriginatesOnAnyMachine));
            }

            return Task.FromResult(builder.Build(message.ClientId, message.From, CheckCommitOriginatesOnLocalMachine));
        }

        bool CheckCommitOriginatesOnAnyMachine(Commit commit)
        {
            return true;
        }
        
        bool CheckCommitOriginatesOnLocalMachine(Commit commit)
        {
            return commit.OriginatesOnMachineNamed(localMachine.GetName());
        }
    }
}