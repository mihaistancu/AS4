using System.Xml.Serialization;

namespace AS4.Soap
{
    public class Body
    {
        [XmlAttribute(Namespace = Namespaces.WebServiceSecurityUtility)]
        public string Id { get; set; }
    }
}
