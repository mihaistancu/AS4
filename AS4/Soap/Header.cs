using System.Xml.Serialization;

namespace AS4.Soap
{
    public class Header
    {
        [XmlElement(Namespace = Namespaces.ElectronicBusinessMessagingService)]
        public Messaging Messaging { get; set; }
        
        [XmlElement(Namespace = Namespaces.WebServiceAddressing)]
        public To To { get; set; }

        [XmlElement(Namespace = Namespaces.WebServiceAddressing)]
        public string Action { get; set; }

        [XmlElement(Namespace = Namespaces.Multihop)]
        public RoutingInput RoutingInput { get; set; }
    }
}
