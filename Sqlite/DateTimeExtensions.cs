namespace SystemDot.EventSourcing.Sqlite.Android
{
    using System;

    public static class DateTimeExtensions
    {
        public static string ToSqliteFormat(this DateTime datetime)
        {
            return datetime.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}