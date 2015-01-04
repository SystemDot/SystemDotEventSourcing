using System;
using System.Linq;
using SystemDot.EventSourcing.Sessions;
using SystemDot.EventSourcing.Streams;
using TechTalk.SpecFlow;

namespace SystemDot.Domain.Synchronisation.Specifications
{
    [Binding]
    public class Events
    {
        readonly IEventSessionFactory sessionFactory;
        readonly CommitContext context;
        IEventSession session;
        
        public Events(IEventSessionFactory sessionFactory, CommitContext context)
        {
            this.sessionFactory = sessionFactory;
            this.context = context;
        }

        [Given(@"I have created a new event session")]
        public void GivenIHaveCreatedANewEventSession()
        {
            session = sessionFactory.Create();
        }

        [Given(@"I have created an event in the session with an id of (.*) for the stream identified as (.*)")]
        public void GivenIHaveCreatedAnEventInTheSessionWithAnIdOfForTheStreamIdentifiedAs(Guid id, Guid streamId)
        {
            session.StoreEvent(new SourcedEvent { Body = new TestEvent { Id = id} }, streamId.ToString());
        }

        [Given(@"I commit the session with the id (.*)")]
        public void GivenICommitTheSessionWithTheId(Guid commitId)
        {
            session.Commit(commitId);
        }

        [When(@"I use the first commit in the event session")]
        public void WhenIUseTheFirstCommitInTheEventSession()
        {
            context.CommitInUse = session.AllCommitsFrom(DateTime.MinValue).First();
        }

        [When(@"I use the second commit in the event session")]
        public void WhenIUseTheSecondCommitInTheEventSession()
        {
            context.CommitInUse = session.AllCommitsFrom(DateTime.MinValue).ElementAt(1);
        }
    }
}
