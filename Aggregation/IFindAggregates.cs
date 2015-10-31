namespace SystemDot.EventSourcing.Aggregation
{
    using SystemDot.Domain;

    public interface IFindAggregates<in TCommand>
    {
        MultiSiteId GetIdFromCommandToFindAggregateWith(TCommand message);
    }
}