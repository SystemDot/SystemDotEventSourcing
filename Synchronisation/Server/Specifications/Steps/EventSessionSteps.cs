using System;
using System.Linq;
using SystemDot.EventSourcing.Sessions;
using SystemDot.EventSourcing.Streams;
using TechTalk.SpecFlow;

namespace SystemDot.EventSourcing.Synchronisation.Server.Specifications.Steps
{
    [Binding]
    public class EventSessionSteps
    {
        IEventSession session;
        readonly IEventSessionFactory sessionFactory;
        readonly CommitContext context;

        public EventSessionSteps(IEventSessionFactory sessionFactory, CommitContext context)
        {
            this.sessionFactory = sessionFactory;
            this.context = context;
        }

        [Given(@"I have created a new event session")]
        public void GivenIHaveCreatedANewEventSession()
        {
            session = sessionFactory.Create();
        }

        [Given(@"I have created an event in the session with an id of (.*) for the stream identified as (.*) in the bucket identified as '(.*)'")]
        public void GivenIHaveCreatedAnEventInTheSessionWithAnIdOfForTheStreamIdentifiedAs(Guid id, Guid streamId, string bucketId)
        {
            session.StoreEvent(new SourcedEvent { Body = new TestEvent { Id = id } }, new EventStreamId(streamId.ToString(), bucketId));
        }

        [Given(@"I commit the session with the id (.*)")]
        public void GivenICommitTheSessionWithTheId(Guid commitId)
        {
            session.Commit(commitId);
        }

        [When(@"I use the first commit in the event session")]
        public void WhenIUseTheFirstCommitInTheEventSession()
        {
            context.CommitInUse = session.AllCommits().ElementAt(0);
        }

        [When(@"I use the second commit in the event session")]
        public void WhenIUseTheSecondCommitInTheEventSession()
        {
            context.CommitInUse = session.AllCommits().ElementAt(1);
        }
    }
}
