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
        public static string EmptyXml =
@"<Envelope xmlns:wsu=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd"" xmlns:s=""http://www.w3.org/2003/05/soap-envelope"" xmlns:ebms=""http://docs.oasis-open.org/ebxml-msg/ebms/v3.0/ns/core/200704/"">
  <s:Header>
    <ebms:Messaging>
      <ebms:SignalMessage />
    </ebms:Messaging>
  </s:Header>
  <s:Body wsu:Id=""body-id"" />
</Envelope>";

        [TestMethod]
        public void SoapEnvelopeSerializesCorrectly()
        {
            var message = new Envelope
            {
                Header = new Header
                {
                    Messaging = new Messaging
                    {
                        SignalMessage = new SignalMessage()
                    }
                },
                Body = new Body
                {
                    Id="body-id"
                }
            };

            var xml = Serialize(message);

            Assert.AreEqual(EmptyXml, xml);
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
            ns.Add("ebms", Namespaces.Ebms);

            using(var stringWriter = new StringWriter())
            using(var xmlWriter = XmlWriter.Create(stringWriter, settings))
            {
                serializer.Serialize(xmlWriter, message, ns);
                return stringWriter.ToString();
            }
        }
    }
}
