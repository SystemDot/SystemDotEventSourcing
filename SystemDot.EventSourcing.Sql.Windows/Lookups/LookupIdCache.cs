using System;
using System.Collections.Generic;

namespace SystemDot.EventSourcing.Sql.Windows.Lookups
{
    public class LookupIdCache : ILookupIdCache
    {
        readonly Dictionary<string, Guid> cache = new Dictionary<string, Guid>();

        public bool TryGetId<T>(string sourceId, out Guid outputId)
        {
            return cache.TryGetValue(GetLookUpCacheKey<T>(sourceId), out outputId);
        }

        public void AddId<T>(string sourceId, Guid outputId)
        {
            cache.Add(GetLookUpCacheKey<T>(sourceId), outputId);
        }

        static string GetLookUpCacheKey<T>(string sourceId)
        {
            return typeof(T).Name + sourceId;
        }
    }
}