using System.Collections.Generic;
using System.Web.Http;

namespace SystemDot.EventSourcing.Synchronisation.Testing
{
    public class SynchronisationController : ApiController
    {
        static readonly List<SynchronisableCommit> Commits = new List<SynchronisableCommit>();

        public IEnumerable<SynchronisableCommit> Get()
        {
            return Commits;
        }

        public void Post([FromBody]SynchronisableCommit toPost)
        {
            Commits.Add(toPost);
        }
    }
}