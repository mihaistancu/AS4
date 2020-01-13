using System;
using System.Xml;

namespace AS4.Security
{
    public class BinarySecurityToken
    {
        private const string Base64Binary = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary";
        private const string X509TokenProfile = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3";

        public string Id { get; set; }
        public byte[] Token { get; set; }

        public BinarySecurityToken()
        {
        }

        public BinarySecurityToken(byte[] token)
        {
            Id = Guid.NewGuid().ToString();
            Token = token;
        }
        
        public XmlElement GetXml()
        {
            var xmlDocument = new XmlDocument();
            var binarySecurityToken = xmlDocument.CreateElement("BinarySecurityToken", Namespaces.WebServiceSecurityExtensions);
            binarySecurityToken.SetAttribute("EncodingType", Base64Binary);
            binarySecurityToken.SetAttribute("ValueType", X509TokenProfile);
            binarySecurityToken.SetAttribute("id", Namespaces.WebServiceSecurityUtility, Id);
            binarySecurityToken.InnerText = Convert.ToBase64String(Token);
            return binarySecurityToken;
        }

        public void LoadXml(XmlElement xml)
        {   
            Id = xml.GetAttribute("Id", Namespaces.WebServiceSecurityUtility);
            Token = Convert.FromBase64String(xml.InnerText);
        }
    }
}
