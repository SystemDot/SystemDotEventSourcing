namespace SystemDot.EventSourcing.Synchronisation.Server
{
    using System;

    public class CommitQuery
    {
        public DateTime From { get; set; }
        public string ClientId { get; set; }
    }
}