using SystemDot.Domain.Queries;
using SystemDot.EventSourcing.Bootstrapping;

namespace SystemDot.EventSourcing.Synchronisation.Server.Bootstrapping
{
    using SystemDot.Domain.Commands;
    using SystemDot.Domain.Events.Dispatching;

    public static class EventSourcingBootstrapBuilderConfigurationExtensions
    {
        public static EventSourcingBootstrapBuilderConfiguration WithSynchronisation(this EventSourcingBootstrapBuilderConfiguration config)
        {
            config.GetBootstrapBuilderConfiguration()
                .RegisterBuildAction(c => c.RegisterControllers())
                .RegisterBuildAction(c => c.RegisterQueryHandlersFromAssemblyOf<CommitQuery>())
                .RegisterBuildAction(c => c.RegisterCommandHandlersFromAssemblyOf<CommitQuery>());

            return config;
        }
    }
}