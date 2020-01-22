using System.Security.Cryptography.Xml;
using System.Xml.Serialization;

namespace AS4.Security
{
    public class IssuerSerial
    {
        [XmlElement(Namespace = SignedXml.XmlDsigNamespaceUrl)]
        public string X509IssuerName { get; set; }

        [XmlElement(Namespace = SignedXml.XmlDsigNamespaceUrl)]
        public string X509SerialNumber { get; set; }
    }
}
