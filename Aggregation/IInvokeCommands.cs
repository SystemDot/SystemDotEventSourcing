namespace SystemDot.EventSourcing.Aggregation
{
    public interface IInvokeCommands<in TCommand>
    {
        void When(TCommand command);
    }
}