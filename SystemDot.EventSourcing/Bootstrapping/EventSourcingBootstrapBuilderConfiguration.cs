using SystemDot.Bootstrapping;

namespace SystemDot.EventSourcing.Bootstrapping
{
    public class EventSourcingBootstrapBuilderConfiguration
    {
        readonly BootstrapBuilderConfiguration config;

        public EventSourcingBootstrapBuilderConfiguration(BootstrapBuilderConfiguration config)
        {
            this.config = config;
            config.RegisterBuildAction(async c => await c.BuildReadModel(), BuildOrder.VeryLate);
        }

        public BootstrapBuilderConfiguration GetBootstrapBuilderConfiguration()
        {
            return config;
        }
    }
}