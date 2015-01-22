using System;
using SystemDot.Bootstrapping;
using SystemDot.Core.Collections;
using SystemDot.Domain;
using SystemDot.Domain.Bootstrapping;
using SystemDot.Domain.Commands;
using SystemDot.Domain.Queries;
using SystemDot.Environment;
using SystemDot.EventSourcing.Bootstrapping;
using SystemDot.EventSourcing.InMemory.Bootstrapping;
using SystemDot.EventSourcing.Sessions;
using SystemDot.EventSourcing.Synchronisation.Server.Bootstrapping;
using SystemDot.Ioc;
using BoDi;
using TechTalk.SpecFlow;

namespace SystemDot.EventSourcing.Synchronisation.Server.Specifications.Bootstrapping
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
            
            Bootstrap.Application()
                .ResolveReferencesWith(container)
                .UseEnvironment()
                .UseDomain().DispatchEventsOnUiThread().WithSimpleMessaging()
                .UseEventSourcing().WithSynchronisation().PersistToMemory()
                .Initialise();

            RegisterInSpecFlow<IEventSessionFactory>();
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