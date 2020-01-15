using System.IO;
using System.Xml;
using System.Xml.Serialization;
using AS4.Soap;

namespace AS4.Serialization
{
    public class EnvelopeToXml
    {
        public static XmlDocument Serialize(Envelope message)
        {
            var xml = new XmlDocument();
            xml.LoadXml(ToString(message));
            return xml;
        }

        public static string ToString(Envelope message)
        {
            var settings = GetSettings();
            var namespaces = GetNamespaceManager();
            var serializer = new XmlSerializer(message.GetType());
            
            using(var stringWriter = new StringWriter())
            using(var xmlWriter = XmlWriter.Create(stringWriter, settings))
            {
                serializer.Serialize(xmlWriter, message, namespaces);
                return stringWriter.ToString();
            }
        }

        private static XmlWriterSettings GetSettings()
        {
            return new XmlWriterSettings
            {
                Indent = true,
                IndentChars = ("  "),
                OmitXmlDeclaration = true
            };
        }

        private static XmlSerializerNamespaces GetNamespaceManager()
        {
            var ns = new XmlSerializerNamespaces();
            ns.Add("s", Namespaces.SoapEnvelope);
            ns.Add("wsu", Namespaces.WebServiceSecurityUtility);
            ns.Add("ebms", Namespaces.ElectronicBusinessMessagingService);
            ns.Add("ebbp", Namespaces.ElectronicBusinessProcess);
            ns.Add("wsa", Namespaces.WebServiceAddressing);
            ns.Add("mh", Namespaces.MultiHop);
            return ns;
        }
    }
}
