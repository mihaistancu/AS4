using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AS4.Tests
{
    [TestClass]
    public class SerializationTests
    {
        const string EmptyXml = @"<?xml version=""1.0"" encoding=""utf-16""?><As4Message xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" />";

        [TestMethod]
        public void TestMethod1()
        {
            var message = new As4Message();

            var xml = Serialize(message);

            Assert.AreEqual(EmptyXml, xml);
        }

        private string Serialize(As4Message message)
        {   
            var serializer = new XmlSerializer(typeof(As4Message));
            
            using(var stringWriter = new StringWriter())
            using(var xmlWriter = XmlWriter.Create(stringWriter))
            {
                serializer.Serialize(xmlWriter, message);
                return stringWriter.ToString();
            }
        }
    }
}
