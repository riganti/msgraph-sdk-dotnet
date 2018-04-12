using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Graph
{
    public class BatchRequest
    {
        public object Body { get; set; }

        public string Url { get; set; }

        public string Method { get; set; }

        public BatchRequest()
        {

        }
    }
}
