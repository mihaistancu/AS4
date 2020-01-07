using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using AS4.Soap;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AS4.Tests
{
    [TestClass]
    public class SerializationTests
    {
        [TestMethod]
        public void PullRequestSerializesCorrectly()
        {
            var pullRequest = new PullRequestDetails
            {
                Timestamp = new DateTime(2020, 1, 6),
                MessageId = "message-id"
            };

            var message = MessageFactory.Create(pullRequest);

            var actual = Serialize(message);

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

            var actual = Serialize(message);

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

            var actual = Serialize(message);

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

            var actual = Serialize(message);

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

        private string Serialize(object message)
        {
            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = ("  "),
                OmitXmlDeclaration = true
            };

            var serializer = new XmlSerializer(message.GetType());
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("s", Namespaces.SoapEnvelope);
            ns.Add("wsu", Namespaces.WebServiceSecurityUtility);
            ns.Add("ebms", Namespaces.ElectronicBusinessMessagingService);
            ns.Add("ebbp", Namespaces.ElectronicBusinessProcess);
            ns.Add("wsa", Namespaces.WebServiceAddressing);
            ns.Add("mh", Namespaces.Multihop);

            using(var stringWriter = new StringWriter())
            using(var xmlWriter = XmlWriter.Create(stringWriter, settings))
            {
                serializer.Serialize(xmlWriter, message, ns);
                return stringWriter.ToString();
            }
        }
    }
}
