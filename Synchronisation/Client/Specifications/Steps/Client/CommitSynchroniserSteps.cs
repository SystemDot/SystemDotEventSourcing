using TechTalk.SpecFlow;

namespace SystemDot.Domain.Synchronisation.Client.Specifications.Steps.Client
{
    using System;
    using SystemDot.EventSourcing.Synchronisation.Client;
    using SystemDot.EventSourcing.Synchronisation.Client.Retrieval;
    using FluentAssertions;

    [Binding]
    public class CommitSynchroniserSteps
    {
        readonly CommitSynchroniserContext commitSynchroniserContext;
        CommitSynchronisationResult result;
        bool errorOccured;

        public CommitSynchroniserSteps(CommitSynchroniserContext commitSynchroniserContext)
        {
            this.commitSynchroniserContext = commitSynchroniserContext;
        }

        [When(@"I synchronise commits for the client id '(.*)'")]
        public void WhenISynchroniseTheClientWithTheServerWithClientId(string clientId)
        {
            commitSynchroniserContext.CommitSynchroniser
                .SynchroniseAsync(new CommitSynchronisationCriteria(DateTime.MinValue, DateTime.MinValue, clientId), r => result = r, () => errorOccured = true)
                .Wait();
        }
        
        [Then(@"the successful completion of the synchronisation should be signalled with the date of the last pull commit")]
        public void ThenTheSuccessfulCompletionOfTheSynchronisationShouldBeSignalledWithTheDateOfTheLastPullCommit()
        {
            result.LastPullCommitDate.Should().NotBe(DateTime.MinValue);
        }
        
        [Then(@"the successful completion of the synchronisation should be signalled with the date of the last push commit")]
        public void ThenTheSuccessfulCompletionOfTheSynchronisationShouldBeSignalledWithTheDateOfTheLastPushCommit()
        {
            result.LastPushCommitDate.Should().NotBe(DateTime.MinValue);
        }

        [Then(@"the unsuccessful completion of the synchronisation should be signalled")]
        public void ThenTheUnsuccessfulCompletionOfTheSynchronisationShouldBeSignalled()
        {
            errorOccured.Should().BeTrue();
        }

        [Then(@"the successful completion of the synchronisation should not be signalled")]
        public void ThenTheSuccessfulCompletionOfTheSynchronisationShouldNotBeSignalled()
        {
            result.Should().BeNull();
        }
    }
}