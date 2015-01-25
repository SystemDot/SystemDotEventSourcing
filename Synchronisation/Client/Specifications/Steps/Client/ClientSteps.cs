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
        ClientSynchronisationSuccessfullyCompleted successfulCompletionEvent;
        ClientSynchronisationUnsuccessful unsuccessfulCompletionEvent;
        
        public ClientSteps(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
            this.dispatcher.RegisterHandler<ClientSynchronisationSuccessfullyCompleted>(e => successfulCompletionEvent = e);
            this.dispatcher.RegisterHandler<ClientSynchronisationUnsuccessful>(e => unsuccessfulCompletionEvent = e);
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

        [When(@"I synchronise the client with the server with client id '(.*)'")]
        public void WhenISynchroniseTheClientWithTheServerWithClientId(string clientId)
        {
            dispatcher.SendAsync(new SynchroniseCommits{ ClientId = clientId }).Wait();
        }

        [Then(@"the successful completion of the synchronisation should be signalled")]
        public void ThenTheSuccsessfulCompletionOfTheSynchronisationShouldBeSignalled()
        {
            successfulCompletionEvent.Should().NotBeNull();
        }

        [Then(@"the unsuccessful completion of the synchronisation should be signalled")]
        public void ThenTheUnsuccsessfulCompletionOfTheSynchronisationShouldBeSignalled()
        {
            unsuccessfulCompletionEvent.Should().NotBeNull();
        }
    }
}