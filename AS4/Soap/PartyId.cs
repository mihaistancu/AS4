using System.Xml.Serialization;

namespace AS4.Soap
{
    public class PartyId
    {
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }

        [XmlText]
        public string Value { get; set; }
    }
}
