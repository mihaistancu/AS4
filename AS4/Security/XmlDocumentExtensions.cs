using System.Security.Cryptography.X509Certificates;
using System.Xml;
using AS4.Soap;

namespace AS4.Security
{
    public static class XmlDocumentExtensions
    {
        public static void Sign(this XmlDocument xml, X509Certificate2 certificate, params string[] uris)
        {
            var binarySecurityToken = new BinarySecurityToken(certificate.GetRawCertData());

            var signature = new Signature(xml, certificate.GetRSAPrivateKey(), uris, binarySecurityToken.Id);

            var security = new Security(binarySecurityToken, signature);
            
            xml.GetHeader().AppendChild(xml.ImportNode(security.GetXml(), true));
        }

        private static XmlNode GetHeader(this XmlDocument xml)
        {
            var namespaceManager = new XmlNamespaceManager(xml.NameTable);
            namespaceManager.AddNamespace("s", Namespaces.SoapEnvelope);
            return xml.SelectSingleNode("/s:Envelope/s:Header", namespaceManager);
        }
    }
}
