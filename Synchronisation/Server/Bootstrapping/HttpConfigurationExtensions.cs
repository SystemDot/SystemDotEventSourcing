namespace SystemDot.EventSourcing.Synchronisation.Server.Bootstrapping
{
    using System.Web.Http;

    public static class HttpConfigurationExtensions
    {
        public static void MapSynchronisationRoutes(this HttpConfiguration configuration)
        {
            configuration.Routes.MapHttpRoute(
                "Synchronisation",
                "Synchronisation/{clientId}/{fromInTicks}",
                new { controller = "Synchronisation" },
                new { fromInTicks = @"\d+" });
        }
    }
}