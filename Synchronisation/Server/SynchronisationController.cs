namespace SystemDot.EventSourcing.Synchronisation.Server
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Http;
    using SystemDot.Domain.Queries;

    public class SynchronisationController : ApiController
    {
        readonly IAsyncQueryHandler<CommitQuery, IEnumerable<SynchronisableCommit>> handler;

        public SynchronisationController(IAsyncQueryHandler<CommitQuery, IEnumerable<SynchronisableCommit>> handler)
        {
            this.handler = handler;
        }

        public async Task<IHttpActionResult> GetAsync(long fromInTicks)
        {
            return Ok(await handler.Handle(new CommitQuery { From = new DateTime(fromInTicks) }));
        }
    }
}