using SystemDot.Bootstrapping;
using SystemDot.Core.Collections;
using SystemDot.Domain.Bootstrapping;
using SystemDot.Domain.Commands;
using SystemDot.Domain.Queries;
using SystemDot.EventSourcing.Bootstrapping;
using SystemDot.EventSourcing.InMemory.Bootstrapping;
using SystemDot.EventSourcing.Sessions;
using SystemDot.EventSourcing.Synchronisation;
using SystemDot.Ioc;
using SystemDot.Messaging.Simple;
using BoDi;
using TechTalk.SpecFlow;

namespace SystemDot.Domain.Synchronisation.Specifications
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
                .UseDomain().DispatchEventsOnUiThread().WithSimpleMessaging()
                .UseEventSourcing().WithSynchronisation().PersistToMemory()
                .Initialise();

            specFlowContainer.RegisterInstanceAs<IEventSessionFactory>(container.Resolve<IEventSessionFactory>());
            
            container.ResolveMutipleTypes()
                .ThatImplementOpenType(typeof(IAsyncQueryHandler<,>))
                .ForEach(h => specFlowContainer.RegisterInstanceAs(h));
        }

        static void Reset()
        {
            Messenger.Reset();
            container = new IocContainer();
        }
    }
}