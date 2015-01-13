using SystemDot.Bootstrapping;
using SystemDot.Domain.Bootstrapping;
using SystemDot.Domain.Commands;
using SystemDot.Domain.Queries;
using SystemDot.EventSourcing.Bootstrapping;
using SystemDot.EventSourcing.Synchronisation.Client;
using SystemDot.EventSourcing.Synchronisation.Client.Http;
using SystemDot.EventSourcing.Synchronisation.Server;
using SystemDot.Messaging.Handling.Configuration;
using SystemDot.Messaging.Simple;

namespace SystemDot.EventSourcing.Synchronisation.Bootstrapping
{
    public static class EventSourcingBootstrapBuilderConfigurationExtensions
    {
        public static EventSourcingBootstrapBuilderConfiguration WithSynchronisation(this EventSourcingBootstrapBuilderConfiguration config)
        {
            config.GetBootstrapBuilderConfiguration()
                .RegisterBuildAction(c => c.RegisterInstance<IHttpClientFactory, HttpClientFactory>())
                .RegisterBuildAction(c => c.RegisterQueryHandlersFromAssemblyOf<CommitQuery>())
                .RegisterBuildAction(c => c.RegisterCommandHandlersFromAssemblyOf<CommitQuery>())
                .RegisterBuildAction(c => c.RegisterOpenTypeHandlersWithMessenger(typeof(IAsyncCommandHandler<>)), BuildOrder.Ultimate);

            return config;
        }
    }
}