using System;
using System.Collections.Generic;
using System.Linq;
using SystemDot.Domain.Queries;
using SystemDot.EventSourcing.Synchronisation;
using SystemDot.Serialisation;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace SystemDot.Domain.Synchronisation.Specifications
{
    [Binding]
    public class Synchronisation
    {
        readonly IAsyncQueryHandler<CommitQuery, IEnumerable<SynchronisableCommit>> handler;
        readonly CommitContext commitContext;
        readonly ISerialiser serialiser;
        IEnumerable<SynchronisableCommit> commits;
        SynchronisableCommit commitInUse;
        IEnumerable<object> events;

        public Synchronisation(CommitQueryHandler handler, CommitContext commitContext)
        {
            this.handler = handler;
            this.commitContext = commitContext;
            serialiser = new JsonSerialiser();
        }

        [When(@"I request events for synchronisation")]
        public void WhenIRequestEventsForSynchronisation()
        {
            commits = handler.Handle(new CommitQuery()).Result;
        }
        
        [When(@"I use the first synchronisable commit requested")]
        public void WhenIUseTheFirstSynchronisableCommitRequested()
        {
            commitInUse = commits.First();
        }
        
        [When(@"I use the second synchronisable commit requested")]
        public void WhenIUseTheSecondSynchronisableCommitRequested()
        {
            commitInUse = commits.ElementAt(1);
        }

        [When(@"I deserialise the synchronisable commit events")]
        public void WhenIDeserialiseTheSynchronisableCommitEvents()
        {
            events = commitInUse.Events.Select(e => serialiser.Deserialise(e.Body));
        }

        [Then(@"the synchronisable commit should have the same id as the commit")]
        public void ThenTheSynchronisableCommitShouldHaveTheSameIdAsTheCommit()
        {
            commitInUse.CommitId.Should().Be(commitContext.CommitInUse.CommitId);
        }

        [Then(@"the synchronisable commit should be for the same stream as the commit")]
        public void ThenTheSynchronisableCommitShouldBeForTheSameStreamAsTheCommit()
        {
            commitInUse.StreamId.Should().Be(commitContext.CommitInUse.StreamId);
        }

        [Then(@"the synchronisable commit should be for the same date and time as the commit")]
        public void ThenTheSynchronisableCommitShouldBeForTheSameDateAndTimeAsTheCommit()
        {
            commitInUse.CreatedOn.Should().Be(commitContext.CommitInUse.CreatedOn);
        }

        [Then(@"the deserialised events should contain an event an id of (.*)")]
        public void ThenTheDeserialisedEventsShouldContainAnEventAnIdOf(Guid id)
        {
            events.OfType<TestEvent>().Should().Contain(a => a.Id == id);
        }
    }
}