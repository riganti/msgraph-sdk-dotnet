using Microsoft.Graph;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Graph.Test.Requests.Functional
{
    [TestClass]
    public class BatchTests : GraphTestBase
    {
        [TestMethod]
        public async System.Threading.Tasks.Task _TestBatch()
        {
            var batch = new Batch();
            UserBatchPart part1 = graphClient.Me.BatchRequest(batch).Get();
            UserBatchPart part2 = graphClient.Users["admin@M365x462896.onmicrosoft.com"].BatchRequest(batch).Get();

            batch = await batch.PostAsync();

            // Scenarios where all batch parts of the same return type.
            foreach (BatchPart part in batch.BatchItems)
            {
                User myUser = part.Response as User;
            }

            // Scenario for accessing a single batch part 
            User user = part1.Response;
        }
    }
}