using System.Xml.Serialization;

namespace AS4.Security
{
    public class DigestMethod
    {
        [XmlAttribute]
        public string Algorithm { get; set; }
    }
}
