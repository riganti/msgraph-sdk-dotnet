using Newtonsoft.Json.Linq;
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



            // First partially deserialize the response into a BatchResponse.
            // Actually, I don't think we'll do this. I think we'll new up this object.
            // Although this does get the entire body deserialized into additional properties.
            // We may want to enable this with a flag.
            //BatchResponse batchResponse = this.Client.HttpProvider.Serializer
            //                        .DeserializeObject<BatchResponse>(await response.Content.ReadAsStringAsync());
            BatchResponse batchResponse = new BatchResponse();

            // Set information in the response envelope.
            batchResponse.HttpStatusCode = response.StatusCode;
            batchResponse.HttpHeaders = response.Headers;

            // TODO: If it is an error response, go to the short circuit code  path
            // to deserialize the error.


            string responseBody = await response.Content.ReadAsStringAsync();
            // https://www.newtonsoft.com/json/help/html/SerializingJSONFragments.htm
            JObject responseBodyObj = JObject.Parse(responseBody);

            // get JSON result objects into a list
            JEnumerable<JToken> results = responseBodyObj["responses"].Children();

            // TODO: inspect each response batch part. First inspect the status code
            // and determine whether we need to deserialize into an error.
            // If it is a success, determine how to deserialize
            // by the part id which will correlate back to the collection from the 
            // request.

            

            //hrm.Content = new 
            //this.Client.HttpProvider.

            throw new NotImplementedException();
        }
    }
}
