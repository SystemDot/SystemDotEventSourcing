using System;
using System.Collections.Generic;
using SystemDot.EventSourcing.Synchronisation;
using SystemDot.Serialisation;
using TechTalk.SpecFlow;

namespace SystemDot.Domain.Synchronisation.Client.Specifications.Steps.Synchronisation
{
    using System.Linq;
    using SystemDot.Domain.Synchronisation.Client.Specifications.Steps.Commits;
    using FluentAssertions;

    [Binding]
    public class SynchronisableCommitSteps
    {
        readonly SynchronisableCommitContext context;
        readonly CommitContext commitContext;
        IEnumerable<object> events;

        public SynchronisableCommitSteps(SynchronisableCommitContext context, CommitContext commitContext)
        {
            this.context = context;
            this.commitContext = commitContext;
        }

        [Given(@"I have created a synchronisable commit with an id of (.*) and stream identified as '(.*)' and client identified as '(.*)'")]
        public void GivenIHaveCreatedASynchronisableCommitWithAnIdOfAndStreamIdentifiedAs(Guid id, string streamId, string clientId)
        {
            context.CommitInUse = new SynchronisableCommit
            {
                CommitId = id,
                StreamId = new SynchronisableEventStreamId { ClientId = clientId, Id = streamId }, 
                CreatedOn = DateTime.Now,
                Events = new List<SynchronisableSourcedEvent>()
            };
        }

        [Given(@"I add a serialised event with an id of (.*) to the commit")]
        public void GivenIAddASerialisedEventWithAnIdOfToTheCommit(Guid id)
        {
            context.CommitInUse.Events.Add(new SynchronisableSourcedEvent { Body = new JsonSerialiser().Serialise(new TestEvent { Id = id }) });
        }
        
        [When(@"I deserialise the synchronisable commit events")]
        public void WhenIDeserialiseTheSynchronisableCommitEvents()
        {
            events = context.CommitInUse.Events.Select(e => new JsonSerialiser().Deserialise(e.Body));
        }

        [Then(@"the deserialised events should contain an event an id of (.*)")]
        public void ThenTheDeserialisedEventsShouldContainAnEventAnIdOf(Guid id)
        {
            events.OfType<TestEvent>().Should().Contain(a => a.Id == id);
        }

        [Then(@"the deserialised events should not contain an event an id of (.*)")]
        public void ThenTheDeserialisedEventsShouldNotContainAnEventAnIdOf(Guid id)
        {
            events.OfType<TestEvent>().Should().NotContain(a => a.Id == id);
        }

        [Then(@"the synchronisable commit should have the same id as the commit")]
        public void ThenTheSynchronisableCommitShouldHaveTheSameIdAsTheCommit()
        {
            context.CommitInUse.CommitId.Should().Be(commitContext.CommitInUse.CommitId);
        }

        [Then(@"the synchronisable commit should be for the same stream as the commit")]
        public void ThenTheSynchronisableCommitShouldBeForTheSameStreamAsTheCommit()
        {
            context.CommitInUse.StreamId.Id.Should().Be(commitContext.CommitInUse.StreamId);
        }

        [Then(@"the synchronisable commit should be for the same client as the commit")]
        public void ThenTheSynchronisableClientShouldBeForTheSameStreamAsTheCommit()
        {
            context.CommitInUse.StreamId.ClientId.Should().Be(commitContext.CommitInUse.BucketId);
        }

        [Then(@"the synchronisable commit should be for the same date and time as the commit")]
        public void ThenTheSynchronisableCommitShouldBeForTheSameDateAndTimeAsTheCommit()
        {
            context.CommitInUse.CreatedOn.Should().Be(commitContext.CommitInUse.CreatedOn);
        }
    }
}