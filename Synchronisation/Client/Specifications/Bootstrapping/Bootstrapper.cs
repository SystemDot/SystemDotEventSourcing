using SystemDot.Bootstrapping;
using SystemDot.Domain.Bootstrapping;
using SystemDot.Environment;
using SystemDot.EventSourcing.Bootstrapping;
using SystemDot.EventSourcing.InMemory.Bootstrapping;
using SystemDot.EventSourcing.Sessions;
using SystemDot.EventSourcing.Synchronisation.Client.Bootstrapping;
using SystemDot.EventSourcing.Synchronisation.Client.Retrieval;
using SystemDot.Ioc;
using TechTalk.SpecFlow;
using SystemDot.Domain.Synchronisation.Client.Specifications.Steps;

namespace SystemDot.Domain.Synchronisation.Client.Specifications.Bootstrapping
{
    using System;
    using SystemDot.Domain.Synchronisation.Client.Specifications.Steps.Client;
    using SystemDot.Domain.Synchronisation.Client.Specifications.Steps.Commits;
    using SystemDot.Domain.Synchronisation.Client.Specifications.Steps.Server;
    using SystemDot.EventSourcing.Synchronisation.Client;
    using SystemDot.EventSourcing.Synchronisation.Client.Http;

    [Binding]
    public class Bootstrapper
    {
        readonly CommitContext commitContext;
        readonly CommitSynchroniserContext commitSynchroniserContext;
        readonly ServerContext serverContext;

        public Bootstrapper(CommitContext commitContext, CommitSynchroniserContext commitSynchroniserContext, ServerContext serverContext)
        {
            this.commitContext = commitContext;
            this.commitSynchroniserContext = commitSynchroniserContext;
            this.serverContext = serverContext;
        }

        [BeforeScenario]
        public void OnBeforeScenario()
        {
            var container = new IocContainer();

            serverContext.ServerUri = new Uri("http://TestServer");
            serverContext.SynchronisationHttpClient = new TestSynchronisationHttpClient();
            container.RegisterInstance<ISynchronisationHttpClient>(() => serverContext.SynchronisationHttpClient);
            
            Bootstrap.Application()
                .ResolveReferencesWith(container)
                .UseEnvironment()
                .UseDomain().DispatchEventsOnUiThread().WithSimpleMessaging()
                .UseEventSourcing().WithSynchronisation(serverContext.ServerUri).PersistToMemory()
                .Initialise();

            commitContext.EventSessionFactory = container.Resolve<IEventSessionFactory>();
            commitSynchroniserContext.CommitSynchroniser = container.Resolve<CommitSynchroniser>();
        }
    }
}