namespace SystemDot.EventSourcing.Synchronisation.Server
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Domain.Queries;

    public class SynchronisationController : ApiController
    {
        readonly IAsyncQueryHandler<CommitQuery, IEnumerable<SynchronisableCommit>> handler;

        public SynchronisationController(IAsyncQueryHandler<CommitQuery, IEnumerable<SynchronisableCommit>> handler)
        {
            this.handler = handler;
        }

        public async Task<IHttpActionResult> GetAsync(string clientId, long fromInTicks)
        {
            return Ok(await handler.Handle(
                new CommitQuery
                {
                    ClientId = clientId, 
                    From = new DateTime(fromInTicks)
                }));
        }
    }
}