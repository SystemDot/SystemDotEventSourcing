using System;
using System.Linq;
using SystemDot.EventSourcing.Sessions;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace SystemDot.Domain.Synchronisation.Client.Specifications.Steps.Commits
{
    using SystemDot.EventSourcing.Headers;
    using SystemDot.EventSourcing.Streams;

    [Binding]
    public class CommitSteps
    {
        readonly CommitContext context;
        IEventSession session;
        
        public CommitSteps(CommitContext context)
        {
            this.context = context;
        }

        [Given(@"I have created a new event session")]
        public void GivenIHaveCreatedANewEventSession()
        {
            session = context.EventSessionFactory.Create();
        }
        
        [Given(@"I have created an event in the session with an id of (.*) for the stream identified as (.*) in the bucket identified as '(.*)'")]
        public void GivenIHaveCreatedAnEventInTheSessionWithAnIdOfForTheStreamIdentifiedAs(Guid id, Guid streamId, string bucketId)
        {
            session.StoreEvent(new SourcedEvent { Body = new TestEvent { Id = id } }, new EventStreamId(streamId.ToString(), bucketId));
        }

        [Given(@"I add an event origin for the local machine as a header for the stream identified as (.*) in the bucket identified as '(.*)'")]
        public void GivenIAddAnEventOriginForTheLocalMachineAsAHeaderToTheEventSession(Guid streamId, string bucketId)
        {
            session.StoreHeader(new EventStreamId(streamId.ToString(), bucketId), EventOriginHeader.Key, new EventOriginHeader { MachineName = System.Environment.MachineName });
        }

        [Given(@"I add an event origin for another machine as a header for the stream identified as (.*) in the bucket identified as '(.*)'")]
        public void GivenIAddAnEventOriginForAnotherMachineAsAHeaderToTheEventSession(Guid streamId, string bucketId)
        {
            session.StoreHeader(new EventStreamId(streamId.ToString(), bucketId), EventOriginHeader.Key, new EventOriginHeader { MachineName = "MachineName" });
        }

        [Given(@"I commit the session with the id (.*)")]
        public void GivenICommitTheSessionWithTheId(Guid commitId)
        {
            session.Commit(commitId);
        }

        [When(@"I use the commit in the session with an id of (.*)")]
        public void WhenIUseTheCommitInTheSessionWithAnIdOf(Guid commitId)
        {
            context.CommitInUse = session.AllCommits().Single(c => c.CommitId == commitId);
        }
        
        [When(@"I use the first commit in the event session")]
        public void WhenIUseTheFirstCommitInTheEventSession()
        {
            context.CommitInUse = session.AllCommits().ElementAt(0);
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
