using System;

namespace SystemDot.EventSourcing.Sql.Windows.Lookups
{
    public class NullLookupIdCache : ILookupIdCache
    {
        public bool TryGetId<T>(string sourceId, out Guid outputId)
        {
            outputId = default(Guid);
            return false;
        }

        public void AddId<T>(string sourceId, Guid outputId)
        {            
        }
    }
}