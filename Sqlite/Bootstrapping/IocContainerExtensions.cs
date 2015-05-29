namespace SystemDot.EventSourcing.Sqlite.Android.Bootstrapping
{
    using SystemDot.EventSourcing.Sessions;
    using SystemDot.EventSourcing.Streams;
    using SystemDot.Ioc;

    internal static class IocContainerExtensions
    {
        internal static void RegisterSqliteEventSourcing(this IIocContainer container)
        {
            container.RegisterInstance<IEventSessionFactory, EventSessionFactory>();
            container.RegisterInstance<IEventStore, SqlLiteEventStore>();
        }
    }
}