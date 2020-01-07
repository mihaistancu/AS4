using System.Xml.Serialization;

namespace AS4.Soap
{
    public class UserMessage
    {
        [XmlAttribute(AttributeName = "mpc", Namespace = Namespaces.Multihop)]
        public string MessagePartitionChannel { get; set; }

        [XmlElement(Namespace = Namespaces.Ebms)]
        public MessageInfo MessageInfo { get; set; }

        [XmlElement(Namespace = Namespaces.Ebms)]
        public PartyInfo PartyInfo { get; set; }
    }
}
