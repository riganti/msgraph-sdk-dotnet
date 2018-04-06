using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using Async = System.Threading.Tasks;

namespace Microsoft.Graph
{
     public class BatchRequestBuilder : BaseRequestBuilder, IBatchRequestBuilder
    {
        private string _requestUrl = "https://graph.microsoft.com/v1.0/$batch";

        public BatchRequestBuilder(string requestUrl, GraphServiceClient client) : base(requestUrl, client)
        {

        }

        public Async.Task<BatchResponse> PostBatchAsync(BatchRequest batchRequest)
        {
            return this.PostBatchAsync(batchRequest, CancellationToken.None);
        }

        /// <summary>
        /// Most of the code here will need to be refactored out to a base request or batch request class.
        /// </summary>
        /// <param name="batchRequest"></param>
        /// <param name="none"></param>
        /// <returns></returns>
        private async Async.Task<BatchResponse> PostBatchAsync(BatchRequest batchRequest, CancellationToken none)
        {
            // Batch is always POST, is that a safe assumption?

            HttpRequestMessage hrm = new HttpRequestMessage(batchRequest.HttpMethod, _requestUrl);
            hrm.Content = new StringContent(this.Client.HttpProvider.Serializer.SerializeObject(batchRequest));

            hrm.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");


            HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead;
            CancellationToken cancellationToken = CancellationToken.None;

            await this.Client.AuthenticationProvider.AuthenticateRequestAsync(hrm);

            HttpResponseMessage response = await this.Client.HttpProvider.SendAsync(hrm, completionOption, cancellationToken).ConfigureAwait(false);



            //hrm.Content = new 
            //this.Client.HttpProvider.

            throw new NotImplementedException();
        }
    }
}
