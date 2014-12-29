using SystemDot.Bootstrapping;
using SystemDot.Domain.Events.Dispatching;
using SystemDot.EventSourcing.Bootstrapping;
using SystemDot.Ioc;

namespace SystemDot.EventSourcing.Sql.Windows.Bootstrapping
{
    using NEventStore;

    public static class EventSourcingBootstrapBuilderConfigurationExtensions
    {
        public static BootstrapBuilderConfiguration PersistToSql(this EventSourcingBootstrapBuilderConfiguration config, string connectionString)
        {
            config.GetBootstrapBuilderConfiguration().RegisterBuildAction(c => c.RegisterSqlWindowsEventSourcing());
            config.GetBootstrapBuilderConfiguration().RegisterBuildAction(c => Build(c, connectionString));
            return config.GetBootstrapBuilderConfiguration();
        }

        static void Build(IIocContainer container, string connectionString)
        {
            container.RegisterInstance<IStoreEvents>(() =>
                Wireup.Init()
                    .UsingSqlPersistence(connectionString)
                    .PageEvery(int.MaxValue)
                    .InitializeStorageEngine()
                    .UsingJsonSerialization()
                    .UsingSynchronousDispatchScheduler()
                    .DispatchTo(new MessageBusCommitDispatcher(container.Resolve<IEventDispatcher>()))
                    .Build());
        }
    }
}