using System.Xml.Serialization;

namespace AS4.Soap
{
    public class SignalMessage
    {
        [XmlElement(Namespace = Namespaces.ElectronicBusinessMessagingService)]
        public MessageInfo MessageInfo { get; set; }

        [XmlElement(Namespace = Namespaces.ElectronicBusinessMessagingService)]
        public PullRequest PullRequest { get; set; }

        [XmlElement(Namespace = Namespaces.ElectronicBusinessMessagingService)]
        public Receipt Receipt { get; set; }

        [XmlElement(Namespace = Namespaces.ElectronicBusinessMessagingService)]
        public Error Error { get; set; }
    }
}
