using System.Threading.Tasks;
using SystemDot.Domain.Commands;
using SystemDot.Environment;
using SystemDot.EventSourcing.Synchronisation.Client.Messages;

namespace SystemDot.EventSourcing.Synchronisation.Client.Process
{
    public class InitialiseClientSynchronisationProcessHandler : IAsyncCommandHandler<InitialiseClientSynchronisationProcess>
    {
        readonly IDomainRepository domainRepository;
        readonly ILocalMachine localMachine;

        public InitialiseClientSynchronisationProcessHandler(IDomainRepository domainRepository, ILocalMachine localMachine)
        {
            this.domainRepository = domainRepository;
            this.localMachine = localMachine;
        }

        public Task Handle(InitialiseClientSynchronisationProcess message)
        {
            domainRepository.Save(ClientSynchronisationProcess.Initialise(localMachine, message.ClientId, message.ServerUri));
            return Task.FromResult(false);
        }
    }
}