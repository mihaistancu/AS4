using System.Xml.Serialization;

namespace AS4.Soap
{
    public class Property
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlText]
        public string Value { get;set; }
    }
}
