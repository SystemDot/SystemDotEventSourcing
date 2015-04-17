namespace SystemDot.EventSourcing.Projections
{
    using System;
    using System.Threading.Tasks;
    using SystemDot.Domain;

    public class DataStore<TProjection> where TProjection : new()
    {
        readonly EventProjector projector;

        public DataStore(EventProjector projector)
        {
            this.projector = projector;
        }

        public async Task LoadAsync(MultiSiteId id, Action<TProjection> onLoad)
        {
            await projector.ProjectAsync(id, onLoad);
        }
    }
}