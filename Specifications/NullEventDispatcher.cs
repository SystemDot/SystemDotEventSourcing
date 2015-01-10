namespace SystemDot.EventSourcing.Specifications
{
    using SystemDot.Domain.Events.Dispatching;

    internal class NullEventDispatcher : IEventDispatcher
    {
        public void Dispatch(object toDispatch)
        {
        }
    }
}