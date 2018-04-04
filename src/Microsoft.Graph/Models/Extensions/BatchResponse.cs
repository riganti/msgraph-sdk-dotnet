using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Microsoft.Graph
{
    public class BatchResponse
    {
        public HttpHeaders HttpHeaders { get; private set; }
        public HttpStatusCode HttpStatusCode { get; private set; }
        

        public BatchResponse() { }

        public BatchResponse(IEnumerable<IBatchPart> batchParts)
        {
            //TODO: process each batch part by giving it an ID, adding to an enumerable container, set dependson relationship.
            //BatchParts.AddRange(batchParts);

        }

        // We don't want customers setting batch parts. We need to make sure they have an ID.
        public List<IBatchPart> BatchParts { get; private set; }
        
    }
}