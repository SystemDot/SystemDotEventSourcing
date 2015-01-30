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
            context.CommitInUse = session.AllCommits().Single(c => c.CommitId == commitId);
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

        [Then(@"a commit with an id of (.*) should not exist in the session")]
        public void ThenACommitWithAnIdOfShouldNotExistInTheSession(Guid commitId)
        {
            session.AllCommits().Should().NotContain(c => c.CommitId == commitId);
        }
    }
}
