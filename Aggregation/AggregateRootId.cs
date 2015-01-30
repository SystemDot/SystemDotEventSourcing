using SystemDot.Core;
using SystemDot.EventSourcing.Streams;

namespace SystemDot.EventSourcing.Aggregation
{
    public abstract class AggregateRootId : Equatable<AggregateRootId>
    {
        public string Id { get; private set; }
        readonly string bucketId;

        protected AggregateRootId(string id, string bucketId)
        {
            Id = id;
            this.bucketId = bucketId;
        }

        public override bool Equals(AggregateRootId other)
        {
            return other.Id == Id && other.bucketId == bucketId;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode().ShiftBitsUp(2) ^ Id.GetHashCode();
        }

        internal EventStreamId ToEventStreamId()
        {
            return new EventStreamId(Id, bucketId);
        }
    }
}