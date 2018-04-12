using Microsoft.Graph;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Graph
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class BatchPart
    {
        //Currently the requests are being serialized into a sub-array since they are a separate object
        //Need to write a custom JSON.NET rule to parse this correctly.
        [JsonIgnore]
        public BatchRequest Request { get; set; }

        // Doing this since we can't use reflection in netStandard 1.1. Otherwise we could create a converter for BatchRequest.
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "body", Required = Newtonsoft.Json.Required.Default)]
        public object Body
        {
            get
            {
                return Request.Body;
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "url", Required = Newtonsoft.Json.Required.Default)]
        public string Url
        {
            get
            {
                return Request.Url;
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "method", Required = Newtonsoft.Json.Required.Default)]
        public string Method
        {
            get
            {
                return Request.Method;
            }
        }

        [JsonIgnore]
        public object Response { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "id", Required = Newtonsoft.Json.Required.Default)]
        public int Id { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "dependsOn", Required = Newtonsoft.Json.Required.Default)]
        public int[] DependsOn { get; set; }

        [JsonIgnore]
        public IBaseClient Client { get; set; }

        public BatchPart(BatchRequest batchRequest, int id, string url, string method, object body = null, int[] dependsOn = null)
        {
            this.Id = id;
            this.Request = batchRequest;
            this.Request.Url = url;
            this.Request.Method = method;
            this.Request.Body = body;
        }
    }
}
