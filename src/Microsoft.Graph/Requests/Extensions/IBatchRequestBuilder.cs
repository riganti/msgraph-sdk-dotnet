using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Graph
{
    public interface IBatchRequestBuilder : IBaseRequestBuilder, IBaseRequest
    {
        System.Threading.Tasks.Task<BatchResponse> PostBatchAsync(BatchRequest batchRequest);
    }
}
