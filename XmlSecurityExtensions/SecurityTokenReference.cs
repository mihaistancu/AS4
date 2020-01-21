using System.Security.Cryptography.Xml;
using System.Xml;

namespace XmlSecurityExtensions
{
    public class SecurityTokenReference : KeyInfoClause
    {
        private readonly string referenceId;

        public SecurityTokenReference(string referenceId)
        {
            this.referenceId = referenceId;
        }

        public override XmlElement GetXml()
        {
            var xmlDocument = new XmlDocument {PreserveWhitespace = true};

            var securityTokenReference = xmlDocument.CreateElement("SecurityTokenReference", Namespaces.WebServiceSecurityExtensions);

            var reference = xmlDocument.CreateElement("Reference", Namespaces.WebServiceSecurityExtensions);
            reference.SetAttribute("ValueType", Namespaces.X509TokenProfile);
            reference.SetAttribute("URI", "#" + referenceId);

            securityTokenReference.AppendChild(reference);
            return securityTokenReference;
        }

        public override void LoadXml(XmlElement element)
        {
        }
    }
}
