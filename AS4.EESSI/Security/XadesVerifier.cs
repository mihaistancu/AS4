using System.Linq;
using System.Security.Cryptography.Xml;
using System.Xml;
using XmlSecurityExtensions;

namespace AS4.EESSI.Security
{
    public class XadesVerifier
    {
        public XmlDocument Xml { get; set; }

        public bool Verify()
        {
            var signedXml = new ExtendedSignedXml(Xml);
            var signature = GetSignature(Xml);
            signedXml.LoadXml(signature);
            return signedXml.CheckSignature();
        }

        public static XmlElement GetSignature(XmlDocument xml)
        {   
            return xml.GetElementsByTagName("Signature", SignedXml.XmlDsigNamespaceUrl).Cast<XmlElement>().Single();
        }
    }
}
