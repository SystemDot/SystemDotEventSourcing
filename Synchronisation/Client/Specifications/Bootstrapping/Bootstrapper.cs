using SystemDot.Bootstrapping;
using SystemDot.Domain.Bootstrapping;
using SystemDot.Environment;
using SystemDot.EventSourcing.Bootstrapping;
using SystemDot.EventSourcing.InMemory.Bootstrapping;
using SystemDot.EventSourcing.Sessions;
using SystemDot.EventSourcing.Synchronisation.Client;
using SystemDot.EventSourcing.Synchronisation.Client.Bootstrapping;
using SystemDot.EventSourcing.Synchronisation.Testing;
using SystemDot.Ioc;
using SystemDot.Messaging.Simple;
using BoDi;
using TechTalk.SpecFlow;

namespace SystemDot.Domain.Synchronisation.Client.Specifications.Bootstrapping
{
    [Binding]
    public class Bootstrapper
    {
        readonly IObjectContainer specFlowContainer;
        static IIocContainer container;

        Bootstrapper(IObjectContainer specFlowContainer)
        {
            this.specFlowContainer = specFlowContainer;
        }

        [BeforeScenario]
        public void OnBeforeScenario()
        {
            Reset();

            container.RegisterInstance<ICommitRetrievalClient, TestCommitRetrievalClient>();
            RegisterInSpecFlow<ICommitRetrievalClient>();
            
            Bootstrap.Application()
                .ResolveReferencesWith(container)
                .UseEnvironment()
                .UseDomain().DispatchEventsOnUiThread().WithSimpleMessaging()
                .UseEventSourcing().WithSynchronisation().PersistToMemory()
                .Initialise();

            RegisterInSpecFlow<IEventSessionFactory>();
            RegisterInSpecFlow<Dispatcher>();
        }

        void RegisterInSpecFlow<T>() where T : class
        {
            specFlowContainer.RegisterInstanceAs<T>(container.Resolve<T>());
        }

        static void Reset()
        {
            container = new IocContainer();
        }
    }
}