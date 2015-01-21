using SystemDot.EventSourcing.Bootstrapping;
using SystemDot.EventSourcing.Synchronisation.Client.Http;

namespace SystemDot.EventSourcing.Synchronisation.Client.Bootstrapping
{
    public static class EventSourcingBootstrapBuilderConfigurationExtensions
    {
        public static EventSourcingBootstrapBuilderConfiguration WithSynchronisation(this EventSourcingBootstrapBuilderConfiguration config)
        {
            config.GetBootstrapBuilderConfiguration()
                .RegisterBuildAction(c => c.RegisterInstance<IHttpClientFactory, HttpClientFactory>());

            return config;
        }
    }
}