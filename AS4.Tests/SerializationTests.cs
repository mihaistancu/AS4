using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AS4.Tests
{
    [TestClass]
    public class SerializationTests
    {
        const string EmptyXml = @"<?xml version=""1.0"" encoding=""utf-16""?><Envelope xmlns:s=""http://www.w3.org/2003/05/soap-envelope"" />";

        [TestMethod]
        public void TestMethod1()
        {
            var message = new SoapEnvelope();

            var xml = Serialize(message);

            Assert.AreEqual(EmptyXml, xml);
        }

        private string Serialize(object message)
        {   
            var serializer = new XmlSerializer(message.GetType());
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("s", SoapEnvelope.Namespace);

            using(var stringWriter = new StringWriter())
            using(var xmlWriter = XmlWriter.Create(stringWriter))
            {
                serializer.Serialize(xmlWriter, message, ns);
                return stringWriter.ToString();
            }
        }
    }
}
