using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Async = System.Threading.Tasks;

namespace Microsoft.Graph
{
     public class BatchRequestBuilder : IBatchRequestBuilder
    {
        public BatchRequestBuilder(string requestUrl)
        {

        }

        public Async.Task<BatchResponse> PostBatchAsync(BatchRequest batchRequest)
        {
            return this.PostBatchAsync(batchRequest, CancellationToken.None);
        }

        private Async.Task<BatchResponse> PostBatchAsync(BatchRequest batchRequest, CancellationToken none)
        {
            throw new NotImplementedException();
        }
    }
}
