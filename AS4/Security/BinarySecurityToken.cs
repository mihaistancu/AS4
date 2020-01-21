using System.Xml.Serialization;
using AS4.Soap;

namespace AS4.Security
{
    public class BinarySecurityToken
    {
        [XmlAttribute(Namespace = Namespaces.WebServiceSecurityUtility)]
        public string Id { get; set; }

        [XmlAttribute]
        public string ValueType { get; set; }
        
        [XmlAttribute]
        public string EncodingType { get; set; }

        [XmlText]
        public byte[] Value { get; set; }
    }
}
