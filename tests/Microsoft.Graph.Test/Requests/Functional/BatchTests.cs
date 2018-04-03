using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using Async = System.Threading.Tasks;


namespace Microsoft.Graph.Test.Requests.Functional
{
    [Ignore]
    [TestClass]
    public class BatchTests : GraphTestBase
    {
        [TestMethod]
        public async Async.Task BatchingSample()
        {
            try
            {
                var contact = new Contact();
                contact.GivenName = "_Tom" + Guid.NewGuid().ToString();

                //BatchPart postNewContactBatchPart = graphClient.Me.Contacts.Request().BatchPartAdd(contact, new BatchPart);


            }
            catch (Microsoft.Graph.ServiceException e)
            {
                Assert.Fail("Something happened, check out a trace. Error code: {0}", e.Error.Code);
            }
        }
    }
}
