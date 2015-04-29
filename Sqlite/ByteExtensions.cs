namespace SystemDot.EventSourcing.Sqlite.Android
{
    using System.IO;

    public static class ByteExtensions
    {
        public static T DeserialiseTo<T>(this byte[] toDeserialise, ISerialize serializer)
        {
            toDeserialise = toDeserialise ?? new byte[] { };
            if (toDeserialise.Length == 0)
            {
                return default(T);
            }

            using (var stream = new MemoryStream(toDeserialise))
                return serializer.Deserialize<T>(stream);
        }
    }
}