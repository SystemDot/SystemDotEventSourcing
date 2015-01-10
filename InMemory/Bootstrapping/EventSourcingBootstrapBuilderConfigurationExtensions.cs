using SystemDot.Bootstrapping;
using SystemDot.EventSourcing.Bootstrapping;

namespace SystemDot.EventSourcing.InMemory.Bootstrapping
{
    public static class EventSourcingBootstrapBuilderConfigurationExtensions
    {
        public static BootstrapBuilderConfiguration PersistToMemory(this EventSourcingBootstrapBuilderConfiguration config)
        {
            config.GetBootstrapBuilderConfiguration().RegisterBuildAction(c => c.RegisterInMemoryEventSourcing());
            return config.GetBootstrapBuilderConfiguration();
        }
    }
}