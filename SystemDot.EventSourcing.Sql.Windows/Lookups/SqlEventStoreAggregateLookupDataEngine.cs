using System;
using System.Data;
using System.Data.SqlClient;
using SystemDot.Core;
using EventStore.Persistence.SqlPersistence;

namespace SystemDot.EventSourcing.Sql.Windows.Lookups
{
    public class SqlEventStoreAggregateLookupDataEngine : IAggregateLookupDataEngine
    {
        public Guid Lookup(string aggregateType, string keyHash)
        {
            Guid id;

            using (var scope = ConfigurationConnectionFactory.OpenScope())
            {
                var command = scope.As<ConnectionScope>().Current.CreateCommand();
                command.CommandText = "[dbo].[LookupAggregate]";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@Type", aggregateType));
                command.Parameters.Add(new SqlParameter("@SourceKey", keyHash));

                id = (Guid)command.ExecuteScalar();
            }

            return id;
        }
    }
}