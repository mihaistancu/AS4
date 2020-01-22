using System;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;
using AS4.Security;
using AS4.Serialization;
using XmlSecurityExtensions;

namespace AS4.EESSI.Security
{
    public class XadesSigner
    {   
        public XmlDocument Xml { get; set; }
        public X509Certificate2 Certificate { get; set; }

        public void Sign()
        {
            var qualifyingProperties = GetQualifyingProperties(Certificate);
            var qualifyingPropertiesXml = ObjectToXml.Serialize(qualifyingProperties);

            var signedXml = new ExtendedSignedXml(Xml);

            signedXml.Signature.Id = qualifyingProperties.Target;
            signedXml.SigningKey = Certificate.GetRSAPrivateKey();
            signedXml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigExcC14NTransformUrl;
            signedXml.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA256Url;
            
            var documentReference = new Reference
            {
                Id = qualifyingProperties.SignedProperties.SignedDataObjectProperties.DataObjectFormat.ObjectReference,
                Type = null,
                Uri = ""
            };

            documentReference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
            documentReference.DigestMethod = SignedXml.XmlDsigSHA256Url;
            signedXml.AddReference(documentReference);
            
            var signedProperties = new Reference
            {
                Type = Namespaces.SignedProperties,
                Uri = "#" + qualifyingProperties.SignedProperties.Id
            };

            signedProperties.AddTransform(new XmlDsigExcC14NTransform());
            signedProperties.DigestMethod = SignedXml.XmlDsigSHA256Url;
            signedXml.AddReference(signedProperties);

            var dataObject = new DataObject
            {
                Data = qualifyingPropertiesXml.ChildNodes
            };
            signedXml.AddObject(dataObject);

            var certificateKeyInfo = new KeyInfo();
            certificateKeyInfo.AddClause(new KeyInfoX509Data(Certificate));
            signedXml.KeyInfo = certificateKeyInfo;

            signedXml.ComputeSignature();
            
            var signature = signedXml.GetXml();
            Insert(signature, Xml.DocumentElement);
        }

        private static void Insert(XmlNode source, XmlNode destination)
        {
            using (var writer = destination.CreateNavigator().AppendChild())
            {
                source.WriteTo(writer);
            }
        }

        private QualifyingProperties GetQualifyingProperties(X509Certificate2 certificate)
        {
            var sha256 = new SHA256CryptoServiceProvider();

            return new QualifyingProperties
            {
                Target = "id-" + Guid.NewGuid(),
                SignedProperties = new SignedProperties
                {
                    Id = "xades-" + Guid.NewGuid(),
                    SignedSignatureProperties =  new SignedSignatureProperties
                    {
                        SigningTime = DateTime.UtcNow,
                        SigningCertificate = new SigningCertificate
                        {
                            Cert = new Cert
                            {
                                CertDigest = new CertDigest
                                {
                                    DigestMethod = new DigestMethod
                                    {
                                        Algorithm = SignedXml.XmlDsigSHA256Url
                                    },
                                    DigestValue = sha256.ComputeHash(certificate.RawData)
                                },
                                IssuerSerial = new IssuerSerial
                                {
                                    X509IssuerName = new string(certificate.IssuerName.Name.Where(c => !char.IsWhiteSpace(c)).ToArray()),
                                    X509SerialNumber = BigInteger.Parse(certificate.SerialNumber, NumberStyles.HexNumber).ToString()
                                }
                            }
                        }
                    },
                    SignedDataObjectProperties = new SignedDataObjectProperties
                    {
                        DataObjectFormat = new DataObjectFormat
                        {
                            ObjectReference = "ref-" + Guid.NewGuid(),
                            MimeType = "text/xml"
                        }
                    }
                }
            };
        }
    }
}
