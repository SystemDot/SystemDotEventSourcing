using SystemDot.Bootstrapping;

namespace SystemDot.EventSourcing.Bootstrapping
{
    public static class BootstrapBuilderConfigurationExtensions
    {
        public static EventSourcingBootstrapBuilderConfiguration UseEventSourcing(this BootstrapBuilderConfiguration config)
        {
            config.GetIocContainer().RegisterEventSourcing();
            return new EventSourcingBootstrapBuilderConfiguration(config);
        }
    }
}