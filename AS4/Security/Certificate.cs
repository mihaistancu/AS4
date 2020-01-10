using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;
using AS4.Serialization;
using AS4.Soap;

namespace AS4.Security
{
    public class Certificate
    {
        private readonly X509Certificate2 certificate;

        public Certificate(string path, string password)
        {
            certificate = new X509Certificate2(path, password);    
        }

        public XmlElement Sign(Envelope envelope)
        {
            var serializer = new Serializer();
            
            envelope.Header.Security = new Soap.Security
            {
                BinarySecurityToken = new BinarySecurityToken
                {
                    EncodingType = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary",
                    Id = "xyz",
                    Value = certificate.GetRawCertData()
                }
            };

            var xml = serializer.Serialize(envelope);

            var doc = new XmlDocument { PreserveWhitespace = true };
            doc.LoadXml(xml);
            var signedXml = new SignedXmlWithNamespacedId(doc);
            
            signedXml.SigningKey = certificate.GetRSAPrivateKey();
            
            var messaging = new Reference("#" + envelope.Header.Messaging.Id)
            {
                DigestMethod = SignedXml.XmlDsigSHA256Url
            };
            messaging.AddTransform(new XmlDsigExcC14NTransform());

            var body = new Reference("#" + envelope.Body.Id)
            {
                DigestMethod = SignedXml.XmlDsigSHA256Url
            };
            body.AddTransform(new XmlDsigExcC14NTransform());
            
            signedXml.AddReference(messaging);
            signedXml.AddReference(body);
            
            var keyInfo = new KeyInfo();
            keyInfo.AddClause(new SecurityTokenReference("xyz"));

            signedXml.KeyInfo = keyInfo;

            signedXml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigExcC14NTransformUrl;
            signedXml.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA256Url;

            signedXml.ComputeSignature();

            return signedXml.GetXml();
        }
    }
}
