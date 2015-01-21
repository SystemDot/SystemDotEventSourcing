using System.Net.Http;
using SystemDot.Domain.Synchronisation.Specifications.Steps.Synchronisation;
using Microsoft.Owin.Testing;
using TechTalk.SpecFlow;

namespace SystemDot.Domain.Synchronisation.Specifications.Steps.Server
{
    [Binding]
    public class ServerSteps
    {
        readonly SynchronisableCommitContext commitContext;
        readonly TestServer testServer;

        public ServerSteps(SynchronisableCommitContext commitContext, TestServer testServer)
        {
            this.commitContext = commitContext;
            this.testServer = testServer;
        }

        [Given(@"I set the synchronisable commit to be returned from the server")]
        public void GivenISetTheCommitToBeReturnedFromTheServer()
        {
            testServer
                .HttpClient
                .PostAsJsonAsync("Synchronisation", commitContext.CommitInUse)
                .Wait();
        }
    }
}
