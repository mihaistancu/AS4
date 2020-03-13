using System.Collections.Generic;
using System.Xml.Serialization;

namespace AS4.Soap
{
    public class UserMessage
    {
        [XmlAttribute(AttributeName = "mpc", Namespace = Namespaces.MultiHop)]
        public string MessagePartitionChannel { get; set; }

        [XmlElement(Namespace = Namespaces.ElectronicBusinessMessagingService)]
        public MessageInfo MessageInfo { get; set; }

        [XmlElement(Namespace = Namespaces.ElectronicBusinessMessagingService)]
        public PartyInfo PartyInfo { get; set; }

        [XmlElement(Namespace = Namespaces.ElectronicBusinessMessagingService)]
        public CollaborationInfo CollaborationInfo { get; set; }

        [XmlArray(Namespace = Namespaces.ElectronicBusinessMessagingService)]
        public List<PartInfo> PayloadInfo { get; set; }
    }
}
