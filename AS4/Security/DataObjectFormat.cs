using System.Xml.Serialization;

namespace AS4.Security
{
    public class DataObjectFormat
    {
        [XmlAttribute]
        public string ObjectReference { get; set; }

        public string MimeType { get; set; }
    }
}
