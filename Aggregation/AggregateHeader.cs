using System;

namespace SystemDot.EventSourcing.Aggregation
{
    public class AggregateHeader
    {
        public const string Key = "AggregateType";

        public static AggregateHeader FromType(Type type)
        {
            return new AggregateHeader { Type = type };
        }

        public Type Type { get; set; }
    }
}