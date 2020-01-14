using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using AS4.Mime;

namespace AS4.Security
{
    public static class XmlDocumentExtensions
    {
        public static void Sign(this XmlDocument xml, X509Certificate2 certificate, IEnumerable<string> uris, IEnumerable<Attachment> attachments)
        {
            var binarySecurityToken = new BinarySecurityToken(certificate.GetRawCertData());
            var signature = new Signature(xml);
            signature.ComputeSignature(certificate.GetRSAPrivateKey(), uris, binarySecurityToken.Id, attachments);
            var security = new Security(binarySecurityToken, signature);
            xml.GetHeader().AppendChild(xml.ImportNode(security.GetXml(), true));
        }

        public static void VerifySignature(this XmlDocument xml)
        {   
            var binarySecurityToken = new BinarySecurityToken();
            var binarySecurityTokenXml = xml.GetBinarySecurityToken();
            binarySecurityToken.LoadXml(binarySecurityTokenXml);
            var certificate = new X509Certificate2(binarySecurityToken.Token);

            var signature = new Signature(xml);
            var signatureXml = xml.GetSignature();
            signature.LoadXml(signatureXml);
            signature.VerifySignature(certificate.GetRSAPublicKey());
        }

        private static XmlElement GetHeader(this XmlDocument xml)
        {
            return xml.GetNodeByXpath("/s:Envelope/s:Header");
        }

        private static XmlElement GetSignature(this XmlDocument xml)
        {
            return xml.GetNodeByXpath("/s:Envelope/s:Header/wsse:Security/ds:Signature");
        }

        private static XmlElement GetBinarySecurityToken(this XmlDocument xml)
        {
            return xml.GetNodeByXpath("/s:Envelope/s:Header/wsse:Security/wsse:BinarySecurityToken");
        }

        private static XmlElement GetNodeByXpath(this XmlDocument xml, string xpath)
        {
            var namespaceManager = new XmlNamespaceManager(xml.NameTable);
            namespaceManager.AddNamespace("s", Namespaces.SoapEnvelope);
            namespaceManager.AddNamespace("wsse", Namespaces.WebServiceSecurityExtensions);
            namespaceManager.AddNamespace("ds", Namespaces.DigitalSignature);
            return xml.SelectSingleNode(xpath, namespaceManager) as XmlElement;
        }
    }
}
