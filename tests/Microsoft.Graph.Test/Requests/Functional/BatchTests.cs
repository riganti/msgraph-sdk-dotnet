using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using Async = System.Threading.Tasks;


namespace Microsoft.Graph.Test.Requests.Functional
{
    //[Ignore]
    [TestClass]
    public class BatchTests : GraphTestBase
    {
        [TestMethod]
        public async Async.Task BatchingSample()
        {
            try
            {
                // Contact to delete.
                var contactToDelete = new Contact();
                contactToDelete.GivenName = "_TomDel" + Guid.NewGuid().ToString();
                var deletedContact = await graphClient.Me.Contacts.Request().AddAsync(contactToDelete);

                // Contact to create
                var contact = new Contact();
                contact.GivenName = "_Tom" + Guid.NewGuid().ToString();

                // BatchPart to add a new contact and then get the results. Has both request and response bodies.
                BatchPart<Contact, Contact> postNewContactBatchPart = graphClient.Me.Contacts.Request().BatchPartAdd(contact);
                Contact requestContact = postNewContactBatchPart.RequestBody;
                //Contact responseContact = postNewContactBatchPart.ResponseBody; // This is null until after we get a response.

                // BatchPart to get a user. No Requestbody scenario. Adding dependsOn.
                //BatchPart<User> getUserBatchPart = graphClient.Me.Request().BatchPartGet(postNewContactBatchPart);

                // BatchPart to delete a contact. No RequestBody/ResponseBody scenario.
                //BatchPart deleteContactBatchPart = graphClient.Me.Contacts[deletedContact.Id].Request().BatchPartDelete();

                List<IBatchPart> batchParts = new List<IBatchPart>();
                batchParts.Add(postNewContactBatchPart);
                //batchParts.Add(getUserBatchPart);
                //batchParts.Add(deleteContactBatchPart);

                // Add each batch part to the BatchContainer. We are now ready to send the Batch.
                BatchRequest batchRequest = new BatchRequest(batchParts);
                //batchRequest.Add(postNewContactBatchPart);
                //batchRequest.Add(getUserBatchPart);
                //batchRequest.Add(deleteContactBatchPart);

                // Let's make sure we can correlate request with the response. Customers need to know
                // which part of the batch failed.
                BatchResponse batchResponse = await graphClient.Batch.PostBatchAsync(batchRequest); // Should return a Task.

            }
            catch (Microsoft.Graph.ServiceException e)
            {
                Assert.Fail("Something happened, check out a trace. Error code: {0}", e.Error.Code);
            }
        }
    }
}
