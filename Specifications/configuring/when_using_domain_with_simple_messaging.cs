using System;
using SystemDot.Bootstrapping;
using SystemDot.Domain;
using SystemDot.Domain.Bootstrapping;
using SystemDot.Ioc;
using FluentAssertions;
using Machine.Specifications;

namespace SystemDot.EventSourcing.Specifications.configuring
{
    public class when_using_domain_with_simple_messaging
    {
        static Exception exception;
        static IocContainer toResolveWith;

        Establish context = () => toResolveWith = new IocContainer();

        Because of = () => exception = Catch.Exception(() =>
            Bootstrap.Application()
                .ResolveReferencesWith(toResolveWith)
                .UseDomain().WithSimpleMessaging()
                .Initialise());

        It should_verify_that_all_simple_messaging_components_are_setup = () =>
            exception.Should().BeNull();
    }
}