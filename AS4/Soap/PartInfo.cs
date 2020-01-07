using System.Xml.Serialization;

namespace AS4.Soap
{
    public class PartInfo
    {
        [XmlAttribute(AttributeName = "href")]
        public string Reference { get; set; }
    }
}
