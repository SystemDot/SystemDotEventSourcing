using System;

namespace SystemDot.EventSourcing.Sql.Windows.Lookups
{
    public interface IAggregateLookupDataEngine
    {
        Guid Lookup(string aggregateType, string keyHash);
    }
}