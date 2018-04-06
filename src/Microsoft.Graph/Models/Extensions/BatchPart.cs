using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

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
    public class BatchPart<TRequestBody, TResponseBody> : BatchPartBase
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "body", Required = Newtonsoft.Json.Required.Default)]
        public TRequestBody RequestBody { get; set; }
        public TResponseBody ResponseBody { private get; set; }

        /// <summary>
        /// Constructor to be used in template to generate a batch part request that  sends
        /// a request body and receives a response body.
        /// </summary>
        /// <param name="requestBatchPartMethod"></param>
        /// <param name="url"></param>
        /// <param name="requestBody"></param>
        public BatchPart(HttpMethod requestBatchPartMethod, string url, TRequestBody requestBody)
        {
            HttpMethod = requestBatchPartMethod.ToString();
            Url = url;
            RequestBody = requestBody;
            HttpHeaders = new Dictionary<String, String>() { { "Content-Type", "application/json" } };
        }
    }

    /// <summary>
    /// Used for batch parts that have only a request body or response body.
    /// </summary>
    /// <typeparam name="TBody"></typeparam>
    public class BatchPart<TBody> : BatchPartBase
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "body", Required = Newtonsoft.Json.Required.Default)]
        // This could cause confusion since one of these properties will always be null on a given object.
        public TBody RequestBody { get; set; }
        public TBody ResponseBody { get; set; }

        /// <summary>
        /// Constructor to be used in template to generate a batch part request that only sends
        /// a request body and receives no response body.
        /// </summary>
        /// <param name="requestBatchPartMethod"></param>
        /// <param name="url"></param>
        /// <param name="requestBody"></param>
        public BatchPart(HttpMethod requestBatchPartMethod, string url, TBody requestBody)
        {
            HttpMethod = requestBatchPartMethod.ToString();
            Url = url;
            RequestBody = requestBody;
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
    
    public class BatchPartBase : IBatchPart
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
        int Id { get; set; }
        int[] DependsOn { get; set; }
        string HttpMethod { get; set; }
        string Url { get; set; }
        object HttpHeaders { get; set; }
    }
}
