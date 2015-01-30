using System;
using System.Collections.Generic;
using SystemDot.EventSourcing.Synchronisation;
using SystemDot.Serialisation;
using TechTalk.SpecFlow;

namespace SystemDot.Domain.Synchronisation.Client.Specifications.Steps.Synchronisation
{
    [Binding]
    public class SynchronisableCommitSteps
    {
        readonly SynchronisableCommitContext context;
        
        public SynchronisableCommitSteps(SynchronisableCommitContext context)
        {
            this.context = context;
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
    }
}