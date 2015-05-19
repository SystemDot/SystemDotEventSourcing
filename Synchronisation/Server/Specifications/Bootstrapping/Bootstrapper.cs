
namespace SystemDot.EventSourcing.Synchronisation.Server.Specifications.Bootstrapping
{
    using System.Web.Http;
    using SystemDot.Bootstrapping;
    using SystemDot.Domain;
    using SystemDot.Domain.Bootstrapping;
    using SystemDot.Environment;
    using SystemDot.EventSourcing.Bootstrapping;
    using SystemDot.EventSourcing.InMemory.Bootstrapping;
    using SystemDot.EventSourcing.Sessions;
    using SystemDot.EventSourcing.Synchronisation.Server.Bootstrapping;
    using SystemDot.EventSourcing.Synchronisation.Server.Specifications.Steps;
    using SystemDot.Ioc;
    using SystemDot.Web.WebApi.Caching;
    using SystemDot.Web.WebApi.Ioc;
    using SystemDot.Web.WebApi.ModelState;
    using Microsoft.Owin.Testing;
    using Owin;
    using TechTalk.SpecFlow;

    [Binding]
    public class Bootstrapper
    {
        readonly EventSessionContext eventSessionContext;
        readonly SynchronisableCommitContext synchronisableCommitContext;

        Bootstrapper(EventSessionContext eventSessionContext, SynchronisableCommitContext synchronisableCommitContext)
        {
            this.eventSessionContext = eventSessionContext;
            this.synchronisableCommitContext = synchronisableCommitContext;
        }

        [BeforeScenario]
        public void OnBeforeScenario()
        {
            IIocContainer container = new IocContainer();

             Bootstrap.Application()
                .ResolveReferencesWith(container)
                .UseEnvironment()
                .UseDomain().DispatchEventsOnUiThread().WithSimpleMessaging()
                .UseEventSourcing().WithSynchronisation().PersistToMemory()
                .Initialise();

            eventSessionContext.SessionFactory = container.Resolve<IEventSessionFactory>();

            synchronisableCommitContext.TestServer = TestServer.Create(builder =>
            {
                var configuration = new HttpConfiguration
                {
                    DependencyResolver = new SystemDotDependencyResolver(container)
                };

                configuration.MapHttpAttributeRoutes();
                configuration.Filters.Add(new ModelStateContextFilterAttribute());
                configuration.Filters.Add(new NoCacheFilterAttribute());

                configuration.MapSynchronisationRoutes();

                configuration.Routes.MapHttpRoute(
                    "Default",
                    "{controller}/{id}",
                    new {id = RouteParameter.Optional});

                builder.UseWebApi(configuration);
            });
        }
    }
}