namespace SqlliteEventStore
{
    using System;

    public static class DateTimeExtensions
    {
        public static string ToSqliteFormat(this DateTime datetime)
        {
            return string.Format("{0}-{1}-{2} {3}:{4}:{5}.{6}", datetime.Year, datetime.Month, datetime.Day, datetime.Hour, datetime.Minute, datetime.Second, datetime.Millisecond);
        }
    }
}