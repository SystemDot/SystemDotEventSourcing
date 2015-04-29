namespace SystemDot.EventSourcing.Sqlite.Android.Bootstrapping
{
    using SystemDot.Bootstrapping;
    using SystemDot.EventSourcing.Bootstrapping;

    public static class EventSourcingBootstrapBuilderConfigurationExtensions
    {
        public static BootstrapBuilderConfiguration PersistToSqlite(this EventSourcingBootstrapBuilderConfiguration config)
        {
            return config.GetBootstrapBuilderConfiguration().RegisterBuildAction(c => c.RegisterSqliteEventSourcing());
        }
    }
}