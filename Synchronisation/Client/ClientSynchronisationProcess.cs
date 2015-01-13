using System;
using System.Threading.Tasks;
using SystemDot.EventSourcing.Aggregation;
using SystemDot.EventSourcing.Synchronisation.Messages;

namespace SystemDot.EventSourcing.Synchronisation.Client
{
    public class ClientSynchronisationProcess : AggregateRoot
    {
        Uri serverUri;

        public static ClientSynchronisationProcess Initialise(string clientId, string serverUri)
        {
            return new ClientSynchronisationProcess(clientId, serverUri);
        }

        ClientSynchronisationProcess(string clientId, string serverUri) : base(clientId)
        {
            AddEvent<ClientSynchronisationProcessInitialised>(e => e.ServerUri = serverUri);
        }

        public ClientSynchronisationProcess()
        {
        }

        void ApplyEvent(ClientSynchronisationProcessInitialised @event)
        {
            serverUri = new Uri(@event.ServerUri);
        }

        public async Task Synchronise(CommitSynchroniser commitSynchroniser)
        {
            await commitSynchroniser.Synchronise(serverUri);
            Complete();
        }

        void Complete()
        {
            AddEvent(new ClientSynchronisationCompleted());
        }
    }
}