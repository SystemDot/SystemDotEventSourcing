using System;

namespace SystemDot.EventSourcing.Aggregation
{
    public abstract class AggregateEntity<TRoot> where TRoot : AggregateRoot
    {
        readonly ConventionEventToHandlerRouter eventRouter;

        public TRoot Root { get; private set; }

        protected AggregateEntity(TRoot root)
        {
            Root = root;
            Root.EventReplayed += OnAggregateRootEventAdded;
            eventRouter = new ConventionEventToHandlerRouter(this, "ApplyEvent");
        }

        protected void AddEvent(object @event)
        {
            Root.AddEvent(@event);
        }

        protected void AddEvent<T>(Action<T> initaliseEvent) where T : new()
        {
            Root.AddEvent(initaliseEvent);
        }

        void OnAggregateRootEventAdded(object sender, EventSourceEventArgs e)
        {
            this.eventRouter.RouteEventToHandlers(e.Event);
        }
    }
}