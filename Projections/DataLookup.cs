namespace SystemDot.EventSourcing.Projections
{
    using System.Threading.Tasks;
    using SystemDot.Domain;

    public class DataLookup<TProjection> where TProjection : new()
    {
        readonly EventProjector projector;

        public DataLookup(EventProjector projector)
        {
            this.projector = projector;
        }

        public async Task<TProjection> LookupAsync(MultiSiteId id)
        {
            TProjection projection = default(TProjection);
            await projector.ProjectAsync<TProjection>(id, p => projection = p);
            return projection;
        }
    }
}