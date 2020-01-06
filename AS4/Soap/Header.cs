using System.Xml.Serialization;

namespace AS4.Soap
{
    public class Header
    {
        [XmlElement(Namespace = Namespaces.Ebms)]
        public Messaging Messaging { get; set; }
        
        [XmlElement(Namespace = Namespaces.WebServiceAddressing)]
        public To To { get; set; }

        [XmlElement(Namespace = Namespaces.WebServiceAddressing)]
        public string Action { get; set; }
    }
}
