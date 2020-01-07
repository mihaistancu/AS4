using System.Xml.Serialization;

namespace AS4.Soap
{
    public class Service
    {
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }

        [XmlText]
        public string Value { get; set; }
    }
}
