using System;

namespace SystemDot.EventSourcing.Sessions
{
    public static class IEventSessionExtensions
    {
        public static void StoreEvent<TEvent>(this IEventSession session, Action<TEvent> eventIntialiser, string id)
            where TEvent : new()
        {
            var @event = new TEvent();
            eventIntialiser(@event);
            session.StoreEvent(new SourcedEvent { Body = @event }, id);
        }
    }
}