using System.Xml.Serialization;

namespace AS4.Soap
{
    public class Messaging
    {
        [XmlElement(Namespace = Namespaces.ElectronicBusinessMessagingService)]
        public SignalMessage SignalMessage { get; set; }

        [XmlElement(Namespace = Namespaces.ElectronicBusinessMessagingService)]
        public UserMessage UserMessage { get; set; }
    }
}
