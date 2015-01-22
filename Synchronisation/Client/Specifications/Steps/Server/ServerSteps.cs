using SystemDot.Domain.Synchronisation.Client.Specifications.Steps.Synchronisation;
using TechTalk.SpecFlow;

namespace SystemDot.Domain.Synchronisation.Client.Specifications.Steps.Server
{
    using System;
    using FluentAssertions;

    [Binding]
    public class ServerSteps
    {
        readonly SynchronisableCommitContext synchronisableCommitContext;
        readonly TestCommitRetrievalClient testCommitRetrievalClient;

        public ServerSteps(SynchronisableCommitContext synchronisableCommitContext, 
            TestCommitRetrievalClient testCommitRetrievalClient)
        {
            this.synchronisableCommitContext = synchronisableCommitContext;
            this.testCommitRetrievalClient = testCommitRetrievalClient;
        }

        [Given(@"I set the synchronisable commit to be returned from the server")]
        public void GivenISetTheCommitToBeReturnedFromTheServer()
        {
            testCommitRetrievalClient.Add(synchronisableCommitContext.CommitInUse);
        }

        [Then(@"the commits should have been retreived from the server with the address '(.*)'")]
        public void ThenTheCommitsShouldHaveBeenRetreivedFromTheServerWithTheAddress(string address)
        {
            testCommitRetrievalClient.LastGetCommitsAsyncCallServerUri.ToString().Should().Be(address);
        }

        [Then(@"the commits should have been retreived from the server from the beggining of time")]
        public void ThenTheCommitsShouldHaveBeenRetreivedFromTheServerFromTheBegginingOfTime()
        {
            testCommitRetrievalClient.LastGetCommitsAsyncCallFrom.Should().Be(DateTime.MinValue);
        }

        [Then(@"the commits should have been retreived from the server from the date of the last commit synchronised")]
        public void ThenTheCommitsShouldHaveBeenRetreivedFromTheServerFromTheDateOfTheLastCommitSynchronised()
        {
            testCommitRetrievalClient.LastGetCommitsAsyncCallFrom.Should().Be(synchronisableCommitContext.CommitInUse.CreatedOn);
        }
    }
}
