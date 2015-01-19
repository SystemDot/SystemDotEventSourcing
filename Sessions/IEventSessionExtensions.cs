using System;
using SystemDot.Environment;
using SystemDot.EventSourcing.Headers;
using SystemDot.EventSourcing.Streams;

namespace SystemDot.EventSourcing.Sessions
{
    public static class IEventSessionExtensions
    {
        public static void StoreEvent<TEvent>(this IEventSession session, Action<TEvent> eventIntialiser, string id, ILocalMachine localMachine)
            where TEvent : new()
        {
            var @event = new TEvent();
            eventIntialiser(@event);
            session.StoreEvent(@event, id, localMachine);
        }

        public static void StoreEvent<TEvent>(this IEventSession session, TEvent @event, string id, ILocalMachine localMachine)
            where TEvent : new()
        {
            session.StoreEvent(new SourcedEvent { Body = @event }, id);
            session.StoreHeader(id, EventOriginHeader.Key, EventOriginHeader.ForMachine(localMachine));
                    
        }

        public static void StoreEventAndCommit<TEvent>(this IEventSession session, TEvent @event, string id, ILocalMachine localMachine)
            where TEvent : new()
        {
            session.StoreEvent(@event, id, localMachine);
            session.Commit(Guid.NewGuid());
        }
    }
}