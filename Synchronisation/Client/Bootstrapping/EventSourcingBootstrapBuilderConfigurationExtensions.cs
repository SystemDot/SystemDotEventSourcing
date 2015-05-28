using SystemDot.EventSourcing.Bootstrapping;
using SystemDot.EventSourcing.Synchronisation.Client.Retrieval;

namespace SystemDot.EventSourcing.Synchronisation.Client.Bootstrapping
{
    using System;
    using SystemDot.Domain.Events.Dispatching;
    using SystemDot.EventSourcing.Synchronisation.Client.Http;

    public static class EventSourcingBootstrapBuilderConfigurationExtensions
    {
        public static EventSourcingBootstrapBuilderConfiguration WithSynchronisation(this EventSourcingBootstrapBuilderConfiguration config, Uri synchronisationServerUri)
        {
            config.GetBootstrapBuilderConfiguration()
                .RegisterBuildAction(
                    c => c.RegisterInstance(() => new SynchronisationServerUriProvider(synchronisationServerUri)))
                .RegisterBuildAction(c => c.RegisterInstance<ISynchronisationHttpClient, SynchronisationHttpClient>())
                .RegisterBuildAction(c => c.RegisterInstance<IHttpClientFactory, HttpClientFactory>());

            return config;
        }
    }
}