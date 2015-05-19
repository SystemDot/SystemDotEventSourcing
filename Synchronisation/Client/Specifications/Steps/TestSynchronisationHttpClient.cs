using System.Linq;
using System.Net;
using System.Net.Http;
using SystemDot.EventSourcing.Synchronisation.Client.Retrieval;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemDot.EventSourcing.Synchronisation;

namespace SystemDot.Domain.Synchronisation.Client.Specifications.Steps
{
    using SystemDot.EventSourcing.Synchronisation.Client.Http;

    public class TestSynchronisationHttpClient : ISynchronisationHttpClient
    {
        readonly List<SynchronisableCommit> commits = new List<SynchronisableCommit>();
        
        public HttpStatusCode StatusToReturn { private get; set; }
        public Uri LastAsyncCallServerUri { get; private set; }
        public long LastGetCommitsAsyncCallFrom { get; private set; }
        public CommitSynchronisation LastPostCommitsAsyncCallContent { get; private set; }

        public TestSynchronisationHttpClient()
        {
            StatusToReturn = HttpStatusCode.OK;
        }

        public Task<HttpResponseMessage> GetCommitsAsync(Uri serverUri, string clientId, long @fromCommitInTicks)
        {
            LastAsyncCallServerUri = serverUri;
            LastGetCommitsAsyncCallFrom = @fromCommitInTicks;

            var response = new HttpResponseMessage
            {
                StatusCode = StatusToReturn
            };

            if (response.IsSuccessStatusCode)
            {
                response.Content = new StringContent(JsonConvert.SerializeObject(new CommitSynchronisation {Commits = commits.Where(c => c.StreamId.ClientId == clientId).ToList()}));
            }

            return Task.FromResult(response);
        }

        public Task<HttpResponseMessage> PostCommitsAsync(Uri serverUri, CommitSynchronisation content)
        {
            LastAsyncCallServerUri = serverUri;
            LastPostCommitsAsyncCallContent = content;
            var response = new HttpResponseMessage
            {
                StatusCode = StatusToReturn
            };

            return Task.FromResult(response);
        }

        public void Add(SynchronisableCommit toAdd)
        {
            commits.Add(toAdd);
        }

    }
}