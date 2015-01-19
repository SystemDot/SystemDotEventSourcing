namespace SystemDot.EventSourcing.Headers
{
    public class EventOriginHeader
    {
        public const string Key = "Origin";

        public string MachineName { get; set; }
    }
}