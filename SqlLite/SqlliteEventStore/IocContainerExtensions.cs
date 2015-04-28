namespace SqlliteEventStore
{
    using SystemDot.EventSourcing.Sessions;
    using SystemDot.EventSourcing.Streams;
    using SystemDot.Ioc;

    internal static class IocContainerExtensions
    {
        internal static void RegisterSqlLiteEventSourcing(this IIocContainer container, string connectionString)
        {
            container.RegisterInstance<IEventSessionFactory, EventSessionFactory>();
            container.RegisterInstance<IEventStore, SqlLiteEventStore>();
            container.RegisterInstance(() => new SqlLitePersistenceEngine(connectionString, new JsonSerializer()));
        }
    }
}