using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace AS4.Security
{
    public class Signature
    {
        private readonly XmlDocument xmlDocument;
        private readonly List<ExternalData> attachments;
        private XmlElement signatureXml;

        public Signature(XmlDocument xml)
        {
            xmlDocument = xml;
            attachments = new List<ExternalData>();
        }

        public void AddData(string uri, Stream stream)
        {
            attachments.Add(new ExternalData {Uri = uri, Stream = stream});
        }

        public void ComputeSignature(RSA key, IEnumerable<string> uris, string keyUri)
        {
            var signedXml = new SignedXmlWithNamespacedId(xmlDocument)
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
                var reference = new Reference(attachment.Stream)
                {
                    Uri = "cid:" + attachment.Uri,
                    DigestMethod = SignedXml.XmlDsigSHA256Url
                };
                reference.AddTransform(new AttachmentContentSignatureTransform());
                signedXml.AddReference(reference);
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
            var signedXml = new SignedXmlWithNamespacedId(xmlDocument);
            signedXml.LoadXml(signatureXml);

            foreach (var attachment in attachments)
            {
                var reference = new Reference(attachment.Stream)
                {
                    Uri = "cid:" + attachment.Uri,
                    DigestMethod = SignedXml.XmlDsigSHA256Url
                };
                reference.AddTransform(new AttachmentContentSignatureTransform());
                Replace(signedXml.SignedInfo, reference);
            }
            
            signedXml.CheckSignature(key);
        }

        private void Replace(SignedInfo signedInfo, Reference reference)
        {
            var existing = GetReferenceByUri(signedInfo, reference.Uri);
            reference.DigestValue = existing.DigestValue;
            signedInfo.References.Remove(existing);
            signedInfo.AddReference(reference);
        }

        private Reference GetReferenceByUri(SignedInfo signedInfo, string uri)
        {
            foreach (Reference reference in signedInfo.References)
            {
                if (reference.Uri == uri)
                {
                    return reference;
                }
            }

            throw new Exception("Reference not found");
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
