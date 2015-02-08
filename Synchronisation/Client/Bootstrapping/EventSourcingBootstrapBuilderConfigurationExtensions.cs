using SystemDot.Domain.Commands;
using SystemDot.EventSourcing.Bootstrapping;
using SystemDot.EventSourcing.Synchronisation.Client.Retrieval;

namespace SystemDot.EventSourcing.Synchronisation.Client.Bootstrapping
{
    using SystemDot.EventSourcing.Synchronisation.Client.Http;

    public static class EventSourcingBootstrapBuilderConfigurationExtensions
    {
        public static EventSourcingBootstrapBuilderConfiguration WithSynchronisation(this EventSourcingBootstrapBuilderConfiguration config)
        {
            config.GetBootstrapBuilderConfiguration()
                .RegisterBuildAction(c => c.RegisterCommandHandlersFromAssemblyOf<CommitRetrievalClient>())
                .RegisterBuildAction(c => c.RegisterInstance<ICommitRetrievalClient, CommitRetrievalClient>())
                .RegisterBuildAction(c => c.RegisterInstance<IHttpClientFactory, HttpClientFactory>());

            return config;
        }
    }
}