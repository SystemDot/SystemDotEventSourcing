namespace SystemDot.EventSourcing.Synchronisation.Server.Specifications.Handling
{
    using FluentAssertions;
    using TechTalk.SpecFlow;

    [Binding]
    public class EventHandlerSteps
    {
        readonly EventHandlerContext context;

        public EventHandlerSteps(EventHandlerContext context)
        {
            this.context = context;
        }

        [Then(@"none of the posted events should be dispatched")]
        public void ThenNoneOfThePostedEventsShouldBeDispatched()
        {
            context.LastEvent.Should().BeNull();
        }

        [Then(@"events should be dispatched")]
        public void ThenEventsShouldBeDispatched()
        {
            context.LastEvent.Should().NotBeNull();
        }

    }
}