using SystemDot.EventSourcing.Sessions;

namespace SystemDot.EventSourcing.Esent.Windows
{
    public class EsentEventSessionFactory : IEventSessionFactory
    {
        readonly EsentEventSession eventSession;

        public EsentEventSessionFactory(EsentEventSession eventSession)
        {
            this.eventSession = eventSession;
        }

        public IEventSession Create()
        {
            EventSessionProvider.Session = eventSession;

            return eventSession;
        }
    }
}