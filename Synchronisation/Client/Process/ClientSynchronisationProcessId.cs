using SystemDot.Environment;

namespace SystemDot.EventSourcing.Synchronisation.Client.Process
{
    using SystemDot.Domain;

    public class ClientSynchronisationProcessId : MultiSiteId
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