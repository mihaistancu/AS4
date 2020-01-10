using System.Xml.Serialization;

namespace AS4.Soap
{
    public class BinarySecurityToken
    {
        [XmlAttribute]
        public string EncodingType { get; set; }

        [XmlAttribute]
        public string ValueType { get; set; }

        [XmlAttribute(Namespace = Namespaces.WebServiceSecurityUtility)]
        public string Id { get; set; }

        [XmlText]
        public byte[] Value { get;set; }
    }
}
