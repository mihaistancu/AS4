using System.Xml.Serialization;

namespace AS4.Soap
{
    public class Messaging
    {
        [XmlElement(Namespace = Namespaces.Ebms)]
        public SignalMessage SignalMessage { get; set; }

        [XmlElement(Namespace = Namespaces.Ebms)]
        public UserMessage UserMessage { get; set; }
    }
}
