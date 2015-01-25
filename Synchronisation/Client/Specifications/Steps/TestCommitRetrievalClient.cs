using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemDot.EventSourcing.Synchronisation;
using SystemDot.EventSourcing.Synchronisation.Client;

namespace SystemDot.Domain.Synchronisation.Client.Specifications.Steps
{
    public class TestCommitRetrievalClient : ICommitRetrievalClient
    {
        readonly List<SynchronisableCommit> commits = new List<SynchronisableCommit>();
        
        public HttpStatusCode StatusToReturn { get; set; }
        public Uri LastGetCommitsAsyncCallServerUri { get; private set; }
        public long LastGetCommitsAsyncCallFrom { get; private set; }

        public TestCommitRetrievalClient()
        {
            StatusToReturn = HttpStatusCode.OK;
        }

        public Task<HttpResponseMessage> GetCommitsAsync(Uri serverUri, long @fromCommitInTicks)
        {
            LastGetCommitsAsyncCallServerUri = serverUri;
            LastGetCommitsAsyncCallFrom = @fromCommitInTicks;

            var response = new HttpResponseMessage
            {
                StatusCode = StatusToReturn
            };

            if (response.IsSuccessStatusCode)
            {
                response.Content = new StringContent(JsonConvert.SerializeObject(commits));
            }

            return Task.FromResult(response);
        }
        
        public void Add(SynchronisableCommit toAdd)
        {
            commits.Add(toAdd);
        }

    }
}