

namespace SystemDot.Domain.Synchronisation.Client.Specifications.Steps.Server
{
    using System.Net;
    using SystemDot.Domain.Synchronisation.Client.Specifications.Steps.Synchronisation;
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    using FluentAssertions;

    [Binding]
    public class ServerSteps
    {
        readonly ServerContext context;
        readonly SynchronisableCommitContext synchronisableCommitContext;

        public ServerSteps(
            SynchronisableCommitContext synchronisableCommitContext, 
            ServerContext context)
        {
            this.synchronisableCommitContext = synchronisableCommitContext;
            this.context = context;
        }

        [Given(@"the server is unavailable")]
        public void GivenTheServerIsUnavailable()
        {
            context.SynchronisationHttpClient.StatusToReturn = HttpStatusCode.BadRequest;
        }

        [Given(@"I set the synchronisable commit to be returned from the server")]
        public void GivenISetTheCommitToBeReturnedFromTheServer()
        {
            context.SynchronisationHttpClient.Add(synchronisableCommitContext.CommitInUse);
        }

        [When(@"I use the first synchronisable commit posted")]
        public void WhenIUseTheFirstSynchronisableCommitPosted()
        {
            synchronisableCommitContext.CommitInUse = context.SynchronisationHttpClient.LastPostCommitsAsyncCallContent.Commits.First();
        }
        
        [Then(@"the commits should have been posted to the server with the correct address")]
        [Then(@"the commits should have been retreived from the server with the correct address")]
        public void ThenTheCommitsShouldHaveBeenRetreivedFromTheServerWithTheAddress()
        {
            context.SynchronisationHttpClient.LastAsyncCallServerUri.Should().Be(context.ServerUri);
        }

        [Then(@"the commits should have been retreived from the server from the beggining of time")]
        public void ThenTheCommitsShouldHaveBeenRetreivedFromTheServerFromTheBegginingOfTime()
        {
            context.SynchronisationHttpClient.LastGetCommitsAsyncCallFrom.Should().Be(DateTime.MinValue.Ticks);
        }
        
        [Then(@"No commits should be pushed")]
        public void ThenNoCommitsShouldBePushed()
        {
            context.SynchronisationHttpClient.LastPostCommitsAsyncCallContent.Commits.Should().BeEmpty();
        }
    }
}
