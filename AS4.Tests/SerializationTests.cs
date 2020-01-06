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
        const string EmptyXml = @"<?xml version=""1.0"" encoding=""utf-16""?><Envelope xmlns:s=""http://www.w3.org/2003/05/soap-envelope"" xmlns:wsu=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd""><s:Header /><s:Body wsu:Id=""body-id"" /></Envelope>";

        [TestMethod]
        public void SoapEnvelopeSerializesCorrectly()
        {
            var message = new Envelope
            {
                Header = new Header(),
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
            var serializer = new XmlSerializer(message.GetType());
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("s", Namespaces.SoapEnvelope);
            ns.Add("wsu", Namespaces.WebServiceSecurityUtility);

            using(var stringWriter = new StringWriter())
            using(var xmlWriter = XmlWriter.Create(stringWriter))
            {
                serializer.Serialize(xmlWriter, message, ns);
                return stringWriter.ToString();
            }
        }
    }
}
