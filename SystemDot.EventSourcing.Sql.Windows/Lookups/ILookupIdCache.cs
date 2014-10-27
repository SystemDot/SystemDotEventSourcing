using System;

namespace SystemDot.EventSourcing.Sql.Windows.Lookups
{
    public interface ILookupIdCache
    {
        bool TryGetId<T>(string sourceId, out Guid outputId);
        void AddId<T>(string sourceId, Guid outputId);
    }
}