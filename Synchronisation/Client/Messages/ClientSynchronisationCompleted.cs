using System;

namespace SystemDot.EventSourcing.Synchronisation.Client.Messages
{
    public class ClientSynchronisationCompleted
    {
        public DateTime LastCommitDate { get; set; }
    }
}