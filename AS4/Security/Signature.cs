using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace AS4.Security
{
    public class Signature
    {
        private readonly XmlDocument xml;
        private readonly RSA key;
        private readonly IEnumerable<string> uris;
        private readonly string certificateUri;

        public Signature(XmlDocument xml, RSA key, IEnumerable<string> uris, string certificateUri)
        {
            this.xml = xml;
            this.key = key;
            this.uris = uris;
            this.certificateUri = certificateUri;
        }

        public XmlElement GetXml()
        {
            var signature = new SignedXmlWithNamespacedId(xml)
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

                signature.AddReference(messaging);
            }

            var keyInfo = new KeyInfo();
            keyInfo.AddClause(new SecurityTokenReference(certificateUri));

            signature.KeyInfo = keyInfo;

            signature.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigExcC14NTransformUrl;
            signature.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA256Url;
            
            signature.ComputeSignature();

            return signature.GetXml();
        }
    }
}
