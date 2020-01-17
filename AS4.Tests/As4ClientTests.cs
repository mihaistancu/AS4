using AS4.Tests.Factories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using AS4.Tests.Asserts;

namespace AS4.Tests
{
    [TestClass]
    public class As4ClientTests
    {
        private const string Url = "http://localhost:8080";

        [TestMethod]
        public void SendUserMessageGetBackReceipt()
        {
            var request = Messages.Create(Envelopes.UserMessage, Attachments.Generate());
            var response = Messages.Create(Envelopes.Receipt);
            CheckSendAndReceive(request, response);
        }

        [TestMethod]
        public void SendPullRequestGetBackUserMessage()
        {
            var request = Messages.Create(Envelopes.PullRequest);
            var response = Messages.Create(Envelopes.UserMessage, Attachments.Generate());
            CheckSendAndReceive(request, response);
        }

        [TestMethod]
        public void SendErrorGetBackNothing()
        {
            var request = Messages.Create(Envelopes.PullRequest);
            CheckSendAndReceive(request);
        }

        private void CheckSendAndReceive(As4Message expectedRequest, As4Message expectedResponse)
        {
            Func<As4Message, As4Message> handler = actualRequest =>
            {
                MessageAssert.AreEqual(expectedRequest, actualRequest);
                return expectedResponse;
            };

            using (Servers.Create(Url, handler))
            {
                var client = new As4Client();
                var actualResponse = client.Send(new Uri(Url), expectedRequest);
                MessageAssert.AreEqual(expectedResponse, actualResponse);
            }
        }

        private void CheckSendAndReceive(As4Message expectedRequest)
        {
            Action<As4Message> handler = actualMessage =>
            {
                MessageAssert.AreEqual(expectedRequest, actualMessage);
            };

            using (Servers.Create(Url, handler))
            {
                var client = new As4Client();
                var result = client.Send(new Uri(Url), expectedRequest);
                Assert.IsNull(result);
            }
        }
    }
}