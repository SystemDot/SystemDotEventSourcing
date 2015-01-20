using TechTalk.SpecFlow;

namespace SystemDot.Domain.Synchronisation.Specifications.Steps.Temporal
{
    [Binding]
    public class DateTimeSteps
    {
        DateTimeContext context;

        public DateTimeSteps(DateTimeContext context)
        {
            this.context = context;
        }

        [Given(@"I have set the current date and time back (.*) millisecond")]
        public void GivenIHaveSetTheCurrentDateAndTimeBackMillisecond(int milliseconds)
        {
            context.Current = context.Current.AddMilliseconds(-milliseconds);
        }
    }
}