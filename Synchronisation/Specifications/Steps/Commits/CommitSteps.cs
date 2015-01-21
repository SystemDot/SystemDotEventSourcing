using System;
using System.Linq;
using SystemDot.EventSourcing.Sessions;
using SystemDot.EventSourcing.Streams;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace SystemDot.Domain.Synchronisation.Specifications.Steps.Commits
{
    [Binding]
    public class CommitSteps
    {
        readonly IEventSessionFactory sessionFactory;
        readonly CommitContext context;
        IEventSession session;
        
        public CommitSteps(IEventSessionFactory sessionFactory, CommitContext context)
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
            context.CommitInUse = session.AllCommitsFrom(DateTime.MinValue).ElementAt(0);
        }

        [When(@"I use the second commit in the event session")]
        public void WhenIUseTheSecondCommitInTheEventSession()
        {
            context.CommitInUse = session.AllCommitsFrom(DateTime.MinValue).ElementAt(1);
        }

        [When(@"I use the commit in the session with an id of (.*)")]
        public void WhenIUseTheCommitInTheSessionWithAnIdOf(Guid commitId)
        {
            context.CommitInUse = session.AllCommitsFrom(DateTime.MinValue).Single(c => c.CommitId == commitId);
        }

        [Then(@"there should be a commit in the session with an id of (.*)")]
        public void ThenThereShouldBeACommitInTheSessionWithAnIdOf(Guid commitId)
        {
            session.AllCommitsFrom(DateTime.MinValue).Should().Contain(c => c.CommitId == commitId);
        }

        [Then(@"there should not be a commit in the session with an id of (.*)")]
        public void ThenThereShouldNotBeACommitInTheSessionWithAnIdOf(Guid commitId)
        {
            session.AllCommitsFrom(DateTime.MinValue).Should().NotContain(c => c.CommitId == commitId);
        }
        
        [Then(@"the commit should be for a stream identified as '(.*)'")]
        public void ThenTheCommitShouldBeForAStreamIdentifiedAs(string id)
        {
            context.CommitInUse.StreamId.Should().Be(id);
        }

        [Then(@"the commit should contain an event with an id of (.*)")]
        public void ThenTheCommitShouldContainAnEventWithAnIdOf(Guid id)
        {
            context.CommitInUse.Events.Should().Contain(e => e.Body.As<TestEvent>().Id == id);
        }
    }
}
