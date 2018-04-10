using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using Async = System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Linq;

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
                #region Test contacts
                // Contact to delete.
                var contactToDelete = new Contact();
                contactToDelete.GivenName = "_TomDel" + Guid.NewGuid().ToString();
                var deletedContact = await graphClient.Me.Contacts.Request().AddAsync(contactToDelete);

                // Contact to create
                var contact = new Contact();
                contact.GivenName = "_Tom" + Guid.NewGuid().ToString();
                #endregion

                // BatchPart to add a new contact and then get the results. Has both request and response bodies.
                // The following signature can be generated.
                RequestBatchPart<Contact> postNewContactBatchPart = graphClient.Me.Contacts.Request().BatchPartAdd(contact);

                // BatchPart to get a user. No Requestbody scenario. Adding dependsOn.
                RequestBatchPart<User> getUserBatchPart = graphClient.Me.Request().BatchPartGet();

                // BatchPart to delete a contact. No RequestBody/ResponseBody scenario.
                //BatchPart deleteContactBatchPart = graphClient.Me.Contacts[deletedContact.Id].Request().BatchPartDelete();

                // We assume that customers will perform batch operations on the same type of data often. This is 
                // less useful for workflow scenarios. What this does give us is a way to enumerate similar
                // operations. Operations are considered similar if they return the same entity.
                List<RequestBatchPart<Contact>> contactContactBatchParts = new List<RequestBatchPart<Contact>>();
                contactContactBatchParts.Add(postNewContactBatchPart);

                // Problem is that we can't automatically serialize since BatchParts can contain different return types.
                // Each different generic form of BatchPart is a different class. Plus, first we need to inspect for errors. 
                List<IBatchPart> batchParts = new List<IBatchPart>(contactContactBatchParts);
                batchParts.Add(getUserBatchPart);

                // Add each batch part to the BatchContainer. We are now ready to send the Batch.
                BatchRequest batchRequest = new BatchRequest(batchParts);

                // Send the Batch request and get the response. At this point, you'll know
                // whether the call was a success, get response headers for the entire call,
                // and a JSON response body. If there was anything but a 2xx or 3xx, then 
                // a ServiceException is thrown with the error response as the inner information. 
                BatchResponse batchResponse;

                try
                {
                    batchResponse = await graphClient.Batch.PostBatchAsync(batchRequest);
                    // At this point, you can get the overall response status and headers.
                    // You also can get the raw response. This will be used to deserialize
                    // the response bodies. You can also implement a custom deserializer.
                }
                catch (ServiceException)
                {
                    throw;
                }

                // Get information about the results of the BatchPart. We can loop if the BatchParts 
                // are of the same generic type
                postNewContactBatchPart.LoadBatchPartResponse(batchResponse.ResponseBody);
                Contact myContact = postNewContactBatchPart.ResponseBody;
                var contactBatchPartResponseHeaders = postNewContactBatchPart.ResponseHeaders;
                var contactBatchPartResponseHttpStatusCode = postNewContactBatchPart.ResponseHttpStatusCode;

                getUserBatchPart.LoadBatchPartResponse(batchResponse.ResponseBody);
                User myUser = getUserBatchPart.ResponseBody;
                var userBatchPartResponseHeaders = getUserBatchPart.ResponseHeaders;
                var userBatchPartResponseHttpStatusCode = getUserBatchPart.ResponseHttpStatusCode;
            }
            catch (Microsoft.Graph.ServiceException e)
            {
                Assert.Fail("Something happened, check out a trace. Error code: {0}", e.Error.Code);
            }
        }
    }
}