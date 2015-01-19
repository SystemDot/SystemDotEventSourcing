namespace SystemDot.EventSourcing.Headers
{
    using SystemDot.Environment;

    public class EventOriginHeader
    {
        public static EventOriginHeader ForMachine(ILocalMachine localMachine)
        {
            return new EventOriginHeader { MachineName = localMachine.GetName() };
        }

        public const string Key = "Origin";
        public string MachineName { get; set; }
    }
}