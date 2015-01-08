using System.Web.Http;
using Owin;

namespace SystemDot.EventSourcing.Synchronisation.Testing
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute("DefaultTestServerApi", "{controller}/{id}", new { id = RouteParameter.Optional });
            appBuilder.UseWebApi(config);
        }
    }
}