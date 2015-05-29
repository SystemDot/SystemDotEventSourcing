namespace SystemDot.EventSourcing.Commits
{
    using System.IO;
    using System.Text;

    public static class ObjectExtensions
    {
        public static string SerialiseToString(this object toSerialise)
        {
            using (var stream = new MemoryStream())
            {
                new JsonSerializer().Serialize(stream, toSerialise);

                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}