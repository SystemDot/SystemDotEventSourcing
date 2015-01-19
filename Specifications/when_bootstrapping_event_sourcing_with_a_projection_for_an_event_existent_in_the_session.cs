using System;
using SystemDot.Domain.Events;
using SystemDot.Domain.Events.Dispatching;
using SystemDot.Environment;
using SystemDot.EventSourcing.Streams;
using SystemDot.Bootstrapping;
using SystemDot.Domain;
using SystemDot.Domain.Bootstrapping;
using SystemDot.EventSourcing.Bootstrapping;
using SystemDot.EventSourcing.InMemory;
using SystemDot.EventSourcing.InMemory.Bootstrapping;
using SystemDot.EventSourcing.Sessions;
using SystemDot.Ioc;
using FluentAssertions;
using Machine.Specifications;

namespace SystemDot.EventSourcing.Specifications
{
    public class when_bootstrapping_event_sourcing_with_a_projection_for_an_event_existent_in_the_session
    {
        private const string Id = "Id";
        private static IIocContainer container;
        private static TestHydrateAtStartupProjection projection;

        Establish context = () =>
        {
            container = new IocContainer();
            container.RegisterInstance<IEventSessionFactory>(() => new EventSessionFactory(new InMemoryEventStore(new NullEventDispatcher())));
           
            projection = new TestHydrateAtStartupProjection();
            container.RegisterInstance<TestHydrateAtStartupProjection>(() => projection);
            
            IEventSession session = container.Resolve<IEventSessionFactory>().Create();
            session.StoreEvent(new SourcedEvent { Body = new TestAggregateRootCreatedEvent { Id = Id }, }, Id);
            session.Commit(Guid.NewGuid());
        };

        Because of = async () =>
        {
            await Bootstrap.Application()
                .ResolveReferencesWith(container)
                .UseEnvironment()
                .UseDomain().WithSimpleMessaging()
                .UseEventSourcing().PersistToMemory()
                .InitialiseAsync();
        };

        It should_have_hydrated_the_projection = () => projection.IdFromEvent.Should().Be(Id);
    }
}