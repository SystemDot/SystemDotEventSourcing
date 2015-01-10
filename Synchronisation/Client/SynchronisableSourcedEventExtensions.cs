using SystemDot.EventSourcing.Streams;
using SystemDot.Serialisation;

namespace SystemDot.EventSourcing.Synchronisation.Client
{
    public static class SynchronisableSourcedEventExtensions
    {
        public static SourcedEvent ToSourcedEvent(this SynchronisableSourcedEvent e)
        {
            return new SourcedEvent { Body = new JsonSerialiser().Deserialise(e.Body) };
        }
    }
}