namespace SystemDot.Domain.Synchronisation.Client.Specifications.Steps.Handling
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

        [Then(@"none of the pulled events should be dispatched")]
        public void ThenNoneOfThePulledEventsShouldBeDispatched()
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