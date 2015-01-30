using System.Threading.Tasks;
using SystemDot.Domain.Commands;
using SystemDot.Environment;
using SystemDot.EventSourcing.Synchronisation.Client.Messages;

namespace SystemDot.EventSourcing.Synchronisation.Client.Process
{
    public class SynchroniseCommitsHandler : IAsyncCommandHandler<SynchroniseCommits>
    {
        readonly CommitSynchroniser commitSynchroniser;
        readonly IDomainRepository domainRepository;
        readonly ILocalMachine localMachine;

        public SynchroniseCommitsHandler(
            CommitSynchroniser commitSynchroniser, 
            IDomainRepository domainRepository, 
            ILocalMachine localMachine)
        {
            this.commitSynchroniser = commitSynchroniser;
            this.domainRepository = domainRepository;
            this.localMachine = localMachine;
        }

        public async Task Handle(SynchroniseCommits message)
        {
            var clientSynchronisationProcess = domainRepository.Get<ClientSynchronisationProcess>(ClientSynchronisationProcessId.Parse(localMachine));
            await clientSynchronisationProcess.Synchronise(commitSynchroniser);
            domainRepository.Save(clientSynchronisationProcess);
        }
    }
}