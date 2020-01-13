using System;
using System.IO;
using System.Linq;
using System.Reflection;
using AS4.Factory;
using AS4.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AS4.Tests
{
    [TestClass]
    public class SerializationTests
    {
        private readonly Serializer serializer = new Serializer();

        [TestMethod]
        public void PullRequestSerializesCorrectly()
        {
            var pullRequest = new PullRequestDetails
            {
                Timestamp = new DateTime(2020, 1, 6),
                MessageId = "message-id"
            };

            var message = MessageFactory.Create(pullRequest);

            var actual = serializer.Serialize(message);

            var expected = GetXml("PullRequest.xml");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ReceiptSerializesCorrectly()
        {
            var userMessage = new UserMessageDetails
            {
                Timestamp = new DateTime(2020, 1, 5),
                MessageId = "user-message-id",
                SenderId = "party-1",
                SenderRole = "urn:eu:europa:ec:dgempl:eessi:ir:institution",
                ReceiverId = "party-2",
                ReceiverRole = "urn:eu:europa:ec:dgempl:eessi:ir:institution",
                ConversationId = "conversation-id"
            };

            var receipt = new ReceiptDetails
            {
                Timestamp = new DateTime(2020, 1, 6),
                MessageId = "receipt-message-id",
                UserMessage = userMessage
            };

            var message = MessageFactory.Create(receipt);

            var actual = serializer.Serialize(message);

            var expected = GetXml("Receipt.xml");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ErrorSerializesCorrectly()
        {
            var userMessage = new UserMessageDetails
            {
                Timestamp = new DateTime(2020, 1, 5),
                MessageId = "user-message-id",
                SenderId = "party-1",
                SenderRole = "urn:eu:europa:ec:dgempl:eessi:ir:institution",
                ReceiverId = "party-2",
                ReceiverRole = "urn:eu:europa:ec:dgempl:eessi:ir:institution",
                ConversationId = "conversation-id"
            };

            var error = new ErrorDetails
            {
                Timestamp = new DateTime(2020, 1, 6),
                MessageId = "error-message-id",
                ErrorCode = "EBMS:0004",
                ShortDescription = "Other",
                Description = "error description",
                Details = "error detail",
                UserMessage = userMessage
            };

            var message = MessageFactory.Create(error);

            var actual = serializer.Serialize(message);

            var expected = GetXml("Error.xml");

            Assert.AreEqual(expected, actual);
        }
        
        [TestMethod]
        public void UserMessageSerializesCorrectly()
        {
            var userMessage = new UserMessageDetails
            {
                Timestamp = new DateTime(2020, 1, 6),
                MessageId = "message-id",
                SenderId = "party-1",
                SenderRole = "urn:eu:europa:ec:dgempl:eessi:ir:institution",
                ReceiverId = "party-2",
                ReceiverRole = "urn:eu:europa:ec:dgempl:eessi:ir:institution",
                ConversationId = "conversation-id"
            };

            var message = MessageFactory.Create(userMessage);

            var actual = serializer.Serialize(message);

            var expected = GetXml("UserMessage.xml");

            Assert.AreEqual(expected, actual);
        }

        private string GetXml(string filename)
        {
            var assembly = Assembly.GetExecutingAssembly();

            string resourceName = assembly.GetManifestResourceNames()
                .Single(str => str.EndsWith(filename));

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
