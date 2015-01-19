using System;
using SystemDot.Bootstrapping;
using SystemDot.Domain;
using SystemDot.Domain.Bootstrapping;
using SystemDot.EventSourcing.Bootstrapping;
using SystemDot.EventSourcing.InMemory.Bootstrapping;
using SystemDot.Ioc;
using FluentAssertions;
using Machine.Specifications;

namespace SystemDot.EventSourcing.Specifications.configuring
{
    using SystemDot.Environment;

    public class when_using_in_memory_event_sourcing
    {
        static Exception exception;

        Establish context = () =>
        {
            var container = new IocContainer();
            container.RegisterInstance<ILocalMachine, LocalMachine>();

            exception = Catch.Exception(() => 
                Bootstrap.Application()
                    .ResolveReferencesWith(container)
                    .UseDomain()
                    .WithSimpleMessaging()
                    .UseEventSourcing().PersistToMemory()
                    .Initialise());
        };

        It should_verify_that_all_in_memory_event_sourcing_are_setup = () =>
            exception.Should().BeNull();
    }
}