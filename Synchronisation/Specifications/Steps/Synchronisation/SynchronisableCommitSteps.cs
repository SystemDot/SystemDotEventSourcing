using System;
using System.Collections.Generic;
using System.Linq;
using SystemDot.Domain.Queries;
using SystemDot.Domain.Synchronisation.Specifications.Steps.Commits;
using SystemDot.EventSourcing.Synchronisation;
using SystemDot.EventSourcing.Synchronisation.Server;
using SystemDot.Serialisation;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace SystemDot.Domain.Synchronisation.Specifications.Steps.Synchronisation
{
    [Binding]
    public class SynchronisableCommitSteps
    {
        readonly IAsyncQueryHandler<CommitQuery, IEnumerable<SynchronisableCommit>> handler;
        readonly SynchronisableCommitContext context;
        readonly CommitContext commitContext;
        IEnumerable<SynchronisableCommit> commits;
        
        IEnumerable<object> events;

        public SynchronisableCommitSteps(CommitQueryHandler handler, SynchronisableCommitContext context, CommitContext commitContext)
        {
            this.handler = handler;
            this.context = context;
            this.commitContext = commitContext;
        }

        [Given(@"I have created a synchronisable commit with an id of (.*) and stream identified as '(.*)'")]
        public void GivenIHaveCreatedASynchronisableCommitWithAnIdOfAndStreamIdentifiedAs(Guid id, string streamId)
        {
            context.CommitInUse = new SynchronisableCommit
            {
                CommitId = id, 
                StreamId = streamId, 
                CreatedOn = DateTime.Now,
                Events = new List<SynchronisableSourcedEvent>()
            };
        }

        [Given(@"I add a serialised event with an id of (.*) to the commit")]
        public void GivenIAddASerialisedEventWithAnIdOfToTheCommit(Guid id)
        {
            context.CommitInUse.Events.Add(new SynchronisableSourcedEvent { Body = new JsonSerialiser().Serialise(new TestEvent { Id = id }) });
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

        [Then(@"the deserialised events should contain an event an id of (.*)")]
        public void ThenTheDeserialisedEventsShouldContainAnEventAnIdOf(Guid id)
        {
            events.OfType<TestEvent>().Should().Contain(a => a.Id == id);
        }
    }
}