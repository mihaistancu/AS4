using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using XmlSecurityExtensions;
using System.Security.Cryptography.Xml;
using AS4.Serialization;
using AS4.Soap;

namespace AS4.EESSI.Security
{
    public class EbmsVerifier
    {
        public XmlDocument Xml { get; set; }
        public IEnumerable<Attachment> Attachments { get; set; }

        public bool Verify()
        {   
            var namespaces = new XmlNamespaceManager(Xml.NameTable);
            namespaces.AddNamespace("s", Soap.Namespaces.SoapEnvelope);
            namespaces.AddNamespace("wsse", XmlSecurityExtensions.Namespaces.WebServiceSecurityExtensions);
            namespaces.AddNamespace("ds", XmlSecurityExtensions.Namespaces.DigitalSignature);

            var signatureXml = Xml.SelectSingleNode("/s:Envelope/s:Header/wsse:Security/ds:Signature", namespaces) as XmlElement;

            var signedXml = new ExtendedSignedXml(Xml);
            signedXml.LoadXml(signatureXml);

            foreach (var attachment in Attachments)
            {
                var reference = new Reference(attachment.Stream)
                {
                    Uri = "cid:" + attachment.ContentId,
                    DigestMethod = SignedXml.XmlDsigSHA256Url
                };
                reference.AddTransform(new AttachmentContentSignatureTransform());
                signedXml.AddExternalReference(reference);
            }
            
            var securityXml = Xml.SelectSingleNode("/s:Envelope/s:Header/wsse:Security", namespaces);
            var security = XmlToObject.Deserialize<AS4.Security.Security>(securityXml);
            var certificate = new X509Certificate2(security.BinarySecurityToken.Value);
            return signedXml.CheckSignature(certificate.GetRSAPublicKey());
        }
    }
}
