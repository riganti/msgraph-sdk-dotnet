using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Microsoft.Graph
{
    /// <summary>
    /// Contains a single call in a batch request.
    /// </summary>
    public class BatchPart
    {
        // The BatchPart identifier is added when the BatchPart is added to the BatchContainer.
        private int _id;

        public HttpMethod HttpMethod { get; set; }
        public string Url { get; set; }
        public HttpHeaders HttpHeaders { get; set; }
        public object RequestBody { get; set; }
        public BatchPart DependsOn { get; set; }

        /// <summary>
        /// Initialize a batch part based on request method and the full URL.
        /// </summary>
        /// <param name="requestBatchPartMethod">The HttpMethod to use.</param>
        /// <param name="url">The full url to the Microsoft Graph resource including query parameters.</param>
        public BatchPart(HttpMethod requestBatchPartMethod, string url) : this(requestBatchPartMethod, url, null)
        {}

        /// <summary>
        /// Initialize a batch part based on request method and the full URL.
        /// </summary>
        /// <param name="requestBatchPartMethod">The HttpMethod to use.</param>
        /// <param name="url">The full url to the Microsoft Graph resource including query parameters.</param>
        /// <param name="requestHeaders">The collection of request headers to add to the request.</param>
        public BatchPart(HttpMethod requestBatchPartMethod, string url, HttpHeaders requestHeaders)
        {
            HttpMethod = requestBatchPartMethod;
            Url = url;
            HttpHeaders = requestHeaders;
            
            // TODO: if Headers=null, add Content-Type: application/json, else check that it is available.
        }

        public BatchPart(HttpMethod requestBatchPartMethod, string url, HttpHeaders requestHeaders, object requestBody)
        {
            HttpMethod = requestBatchPartMethod;
            Url = url;
            HttpHeaders = requestHeaders;
            RequestBody = requestBody;
            // TODO: if requestBody=null, make sure we aren't doing a POST, PATCH or PUT
        }

        public BatchPart(HttpMethod requestBatchPartMethod, string url, HttpHeaders requestHeaders, BatchPart dependsOnBatchPart)
        {
            HttpMethod = requestBatchPartMethod;
            Url = url;
            HttpHeaders = requestHeaders;
            DependsOn = dependsOnBatchPart;
            // TODO: if dependsOnBatchPart=null, throw nullException
        }

        public BatchPart(HttpMethod requestBatchPartMethod, string url, HttpHeaders requestHeaders, object requestBody, BatchPart dependsOnBatchPart)
        {
            HttpMethod = requestBatchPartMethod;
            Url = url;
            HttpHeaders = requestHeaders;
            RequestBody = requestBody;
            DependsOn = dependsOnBatchPart;

            // TODO: if dependsOnBatchPart=null, throw nullException
            // TODO: if requestBody=null, make sure we aren't doing a POST, PATCH or PUT
        }
        
        private void initialize()
        {
        }
    }
}
