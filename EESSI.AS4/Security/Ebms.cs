using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using XmlSecurityExtensions;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography;
using System.IO;
using System;
using AS4.Soap;

namespace EESSI.AS4.Security
{
    public class Ebms
    {
        private const string SoapEnvelope = "";

        public static void Sign(XmlDocument xml, X509Certificate2 certificate, IEnumerable<string> uris, IEnumerable<Attachment> attachments)
        {
            var binarySecurityToken = new BinarySecurityToken(certificate.GetRawCertData());
            var signature = new Signature(xml);
            foreach (var attachment in attachments)
            {
                signature.AddData(attachment.ContentId, attachment.Stream);
            }
            signature.ComputeSignature(certificate.GetRSAPrivateKey(), uris, binarySecurityToken.Id);
            var security = new SecurityElement(binarySecurityToken, signature);
            GetHeader(xml).AppendChild(xml.ImportNode(security.GetXml(), true));
        }

        public static void VerifySignature(XmlDocument xml, IEnumerable<Attachment> attachments)
        {   
            var binarySecurityToken = new BinarySecurityToken();
            var binarySecurityTokenXml = GetBinarySecurityToken(xml);
            binarySecurityToken.LoadXml(binarySecurityTokenXml);
            var certificate = new X509Certificate2(binarySecurityToken.Token);

            var signature = new Signature(xml);
            foreach (var attachment in attachments)
            {
                signature.AddData(attachment.ContentId, attachment.Stream);
            }
            var signatureXml = GetSignature(xml);
            signature.LoadXml(signatureXml);
            signature.VerifySignature(certificate.GetRSAPublicKey());
        }

        private static XmlElement GetHeader(XmlDocument xml)
        {
            return GetNodeByXpath(xml, "/s:Envelope/s:Header");
        }

        private static XmlElement GetSignature(XmlDocument xml)
        {
            return GetNodeByXpath(xml, "/s:Envelope/s:Header/wsse:Security/ds:Signature");
        }

        private static XmlElement GetBinarySecurityToken(XmlDocument xml)
        {
            return GetNodeByXpath(xml, "/s:Envelope/s:Header/wsse:Security/wsse:BinarySecurityToken");
        }

        private static XmlElement GetNodeByXpath(XmlDocument xml, string xpath)
        {
            var namespaceManager = new XmlNamespaceManager(xml.NameTable);
            namespaceManager.AddNamespace("s", SoapEnvelope);
            namespaceManager.AddNamespace("wsse", XmlSecurityExtensions.Namespaces.WebServiceSecurityExtensions);
            namespaceManager.AddNamespace("ds", XmlSecurityExtensions.Namespaces.DigitalSignature);
            return xml.SelectSingleNode(xpath, namespaceManager) as XmlElement;
        }
    }

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
            var binarySecurityToken = xmlDocument.CreateElement("BinarySecurityToken", XmlSecurityExtensions.Namespaces.WebServiceSecurityExtensions);
            binarySecurityToken.SetAttribute("EncodingType", Base64Binary);
            binarySecurityToken.SetAttribute("ValueType", X509TokenProfile);
            binarySecurityToken.SetAttribute("id", XmlSecurityExtensions.Namespaces.WebServiceSecurityUtility, Id);
            binarySecurityToken.InnerText = Convert.ToBase64String(Token);
            return binarySecurityToken;
        }

        public void LoadXml(XmlElement xml)
        {   
            Id = xml.GetAttribute("Id", XmlSecurityExtensions.Namespaces.WebServiceSecurityUtility);
            Token = Convert.FromBase64String(xml.InnerText);
        }
    }

    public class SecurityElement
    {
        private readonly BinarySecurityToken binarySecurityToken;
        private readonly Signature signature;

        public SecurityElement(BinarySecurityToken binarySecurityToken, Signature signature)
        {
            this.binarySecurityToken = binarySecurityToken;
            this.signature = signature;
        }

        public XmlElement GetXml()
        {
            var xml = new XmlDocument();
            var security = xml.CreateElement("Security", XmlSecurityExtensions.Namespaces.WebServiceSecurityExtensions);
            security.SetAttribute("mustUnderstand", XmlSecurityExtensions.Namespaces.WebServiceSecurityUtility, "true");
            security.AppendChild(xml.ImportNode(binarySecurityToken.GetXml(), true));
            security.AppendChild(xml.ImportNode(signature.GetXml(), true));
            return security;
        }
    }
}
