namespace SystemDot.EventSourcing.Projections
{
    using System;

    public class DataStore<TProjection> where TProjection : new()
    {
        readonly EventProjector projector;

        public DataStore(EventProjector projector)
        {
            this.projector = projector;
        }

        public void Load(Guid bucketId, Guid id, Action<TProjection> onLoad)
        {
            projector.Project(bucketId.ToString(), id.ToString(), onLoad);
        }

        public void Load(Guid bucketId, Action<TProjection> onLoad)
        {
            projector.Project(bucketId.ToString(), onLoad);
        }
    }
}