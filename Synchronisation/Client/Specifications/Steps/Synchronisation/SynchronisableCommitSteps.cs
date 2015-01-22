using System;
using System.Collections.Generic;
using SystemDot.Domain.Synchronisation.Client.Specifications.Steps.Temporal;
using SystemDot.EventSourcing.Synchronisation;
using SystemDot.Serialisation;
using TechTalk.SpecFlow;

namespace SystemDot.Domain.Synchronisation.Client.Specifications.Steps.Synchronisation
{
    [Binding]
    public class SynchronisableCommitSteps
    {
        readonly SynchronisableCommitContext context;
        readonly DateTimeContext dateTimeContext;
        
        public SynchronisableCommitSteps(SynchronisableCommitContext context, DateTimeContext dateTimeContext)
        {
            this.context = context;
            this.dateTimeContext = dateTimeContext;
        }

        [Given(@"I have created a synchronisable commit with an id of (.*) and stream identified as '(.*)' for the current date and time")]
        public void GivenIHaveCreatedASynchronisableCommitWithAnIdOfAndStreamIdentifiedAs(Guid id, string streamId)
        {
            context.CommitInUse = new SynchronisableCommit
            {
                CommitId = id, 
                StreamId = streamId, 
                CreatedOn = dateTimeContext.Current,
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