using System;
using System.Collections.Generic;
using SystemDot.EventSourcing.Streams;
using SystemDot.Core;

namespace SystemDot.EventSourcing.Commits
{
    using System.Text;

    public class Commit
    {
        public Guid CommitId { get; private set; }

        public string StreamId { get; private set; }

        public string BucketId { get; private set; }

        public IEnumerable<SourcedEvent> Events { get; private set; }

        public DateTime CreatedOn { get; private set; }

        public IDictionary<string, object> Headers { get; private set; }

        public T GetHeader<T>(string key)
        {
            return Headers[key].As<T>();
        }

        public Commit(Guid commitId, string bucketId, string streamId, IEnumerable<SourcedEvent> events, IDictionary<string, object> headers)
        {
            CommitId = commitId;
            StreamId = streamId;
            BucketId = bucketId;
            Events = events;
            CreatedOn = DateTime.Now;
            Headers = headers;
        }

        public override string ToString()
        {
            return new StringBuilder()
                .AppendDelimeted(CommitId)
                .AppendDelimeted(StreamId)
                .AppendDelimeted(BucketId)
                .AppendDelimeted(CreatedOn)
                .AppendDelimeted(Headers.SerialiseToString())
                .Append(Events.SerialiseToString()).ToString();
        }

    }
}