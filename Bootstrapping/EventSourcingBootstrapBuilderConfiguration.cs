using SystemDot.Bootstrapping;

namespace SystemDot.EventSourcing.Bootstrapping
{
    public class EventSourcingBootstrapBuilderConfiguration
    {
        readonly BootstrapBuilderConfiguration config;

        public EventSourcingBootstrapBuilderConfiguration(BootstrapBuilderConfiguration config)
        {
            this.config = config;

            config
                .RegisterBuildAction(c => c.RegisterProjectionsWithMessenger())
                .RegisterBuildAction(async c => await c.HydrateInMemoryProjections(), BuildOrder.VeryLate);
        }

        public BootstrapBuilderConfiguration GetBootstrapBuilderConfiguration()
        {
            return config;
        }
    }
}