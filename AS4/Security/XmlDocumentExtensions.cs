using System.Security.Cryptography.X509Certificates;
using System.Xml;
using AS4.Soap;

namespace AS4.Security
{
    public static class XmlDocumentExtensions
    {
        public static void Sign(this XmlDocument xml, X509Certificate2 certificate, params string[] uris)
        {
            var binarySecurityToken = new BinarySecurityToken("xyz", certificate.GetRawCertData());

            var signature = new Signature(xml, certificate.GetRSAPrivateKey(), uris, binarySecurityToken.Id);

            var security = xml.CreateElement("Security", Namespaces.WebServiceSecurityExtensions);
            security.SetAttribute("mustUnderstand", Namespaces.WebServiceSecurityUtility, "true");
            security.AppendChild(xml.ImportNode(binarySecurityToken.GetXml(), true));
            security.AppendChild(xml.ImportNode(signature.GetXml(), true));

            var namespaceManager = new XmlNamespaceManager(xml.NameTable);
            namespaceManager.AddNamespace("s", Namespaces.SoapEnvelope);
            var header = xml.SelectSingleNode("/s:Envelope/s:Header", namespaceManager);
            header.AppendChild(security);
        }
    }
}
