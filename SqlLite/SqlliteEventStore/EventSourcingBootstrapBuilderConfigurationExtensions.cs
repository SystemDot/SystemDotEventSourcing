namespace SqlliteEventStore
{
    using SystemDot.Bootstrapping;
    using SystemDot.EventSourcing.Bootstrapping;

    public static class EventSourcingBootstrapBuilderConfigurationExtensions
    {
        public static BootstrapBuilderConfiguration PersistToSqlLite(this EventSourcingBootstrapBuilderConfiguration config)
        {
            return config.GetBootstrapBuilderConfiguration().RegisterBuildAction(c => c.RegisterSqlLiteEventSourcing());
        }
    }
}