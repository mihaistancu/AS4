using System.Xml.Serialization;

namespace AS4.Soap
{
    [XmlType(Namespace = Namespaces.SoapEnvelope)]
    public class Body
    {
        [XmlAttribute(Namespace = Namespaces.WebServiceSecurityUtility)]
        public string Id { get; set; }
    }
}
