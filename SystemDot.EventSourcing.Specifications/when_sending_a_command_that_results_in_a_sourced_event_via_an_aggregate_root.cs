using System;
using System.Linq;
using SystemDot.Bootstrapping;
using SystemDot.Core;
using SystemDot.Domain;
using SystemDot.Domain.Bootstrapping;
using SystemDot.Domain.Commands;
using SystemDot.EventSourcing.Aggregation;
using SystemDot.EventSourcing.Bootstrapping;
using SystemDot.EventSourcing.InMemory.Bootstrapping;
using SystemDot.EventSourcing.Sessions;
using SystemDot.Ioc;
using SystemDot.Messaging.Simple;
using Machine.Specifications;

namespace SystemDot.EventSourcing.Specifications
{
    public class when_sending_a_command_that_results_in_a_sourced_event_via_an_aggregate_root
    {
        static TestAggregateRootCreatedEvent handledEvent; 
        static ICommandBus commandBus;
        const string Id = "Id";

        Establish context = () =>
        {
            IIocContainer container = new IocContainer();
            container.RegisterInstance<IAsyncCommandHandler<TestCommand>, TestCommandHandler>();
            container.RegisterDecorator<EventSessionAsyncCommandHandler<TestCommand>, IAsyncCommandHandler<TestCommand>>();

            Bootstrap.Application()
                .ResolveReferencesWith(container)
                .UseDomain().WithSimpleMessaging()
                .UseEventSourcing().PersistToMemory()
                .Initialise();

            commandBus = container.Resolve<ICommandBus>();

            Messenger.RegisterHandler<TestAggregateRootCreatedEvent>(e => handledEvent = e);
        };

        Because of = () => commandBus.SendCommandAsync<TestCommand>(c => c.Id = Id).Wait();


        It should_put_the_sourced_event_in_the_session_with_the_event_as_its_session = async () =>
             EventSessionProvider.Session.GetEventsAsync(Id).Result.Single()
                .Body.As<TestAggregateRootCreatedEvent>().Id.ShouldEqual(Id);

        It should_send_the_event = () => handledEvent.Id.ShouldEqual(Id);
        
        It should_put_the_sourced_event_in_the_session_with_the_aggregate_root_type_in_its_headers = async () =>
            EventSessionProvider.Session.GetEventsAsync(Id).Result.Single()
                .GetHeader<Type>(EventHeaderKeys.AggregateType)
                .ShouldEqual(typeof(TestAggregateRoot));
    }
}