using TechTalk.SpecFlow;

namespace SystemDot.Domain.Synchronisation.Client.Specifications.Steps.Client
{
    using System;
    using SystemDot.Domain.Synchronisation.Client.Specifications.Steps.Commits;
    using SystemDot.EventSourcing.Synchronisation.Client.Retrieval;
    using FluentAssertions;

    [Binding]
    public class CommitSynchroniserSteps
    {
        readonly CommitSynchroniserContext commitSynchroniserContext;
        readonly CommitContext commitContext;
        DateTime lastCommitDate;
        bool errorOccured;

        public CommitSynchroniserSteps(CommitSynchroniserContext commitSynchroniserContext, CommitContext commitContext)
        {
            this.commitSynchroniserContext = commitSynchroniserContext;
            this.commitContext = commitContext;
        }

        [When(@"I pull events from the server for the client id '(.*)'")]
        public void WhenISynchroniseTheClientWithTheServerWithClientId(string clientId)
        {
            commitSynchroniserContext.CommitSynchroniser
                .PullAsync(new CommitRetrievalCriteria(DateTime.MinValue, clientId), d => lastCommitDate = d, () => errorOccured = true)
                .Wait();
        }

        [When(@"I push events to the server for client id '(.*)'")]
        public void WhenIPushEventsToTheServer(string clientId)
        {            
            commitSynchroniserContext.CommitSynchroniser
                .PushAsync(new CommitRetrievalCriteria(DateTime.MinValue, clientId), d => lastCommitDate = d, () => errorOccured = true)
                .Wait();
        }

        [Then(@"the successful completion of the synchronisation should be signalled with the date of the last commit")]
        public void ThenTheSuccessfulCompletionOfTheSynchronisationShouldBeSignalledWithTheDateOfTheLastCommit()
        {
            lastCommitDate.Should().NotBe(DateTime.MinValue);
        }

        [Then(@"the unsuccessful completion of the synchronisation should be signalled")]
        public void ThenTheUnsuccessfulCompletionOfTheSynchronisationShouldBeSignalled()
        {
            errorOccured.Should().BeTrue();
        }

        [Then(@"the successful completion of the synchronisation should not be signalled")]
        public void ThenTheSuccessfulCompletionOfTheSynchronisationShouldNotBeSignalled()
        {
            lastCommitDate.Should().Be(DateTime.MinValue);
        }
    }
}