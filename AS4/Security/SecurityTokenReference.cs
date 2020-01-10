using System.Security.Cryptography.Xml;
using System.Xml;

namespace AS4.Security
{
    public class SecurityTokenReference : KeyInfoClause
    {
        private const string WebServiceSecurityExtensions = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";
        private const string X509TokenProfile = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3";

        private readonly string referenceId;

        public SecurityTokenReference(string referenceId)
        {
            this.referenceId = referenceId;
        }

        public override XmlElement GetXml()
        {
            var xmlDocument = new XmlDocument {PreserveWhitespace = true};

            var securityTokenReference = xmlDocument.CreateElement("SecurityTokenReference", WebServiceSecurityExtensions);

            var reference = xmlDocument.CreateElement("Reference", WebServiceSecurityExtensions);
            reference.SetAttribute("ValueType", X509TokenProfile);
            reference.SetAttribute("URI", "#" + referenceId);

            securityTokenReference.AppendChild(reference);
            return securityTokenReference;
        }

        public override void LoadXml(XmlElement element)
        {
        }
    }
}
