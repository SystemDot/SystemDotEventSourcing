using System;
using System.Collections.Generic;
using SystemDot.EventSourcing.Dispatching;
using SystemDot.EventSourcing.Sessions;

namespace SystemDot.EventSourcing.Esent.Windows
{
    public class EsentEventStream : List<Object>
    {
        public Guid Id { get; set; }

        public EsentEventStream(Guid id)
        {
            Id = id;
        }

        public static EsentEventStream Get(Guid streamId)
        {
            return Db.Db.GetById<EsentEventStream>(streamId);
        }

        public void Commit()
        {
            Db.Db.Store(Id, this);
        }
    }

    public class EsentEventSession : EventSession
    {
        public EsentEventSession(IEventDispatcher eventDispatcher) : base(eventDispatcher)
        {
        }

        public override IEnumerable<object> GetEvents(Guid streamId)
        {
            return GetEventStream(streamId);
        }

        static EsentEventStream GetEventStream(Guid streamId)
        {
            return EsentEventStream.Get(streamId);
        }

        protected override void OnEventsCommitted()
        {  
        }

        protected override void OnEventCommitting(EventContainer eventContainer)
        {
            EsentEventStream stream = GetEventStream(eventContainer.AggregateRootId);
            stream.Add(eventContainer.Event);
            stream.Commit();
        }

        public override IEnumerable<object> AllEvents()
        {
            throw new NotImplementedException();
        }
    }
}