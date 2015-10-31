namespace SystemDot.EventSourcing.Aggregation
{
    public interface IInvokeAggregate<in TCommand>
    {
        void When(TCommand command);
    }
}