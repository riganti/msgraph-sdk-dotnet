using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Linq;

// Do I need an interface for BatchPart?
namespace Microsoft.Graph
{
    /// <summary>
    /// Generic batch part object that contains a single call in a batch request. 
    /// We know what the request body and response body will be at code generation
    /// time. So we can define a generic BatchPart object
    /// 
    /// Used for batch parts that have both a request body and response body.
    /// </summary>
    /// <typeparam name="TRequestBody">The type of the request body of the batch part.</typeparam>
    /// <typeparam name="TResponseBody">The type of the response body of the batch part.</typeparam>
    public class RequestBatchPart<TRequestBody, TResponseBody> : BatchPartBase, IRequestBatchPart
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "body", Required = Newtonsoft.Json.Required.Default)]
        public TRequestBody RequestBody { get; set; }
        public ResponseBatchPart<TResponseBody> ResponseBody { get; set; }

        private bool _isBatchPartSuccess;

        /// <summary>
        /// Constructor to be used in template to generate a batch part request that  sends
        /// a request body and receives a response body.
        /// </summary>
        /// <param name="requestBatchPartMethod"></param>
        /// <param name="url"></param>
        /// <param name="requestBody"></param>
        public RequestBatchPart(HttpMethod requestBatchPartMethod, string url, TRequestBody requestBody)
        {
            HttpMethod = requestBatchPartMethod.ToString();
            Url = url;
            RequestBody = requestBody;
            HttpHeaders = new Dictionary<String, String>() { { "Content-Type", "application/json" } };
        }

        public void LoadBatchPartResponse(string batchResponseBody)
        {
            /***
             * TODO
             * 1 - GetHttpStatusCode and add to this.ResponseHttpStatusCode
             * 2 - GetResponseHeaders and add to this.ResponseHeaders
             * 3 - CHeck status. If error, add info to this.ErrorResponse.
             * 4 - Deserialize the response body into this.ResponseBody where TResponseBody, not ResponseBatchPart.
             * 
             * **/
        }

        public ResponseBatchPart<TResponseBody> GetResponseBatchPart<TResponseBody>(string batchResponseBody)
        {
            JObject responseCorpus = JObject.Parse(batchResponseBody);
            string statusCode = GetHttpStatusCode(responseCorpus);
            // TODO: Handle error condition. Return a null contact and set the ErrorResponse.
            string httpResponseHeaders = GetHttpResponseHeaders(responseCorpus);
            DeserializeBatchPartBody(responseCorpus);
            // TODO: Set the ResponseBody.

            //return ResponseBody;
            return new ResponseBatchPart<TResponseBody>(batchResponseBody, this.Id);

        }

        private void DeserializeBatchPartBody(JObject responseCorpus)
        {
            // TODO: Deserialize the ResponseBatchPart body.
            throw new NotImplementedException("DeserializeBatchPartBody is not implemented.");
        }

        private string GetHttpStatusCode(JObject responseCorpus)
        {
            
            JToken statusCodeToken = responseCorpus.SelectTokens("responses[0]")
                                                   .Where(s => (int)s["id"] == this.Id)
                                                   .Select(i => i["status"])
                                                   .First();
            string statusCode = statusCodeToken.Value<string>();
            // isSuccess determined if the response is a 2xx or 3xx
            //if (statusCode.StartsWith("2") || statusCode.StartsWith("3"))
            //{
            //    _isBatchPartSuccess = true;
            //}
            //else
            //{
            //    _isBatchPartSuccess = false;
            //}
            return statusCode;
        }

        private string GetHttpResponseHeaders(JObject responseCorpus)
        {
            IEnumerable<JToken> responseHeaderTokens = responseCorpus.SelectTokens("responses[0]")
                                                                     .Where(s => (int)s["id"] == this.Id)
                                                                     .Select(i => i["headers"]);

            return "";
        }
    }

    /// <summary>
    /// Used for batch parts that have only a request body or response body.
    /// </summary>
    /// <typeparam name="TBody"></typeparam>
    public class ResponseBatchPart<TBody> : BatchPartBase, IBatchPart
    {
        // This could cause confusion since one of these properties will always be null on a given object.
       // [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "body", Required = Newtonsoft.Json.Required.Default)]

        public TBody ResponseBody { get; private set; }
        public HttpResponseHeaders HttpResponseHeaders { get; private set; }
        public string HttpStatusCode { get; private set; }

        public ErrorResponse ErrorResponse { get; private set; }

        /// <summary>
        /// Constructor to be used in template to generate a batch part request that only sends
        /// a request body and receives no response body.
        /// </summary>
        public ResponseBatchPart(string responseBody, int requestId)
        {
            /** TODO
              1. Identify the response based on requestId.
              2. Extract the HttpStatusCode and figure whether we are deserializing TBody, or an ErrorResponse.
              3. Deserialize the TBody or ErrorResponse.
              4. Extract the HttpResponseHeaders.
             
             
             */


        }

        /// <summary>
        /// Constructor to be used in template to generate a batch part request that doesn't send
        /// a request body and receives a response body.
        /// </summary>
        /// <param name="requestBatchPartMethod"></param>
        /// <param name="url"></param>
        /// <param name="dependsOn"></param>
        public ResponseBatchPart(HttpMethod requestBatchPartMethod, string url, IBatchPart dependsOn)
        {
            HttpMethod = requestBatchPartMethod.ToString();
            Url = url.Replace("https://graph.microsoft.com/v1.0/", ""); // Hack, we'll need to figure how get the batch URL
            SetDependsOn(dependsOn);

        }

        
    }

    /// <summary>
    /// Used for batch parts that have only a request body or response body.
    /// </summary>
    /// <typeparam name="TBody"></typeparam>
    public class BatchPart<TBody> : BatchPartBase, IBatchPart
    {
        // This could cause confusion since one of these properties will always be null on a given object.
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "body", Required = Newtonsoft.Json.Required.Default)]

        public TBody ResponseBody { get; set; }

        /// <summary>
        /// Constructor to be used in template to generate a batch part request that only sends
        /// a request body and receives no response body.
        /// </summary>
        /// <param name="requestBatchPartMethod"></param>
        /// <param name="url"></param>
        /// <param name="requestBody"></param>
        public BatchPart(HttpMethod requestBatchPartMethod, string url, TBody responseBody)
        {
            HttpMethod = requestBatchPartMethod.ToString();
            Url = url;
            ResponseBody = responseBody;
            HttpHeaders = new Dictionary<String, String>() { { "Content-Type", "application/json" } };

        }

        /// <summary>
        /// Constructor to be used in template to generate a batch part request that doesn't send
        /// a request body and receives a response body.
        /// </summary>
        /// <param name="requestBatchPartMethod"></param>
        /// <param name="url"></param>
        /// <param name="dependsOn"></param>
        public BatchPart(HttpMethod requestBatchPartMethod, string url, IBatchPart dependsOn)
        {
            HttpMethod = requestBatchPartMethod.ToString();
            Url = url.Replace("https://graph.microsoft.com/v1.0/", ""); // Hack, we'll need to figure how get the batch URL
            SetDependsOn(dependsOn);

        }
    }

    /// <summary>
    /// Used for batch parts that have no request or response body.
    /// </summary>
    public class BatchPart : BatchPartBase
    {
        /// <summary>
        /// Constructor to be used in template to generate a batch part request that doesn't send
        /// a request body and receives a response body.
        /// </summary>
        /// <param name="requestBatchPartMethod"></param>
        /// <param name="url"></param>
        /// <param name="dependsOn"></param>
        public BatchPart(HttpMethod requestBatchPartMethod, string url, IBatchPart dependsOn)
        {
            HttpMethod = requestBatchPartMethod.ToString();
            Url = url;
            SetDependsOn(dependsOn);

        }
    }

    public class BatchPartBase 
    {
        /// <summary>
        /// The BatchPart identifier is added when the BatchPart is added to the BatchContainer. 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "id", Required = Newtonsoft.Json.Required.Default)]
        public int Id { get; set; }
        /// <summary>
        /// Must use an interface as we don't know whether the the BatchPart will contain the 
        /// both a request and response body.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "dependson", Required = Newtonsoft.Json.Required.Default)]
        public int[] DependsOn { get; set; }

        private string _httpMethod;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "method", Required = Newtonsoft.Json.Required.Default)]
        public string HttpMethod
        {
            get { return _httpMethod; }
            set {
                _httpMethod = value;
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "url", Required = Newtonsoft.Json.Required.Default)]
        public string Url { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "headers", Required = Newtonsoft.Json.Required.Default)]
        public object HttpHeaders { get; set; }

        protected void SetDependsOn(IBatchPart part)
        {
            
            DependsOn.SetValue(part.Id, 0);
        }
    }

    /// <summary>
    /// IBatchPart interface is used to get a handle on the Id of BatchPart that
    /// is depended on by another BatchPart.
    /// </summary>
    public interface IBatchPart
    {
        //TRequestBody RequestBody { get; set; }
        //BatchPart<TResponseBody> ResponseBody { get; set; }
        int Id { get; set; }
        int[] DependsOn { get; set; }
        string HttpMethod { get; set; }
        string Url { get; set; }
        object HttpHeaders { get; set; }


    }

    public interface IRequestBatchPart : IBatchPart
    {
        ResponseBatchPart<TResponseBody> GetResponseBatchPart<TResponseBody>(string batchResponseBody);
    }
}
