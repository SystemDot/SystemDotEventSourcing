namespace SystemDot.EventSourcing.Sqlite.Android
{
    using System.IO;

    public static class ObjectExtensions
    {
        public static byte[] SerialiseToBytes(this object toSerialise, JsonSerializer serializer)
        {
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, toSerialise);
                return stream.ToArray();
            }
        }
    }
}