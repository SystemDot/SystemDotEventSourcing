namespace SystemDot.EventSourcing.Synchronisation
{
    using System;
    using SystemDot.Serialisation;

    public static class ByteExtensions
    {
        public static object Deserialise(this Byte[] serialised)
        {
            return new JsonSerialiser().Deserialise(serialised);
        }
    }
}