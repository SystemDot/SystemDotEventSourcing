using System;
using SystemDot.Bootstrapping;
using SystemDot.Domain;
using SystemDot.Domain.Bootstrapping;
using SystemDot.Ioc;
using Machine.Specifications;

namespace SystemDot.EventSourcing.Specifications.configuring
{
    public class when_using_domain_with_simple_messaging
    {
        static Exception exception;

        Establish context = () => exception = Catch.Exception(() => 
            Bootstrap.Application()
                .ResolveReferencesWith(new IocContainer())
                .UseDomain().WithSimpleMessaging()
                .Initialise());

        It should_verify_that_all_simple_messaging_components_are_setup = () =>
            exception.ShouldBeNull();
    }
}