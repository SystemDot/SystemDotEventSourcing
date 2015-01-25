using System;

namespace SystemDot.EventSourcing.Synchronisation.Client.Messages
{
    public class ClientSynchronisationSuccessfullyCompleted
    {
        public DateTime LastCommitDate { get; set; }
    }
}