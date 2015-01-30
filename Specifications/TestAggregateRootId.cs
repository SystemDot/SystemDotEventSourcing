using SystemDot.EventSourcing.Aggregation;

namespace SystemDot.EventSourcing.Specifications
{
    public class TestAggregateRootId : AggregateRootId
    {
        public string BucketId { get; set; }

        public TestAggregateRootId(string id, string bucketId)
            : base(id, bucketId)
        {
            BucketId = bucketId;
        }
    }
}