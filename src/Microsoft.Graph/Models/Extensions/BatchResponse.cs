using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Microsoft.Graph
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class BatchResponse
    {
        public HttpResponseHeaders HttpHeaders { get; internal set; }
        public HttpStatusCode HttpStatusCode { get; internal set; }
        

        public BatchResponse() { }

        public BatchResponse(IEnumerable<IBatchPart> batchParts)
        {
            //TODO: process each batch part by giving it an ID, adding to an enumerable container, set dependson relationship.
            //BatchParts.AddRange(batchParts);

        }

        // We don't want customers setting batch parts. We need to make sure they have an ID.
       // [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "responses", Required = Newtonsoft.Json.Required.Default)]

        public List<IBatchPart> BatchParts { get; private set; }

        /// <summary>
        /// Gets or sets additional data.
        /// </summary>
        [JsonExtensionData(ReadData = true, WriteData = true)]
        public IDictionary<string, object> AdditionalData { get; set; }
    }
}