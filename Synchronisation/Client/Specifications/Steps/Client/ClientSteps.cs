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
        ClientSynchronisationCompleted completedEvent;
        
        public ClientSteps(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
            this.dispatcher.RegisterHandler<ClientSynchronisationCompleted>(e => completedEvent = e);
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

        [Then(@"the end of the synchronisation should be signalled")]
        public void ThenTheEndOfTheSynchronisationShouldBeSignalled()
        {
            completedEvent.Should().NotBeNull();
        }
    }
}