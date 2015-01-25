using System;
using System.Threading.Tasks;
using SystemDot.EventSourcing.Aggregation;
using SystemDot.EventSourcing.Synchronisation.Client.Messages;

namespace SystemDot.EventSourcing.Synchronisation.Client
{
    public class ClientSynchronisationProcess : AggregateRoot
    {
        Uri serverUri;
        DateTime previousCommitDate;

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
            DateTime commitDate = DateTime.MinValue;

            await commitSynchroniser.Synchronise(
                serverUri, 
                previousCommitDate,
                commit => commitDate = commit.CreatedOn,
                CompleteUnsuccsessfully);

            CompleteSuccsessfully(commitDate);
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