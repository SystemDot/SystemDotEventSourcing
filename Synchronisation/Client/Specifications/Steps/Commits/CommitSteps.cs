using System;
using System.Linq;
using SystemDot.EventSourcing.Sessions;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace SystemDot.Domain.Synchronisation.Client.Specifications.Steps.Commits
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
