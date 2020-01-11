using System.Xml;
using AS4.Soap;

namespace AS4.Security
{
    public class Security
    {
        private readonly BinarySecurityToken binarySecurityToken;
        private readonly Signature signature;

        public Security(BinarySecurityToken binarySecurityToken, Signature signature)
        {
            this.binarySecurityToken = binarySecurityToken;
            this.signature = signature;
        }

        public XmlElement GetXml()
        {
            var xml = new XmlDocument();
            var security = xml.CreateElement("Security", Namespaces.WebServiceSecurityExtensions);
            security.SetAttribute("mustUnderstand", Namespaces.WebServiceSecurityUtility, "true");
            security.AppendChild(xml.ImportNode(binarySecurityToken.GetXml(), true));
            security.AppendChild(xml.ImportNode(signature.GetXml(), true));
            return security;
        }
    }
}
