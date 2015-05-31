namespace SystemDot.EventSourcing.Commits
{
    public static class ObjectExtensions
    {
        public static string SerialiseToString(this object toSerialise)
        {
            return new JsonSerializer().SerialiseToString(toSerialise);
        }
    }
}