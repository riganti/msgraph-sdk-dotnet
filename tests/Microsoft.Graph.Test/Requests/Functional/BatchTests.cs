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

                // We assume that customers will perform batch operations on the same type of data often. This is 
                // less useful for workflow scenarios. What this does give us is a way to enumerate similar
                // operations.
                List<BatchPart<Contact,Contact>> contactContactBatchParts = new List<BatchPart<Contact, Contact>>();
                contactContactBatchParts.Add(postNewContactBatchPart);

                // Problem is that we can't automatically serialize since BatchParts can contain different return types.
                // Each different generic form of BatchPart is a different class.
                List<IBatchPart> batchParts = new List<IBatchPart>(contactContactBatchParts);

                // Add each batch part to the BatchContainer. We are now ready to send the Batch.
                BatchRequest batchRequest = new BatchRequest(batchParts);
                //batchRequest.Add(postNewContactBatchPart);
                //batchRequest.Add(getUserBatchPart);
                //batchRequest.Add(deleteContactBatchPart);

                // Send the Batch request and get the response. At this point, you'll know
                // whether the call was a success, get response headers for the entire call,
                // and a JSON response body. 
                BatchResponse batchResponse = await graphClient.Batch.PostBatchAsync(batchRequest);

                // for each type of batchPart, we deserialize. Customer will need to manage this.
                // The nice thing is that the response parts only get deserialized when they're needed.
                // This is a bit janky since we are referencing the BatchParts from the request. We need
                // to correlate the ids so we can provide the deserialization hints.
                foreach (BatchPart<Contact, Contact> requestBatchPart in batchParts)
                {
                    // We are going to need to be prescriptive for automated batchpart processing. 

                    // TODO: 1. Check if batch part is successful. 
                    Func<string, int, bool> isSuccess = (responseBody, batchPartId) => {


                        var responseCorpus = JObject.Parse(responseBody);
                        JToken statusCodeToken = responseCorpus.SelectTokens("responses[0]").Where(s => (int)s["id"] == batchPartId).Select(i => i["status"]).First();
                        string statusCode = statusCodeToken.Value<string>();

                        // isSuccess determined if the response is a 2xx or 3xx
                        if (statusCode.StartsWith("2") || statusCode.StartsWith("3"))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    };

                    if (isSuccess(batchResponse.ResponseBody, requestBatchPart.Id))
                    {
                        ResponseBatchPart<Contact> responseBatchPart1 = requestBatchPart.GetResponseBatchPart<Contact>(batchResponse.ResponseBody);
                        HttpResponseHeaders batchPartResponseHeaders = responseBatchPart1.HttpResponseHeaders;
                        HttpStatusCode batchPartResponseStatus = responseBatchPart1.HttpStatusCode;
                        Contact batchPartResponseContact = responseBatchPart1.ResponseBody;

                    }
                    else // We need to create an error BatchResponsePart.
                    {
                        ResponseBatchPart<ErrorResponse> responseBatchPart1Error = new ResponseBatchPart<ErrorResponse>(batchResponse.ResponseBody, requestBatchPart.Id);
                    }

                    // This would be an option if we wanted to load the response on to the request batch part.
                    //requestBatchPart.LoadResponseBody(batchResponse.ResponseBody);
                }
            }
            catch (Microsoft.Graph.ServiceException e)
            {
                Assert.Fail("Something happened, check out a trace. Error code: {0}", e.Error.Code);
            }
        }
    }
}
