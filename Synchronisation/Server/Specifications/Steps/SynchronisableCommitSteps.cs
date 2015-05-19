using System;
using System.Collections.Generic;
using System.Linq;
using SystemDot.Serialisation;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace SystemDot.EventSourcing.Synchronisation.Server.Specifications.Steps
{
    using System.Net.Http;
    using SystemDot.EventSourcing.Headers;

    [Binding]
    public class SynchronisableCommitSteps
    {
        List<SynchronisableCommit> commits;
        readonly SynchronisableCommitContext context;
        IEnumerable<object> events;
        readonly CommitContext commitContext;
        
        public SynchronisableCommitSteps(SynchronisableCommitContext context, CommitContext commitContext)
        {
            commits = new List<SynchronisableCommit>();
            this.context = context;
            this.commitContext = commitContext;
        }

        [Given(@"I have created a synchronisable commit with an id of (.*) and stream identified as '(.*)' and client identified as '(.*)'")]
        public void GivenIHaveCreatedASynchronisableCommitWithAnIdOfAndStreamIdentifiedAs(Guid id, string streamId, string clientId)
        {
            commits.Add(new SynchronisableCommit
            {
                CommitId = id,
                StreamId = new SynchronisableEventStreamId { ClientId = clientId, Id = streamId },
                CreatedOn = DateTime.Now,
                Events = new List<SynchronisableSourcedEvent>()
            });
        }

        [Given(@"I add an event origin for the local machine as a header of the synchronisable commit")]
        public void GivenIAddAnEventOriginForTheLocalMachineAsAHeaderOfTheSynchronisableCommit()
        {
            commits.Last().Headers = new List<SynchronisableCommitHeader>
            {
                new SynchronisableCommitHeader
                {
                    Key = EventOriginHeader.Key, 
                    Value = new JsonSerialiser().Serialise(new EventOriginHeader { MachineName = System.Environment.MachineName })
                }
            };
        }

        [Given(@"I add a serialised event with an id of (.*) to the commit")]
        public void GivenIAddASerialisedEventWithAnIdOfToTheCommit(Guid id)
        {
            commits.Last().Events.Add(new SynchronisableSourcedEvent { Body = new JsonSerialiser().Serialise(new TestEvent { Id = id }) });
        }

        [When(@"I request events for synchronisation for the client '(.*)' from the beggining of time")]
        public void WhenIRequestEventsForSynchronisation(string clientId)
        {
            WhenIRequestEventsForSynchronisation(clientId, DateTime.MinValue);
        }

        [When(@"I request events for synchronisation for the client '(.*)' from (.*)")]
        public void WhenIRequestEventsForSynchronisation(string clientId, DateTime from)
        {
            commits = context.TestServer
                .HttpClient
                .GetAsync(string.Format("Synchronisation/{0}/{1}", clientId, from.Ticks))
                .Result.Content.ReadAsAsync<CommitSynchronisation>().Result.Commits;
        }

        [When(@"I request events for synchronisation for the client '(.*)' created after the date of the commit")]
        public void WhenIRequestEventsForSynchronisationCreatedAfterTheDateOfTheCommit(string clientId)
        {
            commits = context.TestServer
                .HttpClient
                .GetAsync(string.Format("Synchronisation/{0}/{1}", clientId, commitContext.CommitInUse.CreatedOn.AddMilliseconds(1).Ticks))
                .Result.Content.ReadAsAsync<CommitSynchronisation>().Result.Commits;
        }

        [When(@"I synchronise the server with the synchronisable commits")]
        public void WhenISynchroniseTheServerWithTheSynchronisableCommits()
        {
            context.TestServer
                .HttpClient
                .PostAsJsonAsync("Synchronisation", new CommitSynchronisation { Commits = commits }).Wait();
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

        [Then(@"the returned commits should be empty")]
        public void ThenTheReturnedCommitsShouldBeEmpty()
        {
            commits.Should().BeEmpty();
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