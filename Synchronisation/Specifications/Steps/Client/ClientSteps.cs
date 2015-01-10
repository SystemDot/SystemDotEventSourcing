using SystemDot.Domain.Commands;
using SystemDot.EventSourcing.Synchronisation.Client;
using SystemDot.EventSourcing.Synchronisation.Testing;
using Microsoft.Owin.Testing;
using TechTalk.SpecFlow;

namespace SystemDot.Domain.Synchronisation.Specifications.Steps.Client
{
    [Binding]
    public class ClientSteps
    {
        readonly IAsyncCommandHandler<SynchroniseCommits> handler;

        public ClientSteps(SynchroniseCommitsHandler handler, TestServer testServer, TestHttpClientFactory httpClientFactory)
        {
            this.handler = handler;
            httpClientFactory.TestServer = testServer;
        }

        [When(@"I synchronise the client with the server")]
        public void WhenISynchroniseTheClientWithTheServer()
        {
            handler.Handle(new SynchroniseCommits { ServerUri = "http://localhost:1234/" }).Wait();
        }
    }
}