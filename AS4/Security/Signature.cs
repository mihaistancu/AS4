using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace AS4.Security
{
    public class Signature
    {
        private readonly XmlDocument xmlDocument;
        private XmlElement signatureXml;
        
        public Signature(XmlDocument xml)
        {
            xmlDocument = xml;
        }

        public void ComputeSignature(RSA key, IEnumerable<string> uris, string certificateUri)
        {
            var signedXml = new SignedXmlWithNamespacedId(xmlDocument)
            {
                SigningKey = key
            };
            
            foreach (var uri in uris)
            {
                var messaging = new Reference("#" + uri)
                {
                    DigestMethod = SignedXml.XmlDsigSHA256Url
                };
                messaging.AddTransform(new XmlDsigExcC14NTransform());

                signedXml.AddReference(messaging);
            }

            var keyInfo = new KeyInfo();
            keyInfo.AddClause(new SecurityTokenReference(certificateUri));

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
