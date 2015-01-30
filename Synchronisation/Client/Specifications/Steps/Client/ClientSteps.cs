using SystemDot.EventSourcing.Synchronisation.Client.Messages;
using SystemDot.Messaging.Simple;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace SystemDot.Domain.Synchronisation.Client.Specifications.Steps.Client
{
    [Binding]
    public class ClientSteps
    {
        readonly Dispatcher dispatcher;
        int successfulCompletionEventCount;
        int unsuccessfulCompletionEventCount;
        
        public ClientSteps(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
            this.dispatcher.RegisterHandler<ClientSynchronisationSuccessfullyCompleted>(e => successfulCompletionEventCount++);
            this.dispatcher.RegisterHandler<ClientSynchronisationUnsuccessful>(e => unsuccessfulCompletionEventCount++);
        }

        [Given(@"I have initialised the client synchronisation process with the server address of '(.*)' and client id of '(.*)'")]
        public void GivenIHaveInitialisedTheClientSynchronisationProcessWithTheServerAddressAndClientIdOf(string address, string clientId)
        {
            dispatcher.SendAsync(new InitialiseClientSynchronisationProcess
            {
                ClientId = clientId,
                ServerUri = address
            }).Wait();
        }

        [When(@"I synchronise the client with the server")]
        public void WhenISynchroniseTheClientWithTheServerWithClientId()
        {
            dispatcher.SendAsync(new SynchroniseCommits()).Wait();
        }

        [Then(@"the successful completion of the synchronisation should be signalled")]
        public void ThenTheSuccsessfulCompletionOfTheSynchronisationShouldBeSignalled()
        {
            successfulCompletionEventCount.Should().Be(1);
        }

        [Then(@"the successful completion of the synchronisation should not be signalled")]
        public void ThenTheSuccessfulCompletionOfTheSynchronisationShouldNotBeSignalled()
        {
            successfulCompletionEventCount.Should().Be(0);
        }

        [Then(@"the unsuccessful completion of the synchronisation should be signalled")]
        public void ThenTheUnsuccsessfulCompletionOfTheSynchronisationShouldBeSignalled()
        {
            unsuccessfulCompletionEventCount.Should().Be(1);
        }
    }
}