using System;
using SystemDot.Bootstrapping;
using SystemDot.Core.Collections;
using SystemDot.Domain.Bootstrapping;
using SystemDot.Domain.Commands;
using SystemDot.Domain.Queries;
using SystemDot.Environment;
using SystemDot.EventSourcing.Bootstrapping;
using SystemDot.EventSourcing.InMemory.Bootstrapping;
using SystemDot.EventSourcing.Sessions;
using SystemDot.EventSourcing.Synchronisation.Bootstrapping;
using SystemDot.EventSourcing.Synchronisation.Client.Http;
using SystemDot.EventSourcing.Synchronisation.Testing;
using SystemDot.Ioc;
using SystemDot.Messaging.Simple;
using BoDi;
using TechTalk.SpecFlow;

namespace SystemDot.Domain.Synchronisation.Specifications.Bootstrapping
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
            
            container.RegisterInstance<IHttpClientFactory, TestHttpClientFactory>();
            RegisterInSpecFlow<IHttpClientFactory>();
            
            Bootstrap.Application()
                .ResolveReferencesWith(container)
                .UseEnvironment()
                .UseDomain().DispatchEventsOnUiThread().WithSimpleMessaging()
                .UseEventSourcing().WithSynchronisation().PersistToMemory()
                .Initialise();

            RegisterInSpecFlow<IEventSessionFactory>();
            RegisterInSpecFlow<Dispatcher>();
            RegisterOpenTypeInSpecFlow(typeof(IAsyncQueryHandler<,>));
            RegisterOpenTypeInSpecFlow(typeof(IAsyncCommandHandler<>));
        }

        void RegisterInSpecFlow<T>() where T : class
        {
            specFlowContainer.RegisterInstanceAs<T>(container.Resolve<T>());
        }

        void RegisterOpenTypeInSpecFlow(Type openType)
        {
            container.ResolveMutipleTypes()
                .ThatImplementOpenType(openType)
                .ForEach(h => specFlowContainer.RegisterInstanceAs(h));
        }

        static void Reset()
        {
            container = new IocContainer();
        }
    }
}