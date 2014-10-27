using SystemDot.Configuration;
using SystemDot.EventSourcing.Configuration;

namespace SystemDot.EventSourcing.Esent.Windows.Configuration
{
    public static class EventSourcingBuilderConfigurationExtensions
    {
        public static IBuilderConfiguration PersistToMemory(this EventSourcingBuilderConfiguration config)
        {
            config.GetBuilderConfiguration()
                .RegisterBuildAction(c => c.RegisterInMemoryEventSourcing());

            return config.GetBuilderConfiguration();
        }
    }
}