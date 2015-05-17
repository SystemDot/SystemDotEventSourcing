namespace SystemDot.EventSourcing.Synchronisation
{
    public class SynchronisableCommitHeader
    {
        public string Key { get; set; }
        public byte[] Value { get; set; }
    }
}