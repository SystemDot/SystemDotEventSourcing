using System;
using System.Threading.Tasks;
using SystemDot.Environment;
using SystemDot.EventSourcing.Aggregation;
using SystemDot.EventSourcing.Synchronisation.Client.Messages;
using SystemDot.EventSourcing.Synchronisation.Client.Retrieval;

namespace SystemDot.EventSourcing.Synchronisation.Client.Process
{
    public class ClientSynchronisationProcess : AggregateRoot
    {
        Uri serverUri;
        DateTime previousCommitDate;
        string clientId;

        public static ClientSynchronisationProcess Initialise(ILocalMachine localMachine, string clientId, string serverUri)
        {
            return new ClientSynchronisationProcess(ClientSynchronisationProcessId.Parse(localMachine), clientId, serverUri);
        }

        ClientSynchronisationProcess(ClientSynchronisationProcessId id, string clientId, string serverUri)
            : base(id)
        {
            AddEvent<ClientSynchronisationProcessInitialised>(e =>
            {
                e.ServerUri = serverUri;
                e.ClientId = clientId;
            });
        }


        public ClientSynchronisationProcess()
        {
        }

        void ApplyEvent(ClientSynchronisationProcessInitialised @event)
        {
            serverUri = new Uri(@event.ServerUri);
            clientId = @event.ClientId;
        }

        public async Task Synchronise(CommitSynchroniser commitSynchroniser)
        {
            await commitSynchroniser.Synchronise(
                new CommitRetrievalCriteria(serverUri, previousCommitDate, clientId), 
                CompleteSuccsessfully,
                CompleteUnsuccsessfully);
        }

        void CompleteSuccsessfully(DateTime lastCommitDate)
        {
            AddEvent(new ClientSynchronisationSuccessfullyCompleted
            {
                LastCommitDate = lastCommitDate
            });
        }

        void CompleteUnsuccsessfully()
        {
            AddEvent(new ClientSynchronisationUnsuccessful());
        }

        void ApplyEvent(ClientSynchronisationSuccessfullyCompleted @event)
        {
            previousCommitDate = @event.LastCommitDate;
        }
    }
}