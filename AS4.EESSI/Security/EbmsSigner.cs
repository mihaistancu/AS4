using System.Security.Cryptography.X509Certificates;
using System.Xml;
using XmlSecurityExtensions;
using System.Security.Cryptography.Xml;
using System;
using System.Collections.Generic;
using AS4.Security;
using AS4.Serialization;
using AS4.Soap;

namespace AS4.EESSI.Security
{
    public class EbmsSigner
    {
        public XmlDocument Xml { get; set; }
        public X509Certificate2 Certificate { get; set; }
        public IEnumerable<string> Uris { get; set; }
        public IEnumerable<Attachment> Attachments { get; set; }

        public void Sign()
        {
            var security = new AS4.Security.Security
            {
                BinarySecurityToken = new BinarySecurityToken
                {
                    Id = Guid.NewGuid().ToString(),
                    EncodingType = Soap.Namespaces.Base64Binary,
                    ValueType = Soap.Namespaces.X509TokenProfile,
                    Value = Certificate.GetRawCertData()
                }
            };

            var securityXml = ObjectToXml.Serialize(security);

            var signedXml = new ExtendedSignedXml(Xml)
            {
                SigningKey = Certificate.GetRSAPrivateKey()
            };
            
            foreach (var uri in Uris)
            {
                var reference = new Reference
                {
                    Uri = "#" + uri,
                    DigestMethod = SignedXml.XmlDsigSHA256Url
                };
                reference.AddTransform(new XmlDsigExcC14NTransform());
                signedXml.AddReference(reference);
            }

            foreach (var attachment in Attachments)
            {
                var reference = new Reference(new NonCloseableStream(attachment.Stream))
                {
                    Uri = "cid:" + attachment.ContentId,
                    DigestMethod = SignedXml.XmlDsigSHA256Url
                };
                reference.AddTransform(new AttachmentContentSignatureTransform());
                signedXml.AddExternalReference(reference);
            }

            var keyInfo = new KeyInfo();
            keyInfo.AddClause(new SecurityTokenReference(security.BinarySecurityToken.Id));

            signedXml.KeyInfo = keyInfo;

            signedXml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigExcC14NTransformUrl;
            signedXml.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA256Url;
            
            signedXml.ComputeSignature();

            var signature = signedXml.GetXml();

            Insert(signature, securityXml.DocumentElement);
            
            var namespaces = new XmlNamespaceManager(Xml.NameTable);
            namespaces.AddNamespace("s", Soap.Namespaces.SoapEnvelope);
            var header = Xml.SelectSingleNode("/s:Envelope/s:Header", namespaces);

            Insert(securityXml, header);
        }

        private static void Insert(XmlNode source, XmlNode destination)
        {
            using (var writer = destination.CreateNavigator().AppendChild())
            {
                source.WriteTo(writer);
            }
        }
    }
}
