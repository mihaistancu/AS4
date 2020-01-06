using System.Xml.Serialization;

namespace AS4.Soap
{
    public class PullRequest
    {
        [XmlAttribute(AttributeName = "mpc")]
        public string MessagePartitionChannel { get;set; }
    }
}
