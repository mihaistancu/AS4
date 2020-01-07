using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using AS4.Soap;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AS4.Tests
{
    [TestClass]
    public class SerializationTests
    {
        public static string PullRequestXml =
@"<Envelope xmlns:ebms=""http://docs.oasis-open.org/ebxml-msg/ebms/v3.0/ns/core/200704/"" xmlns:wsa=""http://www.w3.org/2005/08/addressing"" xmlns:ebbp=""http://docs.oasis-open.org/ebxml-bp/ebbp-signals-2.0"" xmlns:s=""http://www.w3.org/2003/05/soap-envelope"" xmlns:wsu=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd"" xmlns:mh=""http://docs.oasis-open.org/ebxml-msg/ns/ebms/v3.0/multihop/200902/"">
  <s:Header>
    <ebms:Messaging>
      <ebms:SignalMessage>
        <ebms:MessageInfo>
          <ebms:Timestamp>2020-01-06T00:00:00</ebms:Timestamp>
          <ebms:MessageId>message-id</ebms:MessageId>
        </ebms:MessageInfo>
        <ebms:PullRequest mpc=""mpc-id"" />
      </ebms:SignalMessage>
    </ebms:Messaging>
  </s:Header>
  <s:Body wsu:Id=""body-id"" />
</Envelope>";
        
        public static string ReceiptXml =
@"<Envelope xmlns:ebms=""http://docs.oasis-open.org/ebxml-msg/ebms/v3.0/ns/core/200704/"" xmlns:wsa=""http://www.w3.org/2005/08/addressing"" xmlns:ebbp=""http://docs.oasis-open.org/ebxml-bp/ebbp-signals-2.0"" xmlns:s=""http://www.w3.org/2003/05/soap-envelope"" xmlns:wsu=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd"" xmlns:mh=""http://docs.oasis-open.org/ebxml-msg/ns/ebms/v3.0/multihop/200902/"">
  <s:Header>
    <ebms:Messaging>
      <ebms:SignalMessage>
        <ebms:MessageInfo>
          <ebms:Timestamp>2020-01-06T00:00:00</ebms:Timestamp>
          <ebms:MessageId>receipt-message-id</ebms:MessageId>
          <ebms:RefToMessageId>user-message-id</ebms:RefToMessageId>
        </ebms:MessageInfo>
        <ebms:Receipt>
          <ebbp:NonRepudiationInformation />
        </ebms:Receipt>
      </ebms:SignalMessage>
    </ebms:Messaging>
    <wsa:To s:role=""http://docs.oasis-open.org/ebxml-msg/ebms/v3.0/ns/part2/200811/nextmsh"">http://docs.oasis-open.org/ebxml-msg/ebms/v3.0/ns/part2/200811/icloud</wsa:To>
    <wsa:Action>http://docs.oasis-open.org/ebxml-msg/ebms/v3.0/ns/core/200704/oneWay.receipt</wsa:Action>
    <mh:RoutingInput wsa:IsReferenceParameter=""true"" s:MustUnderstandSerializedValue=""false"" s:role=""http://docs.oasis-open.org/ebxml-msg/ebms/v3.0/ns/part2/200811/nextmsh"">
      <mh:UserMessage mpc=""http://docs.oasis-open.org/ebxml-msg/ebms/v3.0/ns/core/200704/defaultMPC.receipt"">
        <ebms:MessageInfo>
          <ebms:Timestamp>2020-01-05T00:00:00</ebms:Timestamp>
          <ebms:MessageId>user-message-id</ebms:MessageId>
        </ebms:MessageInfo>
        <ebms:PartyInfo>
          <ebms:From>
            <ebms:PartyId type=""urn:eu:europa:ec:dgempl:eessi:ir"">party-1</ebms:PartyId>
            <ebms:Role>urn:eu:europa:ec:dgempl:eessi:ir:institution</ebms:Role>
          </ebms:From>
          <ebms:To>
            <ebms:PartyId type=""urn:eu:europa:ec:dgempl:eessi:ir"">party-2</ebms:PartyId>
            <ebms:Role>urn:eu:europa:ec:dgempl:eessi:ir:institution</ebms:Role>
          </ebms:To>
        </ebms:PartyInfo>
        <ebms:CollaborationInfo>
          <ebms:Service type=""urn:eu:europa:ec:dgempl:eessi"">BusinessMessaging</ebms:Service>
          <ebms:Action>Send.response</ebms:Action>
          <ebms:ConversationId>conversation-id</ebms:ConversationId>
        </ebms:CollaborationInfo>
        <ebms:PayloadInfo>
          <ebms:PartInfo href=""cid:DefaultSED"">
            <ebms:PartProperties>
              <ebms:Property name=""PartType"">SED</ebms:Property>
              <ebms:Property name=""MimeType"">application/xml</ebms:Property>
              <ebms:Property name=""CompressionType"">application/gzip</ebms:Property>
            </ebms:PartProperties>
          </ebms:PartInfo>
        </ebms:PayloadInfo>
      </mh:UserMessage>
    </mh:RoutingInput>
  </s:Header>
  <s:Body wsu:Id=""body-id"" />
</Envelope>";

        public static string ErrorXml =
@"<Envelope xmlns:ebms=""http://docs.oasis-open.org/ebxml-msg/ebms/v3.0/ns/core/200704/"" xmlns:wsa=""http://www.w3.org/2005/08/addressing"" xmlns:ebbp=""http://docs.oasis-open.org/ebxml-bp/ebbp-signals-2.0"" xmlns:s=""http://www.w3.org/2003/05/soap-envelope"" xmlns:wsu=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd"" xmlns:mh=""http://docs.oasis-open.org/ebxml-msg/ns/ebms/v3.0/multihop/200902/"">
  <s:Header>
    <ebms:Messaging>
      <ebms:SignalMessage>
        <ebms:MessageInfo>
          <ebms:Timestamp>2020-01-06T00:00:00</ebms:Timestamp>
          <ebms:MessageId>error-message-id</ebms:MessageId>
          <ebms:RefToMessageId>user-message-id</ebms:RefToMessageId>
        </ebms:MessageInfo>
        <ebms:Error category=""Content"" errorCode=""EBMS:0004"" origin=""ebMS"" severity=""failure"" shortDescription=""Other"">
          <ebms:Description>error description</ebms:Description>
          <ebms:ErrorDetail>error detail</ebms:ErrorDetail>
        </ebms:Error>
      </ebms:SignalMessage>
    </ebms:Messaging>
    <wsa:To s:role=""http://docs.oasis-open.org/ebxml-msg/ebms/v3.0/ns/part2/200811/nextmsh"">http://docs.oasis-open.org/ebxml-msg/ebms/v3.0/ns/part2/200811/icloud</wsa:To>
    <wsa:Action>http://docs.oasis-open.org/ebxml-msg/ebms/v3.0/ns/core/200704/oneWay.error</wsa:Action>
    <mh:RoutingInput wsa:IsReferenceParameter=""true"" s:MustUnderstandSerializedValue=""false"" s:role=""http://docs.oasis-open.org/ebxml-msg/ebms/v3.0/ns/part2/200811/nextmsh"">
      <mh:UserMessage mpc=""http://docs.oasis-open.org/ebxml-msg/ebms/v3.0/ns/core/200704/defaultMPC.error"">
        <ebms:MessageInfo>
          <ebms:Timestamp>2020-01-05T00:00:00</ebms:Timestamp>
          <ebms:MessageId>user-message-id</ebms:MessageId>
        </ebms:MessageInfo>
        <ebms:PartyInfo>
          <ebms:From>
            <ebms:PartyId type=""urn:eu:europa:ec:dgempl:eessi:ir"">party-1</ebms:PartyId>
            <ebms:Role>urn:eu:europa:ec:dgempl:eessi:ir:institution</ebms:Role>
          </ebms:From>
          <ebms:To>
            <ebms:PartyId type=""urn:eu:europa:ec:dgempl:eessi:ir"">party-2</ebms:PartyId>
            <ebms:Role>urn:eu:europa:ec:dgempl:eessi:ir:institution</ebms:Role>
          </ebms:To>
        </ebms:PartyInfo>
        <ebms:CollaborationInfo>
          <ebms:Service type=""urn:eu:europa:ec:dgempl:eessi"">BusinessMessaging</ebms:Service>
          <ebms:Action>Send.response</ebms:Action>
          <ebms:ConversationId>conversation-id</ebms:ConversationId>
        </ebms:CollaborationInfo>
        <ebms:PayloadInfo>
          <ebms:PartInfo href=""cid:DefaultSED"">
            <ebms:PartProperties>
              <ebms:Property name=""PartType"">SED</ebms:Property>
              <ebms:Property name=""MimeType"">application/xml</ebms:Property>
              <ebms:Property name=""CompressionType"">application/gzip</ebms:Property>
            </ebms:PartProperties>
          </ebms:PartInfo>
        </ebms:PayloadInfo>
      </mh:UserMessage>
    </mh:RoutingInput>
  </s:Header>
  <s:Body wsu:Id=""body-id"" />
</Envelope>";

        public static string UserMessageXml =
@"<Envelope xmlns:ebms=""http://docs.oasis-open.org/ebxml-msg/ebms/v3.0/ns/core/200704/"" xmlns:wsa=""http://www.w3.org/2005/08/addressing"" xmlns:ebbp=""http://docs.oasis-open.org/ebxml-bp/ebbp-signals-2.0"" xmlns:s=""http://www.w3.org/2003/05/soap-envelope"" xmlns:wsu=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd"" xmlns:mh=""http://docs.oasis-open.org/ebxml-msg/ns/ebms/v3.0/multihop/200902/"">
  <s:Header>
    <ebms:Messaging>
      <ebms:UserMessage>
        <ebms:MessageInfo>
          <ebms:Timestamp>2020-01-06T00:00:00</ebms:Timestamp>
          <ebms:MessageId>message-id</ebms:MessageId>
        </ebms:MessageInfo>
        <ebms:PartyInfo>
          <ebms:From>
            <ebms:PartyId type=""urn:eu:europa:ec:dgempl:eessi:ir"">party-1</ebms:PartyId>
            <ebms:Role>urn:eu:europa:ec:dgempl:eessi:ir:institution</ebms:Role>
          </ebms:From>
          <ebms:To>
            <ebms:PartyId type=""urn:eu:europa:ec:dgempl:eessi:ir"">party-2</ebms:PartyId>
            <ebms:Role>urn:eu:europa:ec:dgempl:eessi:ir:institution</ebms:Role>
          </ebms:To>
        </ebms:PartyInfo>
        <ebms:CollaborationInfo>
          <ebms:Service type=""urn:eu:europa:ec:dgempl:eessi"">BusinessMessaging</ebms:Service>
          <ebms:Action>Send</ebms:Action>
          <ebms:ConversationId>conversation-id</ebms:ConversationId>
        </ebms:CollaborationInfo>
        <ebms:PayloadInfo>
          <ebms:PartInfo href=""cid:DefaultSED"">
            <ebms:PartProperties>
              <ebms:Property name=""PartType"">SED</ebms:Property>
              <ebms:Property name=""MimeType"">application/xml</ebms:Property>
              <ebms:Property name=""CompressionType"">application/gzip</ebms:Property>
            </ebms:PartProperties>
          </ebms:PartInfo>
        </ebms:PayloadInfo>
      </ebms:UserMessage>
    </ebms:Messaging>
  </s:Header>
  <s:Body wsu:Id=""body-id"" />
</Envelope>";
        
        [TestMethod]
        public void PullRequestSerializesCorrectly()
        {
            var pullRequest = new PullRequestDetails
            {
                Timestamp = new DateTime(2020, 1, 6),
                MessageId = "message-id"
            };

            var message = MessageFactory.Create(pullRequest);

            var xml = Serialize(message);

            Assert.AreEqual(PullRequestXml, xml);
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

            var xml = Serialize(message);

            Assert.AreEqual(ReceiptXml, xml);
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

            var xml = Serialize(message);

            Assert.AreEqual(ErrorXml, xml);
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

            var xml = Serialize(message);

            Assert.AreEqual(UserMessageXml, xml);
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
