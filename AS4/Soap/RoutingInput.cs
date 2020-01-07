using System.Xml.Serialization;

namespace AS4.Soap
{
    public class RoutingInput
    {
        [XmlAttribute(Namespace = Namespaces.WebServiceAddressing)]
        public bool IsReferenceParameter { get; set; }

        [XmlAttribute(Namespace = Namespaces.SoapEnvelope)]
        public bool MustUnderstandSerializedValue { get; set; }

        [XmlAttribute(AttributeName = "role", Namespace = Namespaces.SoapEnvelope)]
        public string Role { get; set; }
    }
}
