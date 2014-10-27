using System;
using System.Text;

namespace SystemDot.EventSourcing.Sql.Windows.Lookups
{
    public class AggregateLookup : IAggregateLookup
    {
        readonly IAggregateLookupDataEngine dataEngine;
        readonly ILookupIdCache cache;

        public AggregateLookup(IAggregateLookupDataEngine dataEngine, ILookupIdCache cache)
        {
            this.dataEngine = dataEngine;
            this.cache = cache;
        }

        public Guid LookupId<T>(string sourceId)
        {
            Guid outputId;

            if (!cache.TryGetId<T>(sourceId, out outputId))
            {
                outputId = LookupNonCachedId<T>(sourceId);
                cache.AddId<T>(sourceId, outputId);
            }
            return outputId;
        }

        Guid LookupNonCachedId<T>(string sourceId)
        {
            return dataEngine.Lookup(typeof(T).Name, sourceId);
        }
    }
}