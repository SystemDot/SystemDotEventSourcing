using System;

namespace SystemDot.Domain.Synchronisation.Specifications.Steps.Temporal
{
    public class DateTimeContext
    {
        public DateTime Current { get; set; }

        public DateTimeContext()
        {
            Current = DateTime.Now;
        }
    }
}