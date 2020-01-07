using System.Xml.Serialization;

namespace AS4.Soap
{
    public class UserMessage
    {
        [XmlAttribute(AttributeName = "mpc", Namespace = Namespaces.Multihop)]
        public string MessagePartitionChannel { get; set; }

        [XmlElement(Namespace = Namespaces.ElectronicBusinessMessagingService)]
        public MessageInfo MessageInfo { get; set; }

        [XmlElement(Namespace = Namespaces.ElectronicBusinessMessagingService)]
        public PartyInfo PartyInfo { get; set; }

        [XmlElement(Namespace = Namespaces.ElectronicBusinessMessagingService)]
        public CollaborationInfo CollaborationInfo { get; set; }

        [XmlElement(Namespace = Namespaces.ElectronicBusinessMessagingService)]
        public PayloadInfo PayloadInfo { get; set; }
    }
}
