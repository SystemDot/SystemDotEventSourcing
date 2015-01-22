using SystemDot.Domain.Synchronisation.Client.Specifications.Steps.Synchronisation;
using SystemDot.EventSourcing.Synchronisation.Testing;
using TechTalk.SpecFlow;

namespace SystemDot.Domain.Synchronisation.Client.Specifications.Steps.Server
{
    [Binding]
    public class ServerSteps
    {
        readonly SynchronisableCommitContext commitContext;
        readonly TestCommitRetrievalClient testCommitRetrievalClient;

        public ServerSteps(SynchronisableCommitContext commitContext, TestCommitRetrievalClient testCommitRetrievalClient)
        {
            this.commitContext = commitContext;
            this.testCommitRetrievalClient = testCommitRetrievalClient;
        }

        [Given(@"I set the synchronisable commit to be returned from the server")]
        public void GivenISetTheCommitToBeReturnedFromTheServer()
        {
            testCommitRetrievalClient.Add(commitContext.CommitInUse);
        }
    }
}
