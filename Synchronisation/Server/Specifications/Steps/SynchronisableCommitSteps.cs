using System;
using System.Collections.Generic;
using System.Linq;
using SystemDot.Domain.Queries;
using SystemDot.Serialisation;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace SystemDot.EventSourcing.Synchronisation.Server.Specifications.Steps
{
    [Binding]
    public class SynchronisableCommitSteps
    {
        IEnumerable<SynchronisableCommit> commits;
        readonly IAsyncQueryHandler<CommitQuery, IEnumerable<SynchronisableCommit>> handler;
        readonly SynchronisableCommitContext context;
        IEnumerable<object> events;
        CommitContext commitContext;

        public SynchronisableCommitSteps(CommitQueryHandler handler, SynchronisableCommitContext context, CommitContext commitContext)
        {
            this.handler = handler;
            this.context = context;
            this.commitContext = commitContext;
        }

        [When(@"I request events for synchronisation")]
        public void WhenIRequestEventsForSynchronisation()
        {
            commits = handler.Handle(new CommitQuery()).Result;
        }

        [When(@"I use the first synchronisable commit requested")]
        public void WhenIUseTheFirstSynchronisableCommitRequested()
        {
            context.CommitInUse = commits.First();
        }

        [When(@"I use the second synchronisable commit requested")]
        public void WhenIUseTheSecondSynchronisableCommitRequested()
        {
            context.CommitInUse = commits.ElementAt(1);
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

        [Then(@"the synchronisable commit should have the same id as the commit")]
        public void ThenTheSynchronisableCommitShouldHaveTheSameIdAsTheCommit()
        {
            context.CommitInUse.CommitId.Should().Be(commitContext.CommitInUse.CommitId);
        }

        [Then(@"the synchronisable commit should be for the same stream as the commit")]
        public void ThenTheSynchronisableCommitShouldBeForTheSameStreamAsTheCommit()
        {
            context.CommitInUse.StreamId.Should().Be(commitContext.CommitInUse.StreamId);
        }

        [Then(@"the synchronisable commit should be for the same date and time as the commit")]
        public void ThenTheSynchronisableCommitShouldBeForTheSameDateAndTimeAsTheCommit()
        {
            context.CommitInUse.CreatedOn.Should().Be(commitContext.CommitInUse.CreatedOn);
        }
    }
}