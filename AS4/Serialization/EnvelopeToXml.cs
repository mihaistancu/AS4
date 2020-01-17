using System.Xml;
using System.Xml.Serialization;
using AS4.Soap;

namespace AS4.Serialization
{
    public class EnvelopeToXml
    {
        private static readonly XmlSerializerNamespaces SerializerNamespaces;

        static EnvelopeToXml()
        {
            SerializerNamespaces = new XmlSerializerNamespaces();
            SerializerNamespaces.Add("s", Namespaces.SoapEnvelope);
            SerializerNamespaces.Add("wsu", Namespaces.WebServiceSecurityUtility);
            SerializerNamespaces.Add("ebms", Namespaces.ElectronicBusinessMessagingService);
            SerializerNamespaces.Add("ebbp", Namespaces.ElectronicBusinessProcess);
            SerializerNamespaces.Add("wsa", Namespaces.WebServiceAddressing);
            SerializerNamespaces.Add("mh", Namespaces.MultiHop);
        }

        public static XmlDocument Serialize(Envelope message)
        {
            var xml = new XmlDocument {PreserveWhitespace = true};
            var serializer = new XmlSerializer(message.GetType());
            using(var xmlWriter = xml.CreateNavigator().AppendChild())
            {
                serializer.Serialize(xmlWriter, message, SerializerNamespaces);
            }
            return xml;
        }
    }
}
