using System.Threading.Tasks;
using SystemDot.Domain.Commands;
using SystemDot.EventSourcing.Synchronisation.Client.Messages;

namespace SystemDot.EventSourcing.Synchronisation.Client
{
    public class InitialiseClientSynchronisationProcessHandler : IAsyncCommandHandler<InitialiseClientSynchronisationProcess>
    {
        readonly IDomainRepository domainRepository;

        public InitialiseClientSynchronisationProcessHandler(IDomainRepository domainRepository)
        {
            this.domainRepository = domainRepository;
        }

        public Task Handle(InitialiseClientSynchronisationProcess message)
        {
            domainRepository.Save(ClientSynchronisationProcess.Initialise(message.ClientId, message.ServerUri));
            return Task.FromResult(false);
        }
    }
}