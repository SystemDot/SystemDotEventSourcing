using System;

namespace SystemDot.EventSourcing.Synchronisation.Messages
{
    public class ClientSynchronisationCompleted
    {
        public DateTime LastCommitDate { get; set; }
    }
}