using System;
using System.Diagnostics.Contracts;

namespace SystemDot.EventSourcing.Sql.Windows.Lookups
{
    public interface IAggregateLookup
    {
        Guid LookupId<T>(string sourceId);
    }
}