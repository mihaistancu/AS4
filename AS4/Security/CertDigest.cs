using System.Security.Cryptography.Xml;
using System.Xml.Serialization;

namespace AS4.Security
{
    public class CertDigest
    {
        [XmlElement(Namespace = SignedXml.XmlDsigNamespaceUrl)]
        public DigestMethod DigestMethod { get; set; }

        [XmlElement(Namespace = SignedXml.XmlDsigNamespaceUrl)]
        public byte[] DigestValue { get; set; }
    }
}
