using SystemDot.Domain.Queries;
using SystemDot.EventSourcing.Bootstrapping;

namespace SystemDot.EventSourcing.Synchronisation.Server.Bootstrapping
{
    public static class EventSourcingBootstrapBuilderConfigurationExtensions
    {
        public static EventSourcingBootstrapBuilderConfiguration WithSynchronisation(this EventSourcingBootstrapBuilderConfiguration config)
        {
            config.GetBootstrapBuilderConfiguration()
                .RegisterBuildAction(c => c.RegisterControllers())
                .RegisterBuildAction(c => c.RegisterQueryHandlersFromAssemblyOf<CommitQuery>());

            return config;
        }
    }
}