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

        protected void Then(object @event)
        {
            Root.Then(@event);
        }

        protected void Then<T>(Action<T> initaliseEvent) where T : new()
        {
            Root.Then(initaliseEvent);
        }

        void OnAggregateRootEventAdded(object sender, EventSourceEventArgs e)
        {
            eventRouter.RouteEventToHandlers(e.Event);
        }
    }
}