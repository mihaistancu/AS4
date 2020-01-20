using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Xml;
using AS4.Security.Cryptography;
using AS4.Soap;

namespace AS4.Security
{
    public class Signature
    {
        private readonly XmlDocument xmlDocument;
        private readonly List<Attachment> attachments;
        private XmlElement signatureXml;

        public Signature(XmlDocument xml)
        {
            xmlDocument = xml;
            attachments = new List<Attachment>();
        }

        public void AddData(string uri, Stream stream)
        {
            attachments.Add(new Attachment {ContentId = uri, Stream = stream});
        }

        public void ComputeSignature(RSA key, IEnumerable<string> uris, string keyUri)
        {
            var signedXml = new ExtendedSignedXml(xmlDocument)
            {
                SigningKey = key
            };
            
            foreach (var uri in uris)
            {
                var reference = new Reference
                {
                    Uri = "#" + uri,
                    DigestMethod = SignedXml.XmlDsigSHA256Url
                };
                reference.AddTransform(new XmlDsigExcC14NTransform());
                signedXml.AddReference(reference);
            }

            foreach (var attachment in attachments)
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
            keyInfo.AddClause(new SecurityTokenReference(keyUri));

            signedXml.KeyInfo = keyInfo;

            signedXml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigExcC14NTransformUrl;
            signedXml.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA256Url;
            
            signedXml.ComputeSignature();

            signatureXml = signedXml.GetXml();
        }

        public void VerifySignature(RSA key)
        {
            var signedXml = new ExtendedSignedXml(xmlDocument);
            signedXml.LoadXml(signatureXml);

            foreach (var attachment in attachments)
            {
                var reference = new Reference(attachment.Stream)
                {
                    Uri = "cid:" + attachment.ContentId,
                    DigestMethod = SignedXml.XmlDsigSHA256Url
                };
                reference.AddTransform(new AttachmentContentSignatureTransform());
                signedXml.AddExternalReference(reference);
            }
            
            signedXml.CheckSignature(key);
        }

        public XmlElement GetXml()
        {
            return signatureXml;
        }

        public void LoadXml(XmlElement xml)
        {
            signatureXml = xml;
        }
    }
}
