using SystemDot.Domain.Commands;
using SystemDot.Domain.Queries;
using SystemDot.EventSourcing.Bootstrapping;
using SystemDot.EventSourcing.Synchronisation.Client;
using SystemDot.EventSourcing.Synchronisation.Client.Http;
using SystemDot.EventSourcing.Synchronisation.Server;

namespace SystemDot.EventSourcing.Synchronisation.Bootstrapping
{
    public static class EventSourcingBootstrapBuilderConfigurationExtensions
    {
        public static EventSourcingBootstrapBuilderConfiguration WithSynchronisation(this EventSourcingBootstrapBuilderConfiguration config)
        {
            config.GetBootstrapBuilderConfiguration()
                .RegisterBuildAction(c => c.RegisterInstance<IHttpClientFactory, HttpClientFactory>())
                .RegisterBuildAction(c => c.RegisterQueryHandlersFromAssemblyOf<CommitQuery>())
                .RegisterBuildAction(c => c.RegisterCommandHandlersFromAssemblyOf<CommitQuery>());

            return config;
        }
    }
}