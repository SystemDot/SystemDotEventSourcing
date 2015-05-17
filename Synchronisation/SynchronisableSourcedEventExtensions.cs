namespace SystemDot.EventSourcing.Synchronisation
{
    using SystemDot.EventSourcing.Streams;
    using SystemDot.Serialisation;

    public static class SynchronisableSourcedEventExtensions
    {
        public static SourcedEvent ToSourcedEvent(this SynchronisableSourcedEvent e)
        {
            return new SourcedEvent { Body = e.Body.Deserialise() };
        }
    }
}