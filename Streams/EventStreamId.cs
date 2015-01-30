using SystemDot.Core;

namespace SystemDot.EventSourcing.Streams
{
    public sealed class EventStreamId : Equatable<EventStreamId>
    {
        public string Id { get; private set; }
        public string BucketId { get; private set; }

        public EventStreamId(string id, string bucketId)
        {
            Id = id;
            BucketId = bucketId;
        }

        public override bool Equals(EventStreamId other)
        {
            return other.Id == Id && other.BucketId == BucketId;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode().ShiftBitsUp(2) ^ Id.GetHashCode();
        }
    }
}