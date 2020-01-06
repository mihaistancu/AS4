using System.Xml.Serialization;

namespace AS4.Soap
{
    public class SignalMessage
    {
        [XmlElement(Namespace = Namespaces.Ebms)]
        public MessageInfo MessageInfo { get; set; }

        [XmlElement(Namespace = Namespaces.Ebms)]
        public PullRequest PullRequest { get; set; }

        [XmlElement(Namespace = Namespaces.Ebms)]
        public Receipt Receipt { get; set; }
    }
}
