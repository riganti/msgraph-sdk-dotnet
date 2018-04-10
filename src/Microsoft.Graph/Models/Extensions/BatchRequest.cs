using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Microsoft.Graph
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class BatchRequest
    {
        private int _idCounter = 0;

        //private HttpMethod _batchHttpMethod = HttpMethod.Post;
        private const string _batchUrlSegment = "/$batch";
        private const string _acceptType = "application/json";
        private const string _contentType = "application/json";

        public HttpMethod HttpMethod { get; private set; }

        public HttpHeaders HttpHeaders { get; private set; }

        internal string BaseUrl { get; set; }

        // We don't want customers setting batch parts. We need to make sure they have an ID.
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "requests", Required = Newtonsoft.Json.Required.Default)]
        public IEnumerable<IBatchPart> BatchParts { get; set; }

        public BatchRequest() { }

        public BatchRequest(List<IBatchPart> batchParts) : this(batchParts, null)
        {
            //TODO: process each batch part by giving it an ID, adding to an enumerable container, set dependson relationship.
            //BatchParts.AddRange(batchParts);
            //BatchRequest(batchParts, null);

        }

        public BatchRequest(List<IBatchPart> batchParts, HttpHeaders headers)
        {
            HttpHeaders = headers;
            HttpMethod = HttpMethod.Post;
            //BatchParts = new List<IBatchPart>();

            int batchId = 1;

            // Set the BatchPart id.
            if (batchParts.Count > 0)
            {

                foreach (IBatchPart part in batchParts)
                {
                    part.Id = batchId;
                    ++batchId;

                }
            }
            else
            {
                throw new Exception("You cannot send an empty batch.");
            }

            BatchParts = batchParts;



        }

        public BatchRequest(HttpHeaders headers)
        {
            HttpHeaders = headers;
        }





        /// <summary>
        /// Add the BatchPart to the BatchContainer and set the BatchPart.Id.
        /// </summary>
        /// <param name="batchPart">The BatchPart to add to the BatchContainer.</param>
        public void AddToBatch(IBatchPart batchPart)
        {
            // TODO: We need to trim off the baseUrl from the BatchPart. We can do this whe

            //batchPart.Id = _idCounter;
            //++_idCounter;
            //BatchParts.Add(batchPart);
        }
    }
}