using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Microsoft.Graph
{ 
    public class BatchRequest
    {
        private int _idCounter = 0;

        private HttpMethod _batchHttpMethod = HttpMethod.Post;
        private const string _batchUrlSegment = "/$batch";
        private const string _acceptType = "application/json";
        private const string _contentType = "application/json";

        public HttpHeaders HttpHeaders { get; private set; }

        internal string BaseUrl { get; set; }


        public BatchRequest() { }

        public BatchRequest(IEnumerable<IBatchPart> batchParts)
        {
            //TODO: process each batch part by giving it an ID, adding to an enumerable container, set dependson relationship.
            //BatchParts.AddRange(batchParts);
            
        }

        public BatchRequest(IEnumerable<IBatchPart> batchParts, HttpHeaders headers)
        {
            HttpHeaders = headers;
        }

        public BatchRequest(HttpHeaders headers)
        {
            HttpHeaders = headers;
        }



        // We don't want customers setting batch parts. We need to make sure they have an ID.
        public List<IBatchPart> BatchParts { get; private set; }

        /// <summary>
        /// Add the BatchPart to the BatchContainer and set the BatchPart.Id.
        /// </summary>
        /// <param name="batchPart">The BatchPart to add to the BatchContainer.</param>
        public void Add(IBatchPart batchPart)
        {
            // TODO: We need to trim off the baseUrl from the BatchPart. We can do this whe

            batchPart.Id = _idCounter;
            ++_idCounter;
            BatchParts.Add(batchPart);
        }
    }
}