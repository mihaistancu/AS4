using System.Xml.Serialization;

namespace AS4.Security
{
    public class SignedProperties
    {
        [XmlAttribute]
        public string Id { get; set; }

        public SignedSignatureProperties SignedSignatureProperties { get; set; }

        public SignedDataObjectProperties SignedDataObjectProperties { get; set; }
    }
}
