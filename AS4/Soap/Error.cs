using System.Xml.Serialization;

namespace AS4.Soap
{
    public class Error
    {
        [XmlAttribute(AttributeName = "category")]
        public string Category { get; set; }

        [XmlAttribute(AttributeName = "errorCode")]
        public string ErrorCode { get; set; }

        [XmlAttribute(AttributeName = "origin")]
        public string Origin { get; set; }

        [XmlAttribute(AttributeName = "severity")]
        public string Severity { get; set; }

        [XmlAttribute(AttributeName = "shortDescription")]
        public string ShortDescription { get; set; }

        public string Description { get; set; }

        public string ErrorDetail { get; set; }
    }
}
