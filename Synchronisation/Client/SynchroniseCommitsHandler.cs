using System.Threading.Tasks;
using SystemDot.Domain.Commands;
using SystemDot.EventSourcing.Synchronisation.Client.Messages;

namespace SystemDot.EventSourcing.Synchronisation.Client
{
    public class SynchroniseCommitsHandler : IAsyncCommandHandler<SynchroniseCommits>
    {
        readonly CommitSynchroniser commitSynchroniser;
        readonly IDomainRepository domainRepository;

        public SynchroniseCommitsHandler(
            CommitSynchroniser commitSynchroniser, 
            IDomainRepository domainRepository)
        {
            this.commitSynchroniser = commitSynchroniser;
            this.domainRepository = domainRepository;
        }

        public async Task Handle(SynchroniseCommits message)
        {
            var clientSynchronisationProcess = domainRepository.Get<ClientSynchronisationProcess>(message.ClientId);
            await clientSynchronisationProcess.Synchronise(commitSynchroniser);
            domainRepository.Save(clientSynchronisationProcess);
        }
    }
}