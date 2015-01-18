using System;
using SystemDot.Bootstrapping;
using SystemDot.Domain.Bootstrapping;
using SystemDot.EventSourcing.Bootstrapping;
using SystemDot.EventSourcing.Sql.Windows.Bootstrapping;
using SystemDot.Ioc;
using Machine.Specifications;

namespace SystemDot.Domain.Specifications.configuring
{
    public class when_using_sql_event_sourcing
    {
        static Exception exception;

        Establish context = () => exception = Catch.Exception(() =>
            Bootstrap.Application()
                .ResolveReferencesWith(new IocContainer())
                .UseDomain()
                .WithSimpleMessaging()
                .UseEventSourcing().PersistToSql(string.Empty)
                .Initialise());

        It should_verify_that_all_event_sourcing_components_are_setup = () =>
            exception.InnerException.ShouldNotBeAssignableTo<ContainerUnverifiableException>();
    }
}