namespace SystemDot.EventSourcing.Synchronisation.Server
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Http;
    using SystemDot.Domain.Commands;
    using SystemDot.EventSourcing.Commits;
    using Domain.Queries;

    public class SynchronisationController : ApiController
    {
        readonly IAsyncQueryHandler<CommitQuery, IEnumerable<SynchronisableCommit>> queryHandler;
        readonly IAsyncCommandHandler<CommitSynchronisableCommits> commandHandler;

        public SynchronisationController(
            IAsyncQueryHandler<CommitQuery, IEnumerable<SynchronisableCommit>> queryHandler, 
            IAsyncCommandHandler<CommitSynchronisableCommits> commandHandler)
        {
            this.queryHandler = queryHandler;
            this.commandHandler = commandHandler;
        }

        public async Task<IHttpActionResult> GetAsync(string clientId, long fromInTicks)
        {
            return Ok(await queryHandler.Handle(
                new CommitQuery
                {
                    ClientId = clientId, 
                    From = new DateTime(fromInTicks)
                }));
        }

        public async Task<IHttpActionResult> PostAsync(IEnumerable<SynchronisableCommit> toPost)
        {
            await commandHandler.Handle(new CommitSynchronisableCommits { Commits = toPost });
            return Ok();
        }
    }
}