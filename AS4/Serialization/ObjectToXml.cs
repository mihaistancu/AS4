using System.Xml;
using System.Xml.Serialization;
using AS4.Soap;

namespace AS4.Serialization
{
    public class ObjectToXml
    {
        private static readonly XmlSerializerNamespaces SerializerNamespaces;

        static ObjectToXml()
        {
            SerializerNamespaces = new XmlSerializerNamespaces();
            SerializerNamespaces.Add("s", Namespaces.SoapEnvelope);
            SerializerNamespaces.Add("wsu", Namespaces.WebServiceSecurityUtility);
            SerializerNamespaces.Add("ebms", Namespaces.ElectronicBusinessMessagingService);
            SerializerNamespaces.Add("ebbp", Namespaces.ElectronicBusinessProcess);
            SerializerNamespaces.Add("wsa", Namespaces.WebServiceAddressing);
            SerializerNamespaces.Add("mh", Namespaces.MultiHop);
        }

        public static XmlDocument Serialize<T>(T item)
        {
            var xml = new XmlDocument {PreserveWhitespace = true};
            var serializer = new XmlSerializer(item.GetType());
            using(var xmlWriter = xml.CreateNavigator().AppendChild())
            {
                serializer.Serialize(xmlWriter, item, SerializerNamespaces);
            }
            return xml;
        }
    }
}
