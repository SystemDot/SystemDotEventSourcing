using SystemDot.Environment;
using SystemDot.EventSourcing.Aggregation;

namespace SystemDot.EventSourcing.Synchronisation.Client.Process
{
    public class ClientSynchronisationProcessId : AggregateRootId
    {
        public static ClientSynchronisationProcessId Parse(ILocalMachine localMachine)
        {
            return new ClientSynchronisationProcessId(localMachine.GetName());
        }

        private ClientSynchronisationProcessId(string localMachineName)
            : base(localMachineName, "ClientSynchronisationProcess")
        {
        }
    }
}