using System.Collections.Generic;
using System.Xml.Serialization;

namespace AS4.Soap
{
    public class PartInfo
    {
        [XmlAttribute(AttributeName = "href")]
        public string Reference { get; set; }

        public List<Property> PartProperties { get; set; }
    }
}
