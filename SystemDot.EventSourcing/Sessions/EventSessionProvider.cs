using System.Threading;

namespace SystemDot.EventSourcing.Sessions
{
    public class EventSessionProvider
    {
        static readonly ThreadLocal<IEventSession> SessionStorage = new ThreadLocal<IEventSession>();

        public static IEventSession Session
        {
            get
            {
                return SessionStorage.Value;
            }
            set
            {
                SessionStorage.Value = value;
            }
        }
    }
}