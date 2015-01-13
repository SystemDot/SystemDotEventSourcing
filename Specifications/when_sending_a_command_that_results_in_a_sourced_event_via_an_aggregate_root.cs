using System;
using System.Linq;
using SystemDot.Bootstrapping;
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
using FluentAssertions;

namespace SystemDot.EventSourcing.Specifications
{
    public class when_sending_a_command_that_results_in_a_sourced_event_via_an_aggregate_root
    {
        static TestAggregateRootCreatedEvent handledEvent;
        static IIocContainer container;
        const string Id = "Id";

        Establish context = () =>
        {
            container = new IocContainer();
            container.RegisterInstance<IAsyncCommandHandler<TestCommand>, TestCommandHandler>();
            
            Bootstrap.Application()
                .ResolveReferencesWith(container)
                .UseDomain().WithSimpleMessaging()
                .UseEventSourcing().PersistToMemory()
                .Initialise();
            
            container.Resolve<Dispatcher>().RegisterHandler<TestAggregateRootCreatedEvent>(e => handledEvent = e);
        };

        Because of = () => container.Resolve<IAsyncCommandHandler<TestCommand>>().Handle(new TestCommand { Id = Id }).Wait();
        
        It should_put_the_sourced_event_in_the_session_with_the_event_as_its_session = () =>
             container.Resolve<IEventSessionFactory>().Create().GetEvents(Id).Single()
                .Body.As<TestAggregateRootCreatedEvent>().Id.Should().Be(Id);

        It should_send_the_event = () => handledEvent.Id.Should().Be(Id);
        
        It should_put_the_sourced_event_in_the_session_with_the_aggregate_root_type_in_its_headers = () =>
            container.Resolve<IEventSessionFactory>().Create().GetEvents(Id).Single()
                .GetHeader<Type>(EventHeaderKeys.AggregateType)
                .Should().Be(typeof(TestAggregateRoot));
    }
}