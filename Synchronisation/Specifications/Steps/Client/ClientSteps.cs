using SystemDot.EventSourcing.Synchronisation.Messages;
using SystemDot.EventSourcing.Synchronisation.Testing;
using SystemDot.Messaging.Simple;
using FluentAssertions;
using Microsoft.Owin.Testing;
using TechTalk.SpecFlow;

namespace SystemDot.Domain.Synchronisation.Specifications.Steps.Client
{
    [Binding]
    public class ClientSteps
    {
        readonly Dispatcher dispatcher;
        ClientSynchronisationCompleted completedEvent;
        
        public ClientSteps(
            TestServer testServer, 
            TestHttpClientFactory httpClientFactory,
            Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
            this.dispatcher.RegisterHandler<ClientSynchronisationCompleted>(e => completedEvent = e);
            httpClientFactory.TestServer = testServer;
        }

        [Given(@"I have initialised the client synchronisation process with the server address and client id of '(.*)'")]
        public void GivenIHaveInitialisedTheClientSynchronisationProcessWithTheServerAddressAndClientIdOf(string clientId)
        {
            dispatcher.SendAsync(new InitialiseClientSynchronisationProcess
            {
                ClientId = clientId,
                ServerUri = "http://localhost:1234/"
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