namespace SqlliteEventStore
{
    using System;
    using System.Data.Common;

    public static class DbConnectionExtensions
    {
        static DbCommand GetCommand(this DbConnection connection, string toExecute)
        {
            DbCommand command = connection.CreateCommand();
            command.CommandText = toExecute;

            return command;
        }

        public static void ExecuteReader(this DbConnection connection, string toExecute, Action<DbDataReader> onRowRead)
        {
            using (var command = connection.GetCommand(toExecute))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        onRowRead(reader);
                    }
                    reader.Close();
                }
            }
        }

        public static int Execute(this DbConnection connection, string toExecute, Action<DbCommand> onCommandInit)
        {
            using (var command = connection.GetCommand(toExecute))
            {
                onCommandInit(command);
                return command.ExecuteNonQuery();
            }
        }
    }
}