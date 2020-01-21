using System;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;
using XmlSecurityExtensions;

namespace AS4.EESSI.Security
{
    public class Xades
    {
        public const string XAdESPrefix = "xades";
        public const string XadesNamespaceUrl = "http://uri.etsi.org/01903/v1.3.2#";
        public const string XmlDsigNamespacePrefix = "ds";
        
        public void Sign(XmlDocument xml, X509Certificate2 certificate)
        {
            var signedXml = new ExtendedSignedXml(xml);
           
            var signatureId = "id-" + Guid.NewGuid().ToString().Replace("-", string.Empty);
            var referenceId = "ref0-" + signatureId;

            signedXml.Signature.Id = signatureId;

            signedXml.SigningKey = certificate.GetRSAPrivateKey();

            signedXml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigExcC14NTransformUrl;
            signedXml.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA256Url;
            
            var documentReference = new Reference
            {
                Id = referenceId,
                Type = null,
                Uri = ""
            };

            documentReference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
            documentReference.DigestMethod = SignedXml.XmlDsigSHA256Url;
            signedXml.AddReference(documentReference);

            var signedPropertiesId = $"xades-{signatureId}";
            var signedProperties = new Reference
            {
                Type = "http://uri.etsi.org/01903#SignedProperties",
                Uri = "#" + signedPropertiesId
            };

            signedProperties.AddTransform(new XmlDsigExcC14NTransform());
            signedProperties.DigestMethod = SignedXml.XmlDsigSHA256Url;
            signedXml.AddReference(signedProperties);


            DataObject dataObject = new DataObject();
            XmlElement result = xml.CreateElement(XAdESPrefix, "QualifyingProperties", XadesNamespaceUrl);
            result.SetAttribute("Target", "#" + signedXml.Signature.Id);
            dataObject.Data = result.SelectNodes(".");
            signedXml.AddObject(dataObject);

            XmlElement qualifyingPropertiesNode = result;

            XmlElement signedPropertiesNode = AddSignedPropertiesNode(xml, qualifyingPropertiesNode, signedPropertiesId);

            XmlElement signedSignatureProperties = AddSignedSignaturePropertiesNode(xml, signedPropertiesNode);

            const string DateTimeCanonicalFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'";
            DateTime now = DateTime.Now.ToUniversalTime();
            var signingTime = now.ToString(DateTimeCanonicalFormat);
            AddSigningTimeNode(xml, signedSignatureProperties, signingTime);

            SHA256 cryptoServiceProvider = new SHA256CryptoServiceProvider();
            byte[] hash = cryptoServiceProvider.ComputeHash(certificate.RawData);
            var digestValue =  Convert.ToBase64String(hash);

            var issuerName =  new string(certificate.IssuerName.Name.Where(c => !char.IsWhiteSpace(c)).ToArray());

            var serialNumber  = BigInteger.Parse(certificate.SerialNumber, NumberStyles.HexNumber);

            AddSigningCertificate(xml, signedSignatureProperties, digestValue, issuerName, serialNumber.ToString());

            XmlElement signedDataObjectProperties = AddSignedDataObjectPropertiesNode(xml, signedPropertiesNode);

            XmlElement dataObjectFormat = AddDataObjectFormatNode(xml, signedDataObjectProperties, referenceId);

            AddMimeTypeNode(xml, dataObjectFormat, "text/xml");

            KeyInfo certificateKeyInfo = new KeyInfo();
            certificateKeyInfo.AddClause(new KeyInfoX509Data(certificate));
            signedXml.KeyInfo = certificateKeyInfo;

            signedXml.ComputeSignature();
            
            XmlElement signedXmlElement = signedXml.GetXml();
            XmlNode digitalSignature = xml.ImportNode(signedXmlElement, true);
            xml.DocumentElement.AppendChild(digitalSignature);
        }
        
        private XmlElement AddSignedPropertiesNode(XmlDocument xmlDocument, XmlElement qualifyingPropertiesNode, string id)
        {
            XmlElement result = xmlDocument.CreateElement(XAdESPrefix, "SignedProperties", XadesNamespaceUrl);
            qualifyingPropertiesNode.AppendChild(result);
            XmlElement signedPropertiesNode =  result;
            signedPropertiesNode.SetAttribute("Id", id);

            return signedPropertiesNode;
        }

        private XmlElement AddSignedSignaturePropertiesNode(XmlDocument xmlDocument, XmlElement propertiesNode)
        {
            XmlElement result = xmlDocument.CreateElement(XAdESPrefix, "SignedSignatureProperties", XadesNamespaceUrl);
            propertiesNode.AppendChild(result);
            return result;
        }

        private void AddSigningTimeNode(XmlDocument xmlDocument, XmlElement signedSignaturePropertiesNode, string signingTime)
        {
            XmlElement result = xmlDocument.CreateElement(XAdESPrefix, "SigningTime", XadesNamespaceUrl);
            signedSignaturePropertiesNode.AppendChild(result);
            XmlElement newNode = result;
            newNode.InnerText = signingTime;
        }

        private void AddSigningCertificate(XmlDocument xmlDocument, XmlElement signedSignatureProperties, string digestValue, string issuer, string serialNumber)
        {
            XmlElement result = xmlDocument.CreateElement(XAdESPrefix, "SigningCertificate", XadesNamespaceUrl);
            signedSignatureProperties.AppendChild(result);
            XmlElement signingCertificateNode = result;
            XmlElement result1 = xmlDocument.CreateElement(XAdESPrefix, "Cert", XadesNamespaceUrl);
            signingCertificateNode.AppendChild(result1);
            XmlElement certNode = result1;
            AddCertDigestNode(xmlDocument, certNode, digestValue);
            AddIssuerSerialNode(xmlDocument, certNode, issuer, serialNumber);
        }

        private void AddCertDigestNode(XmlDocument xmlDocument, XmlElement certNode, string digestValue)
        {
            XmlElement result = xmlDocument.CreateElement(XAdESPrefix, "CertDigest", XadesNamespaceUrl);
            certNode.AppendChild(result);
            XmlElement certDigestNode =  result;
            XmlElement result1 = xmlDocument.CreateElement(XmlDsigNamespacePrefix, "DigestMethod", SignedXml.XmlDsigNamespaceUrl);
            certDigestNode.AppendChild(result1);
            XmlElement certDigestMethod =  result1;
            certDigestMethod.SetAttribute("Algorithm", SignedXml.XmlDsigSHA256Url);
            XmlElement result2 = xmlDocument.CreateElement(XmlDsigNamespacePrefix, "DigestValue", SignedXml.XmlDsigNamespaceUrl);
            certDigestNode.AppendChild(result2);
            XmlElement newNode = result2;
            newNode.InnerText = digestValue;
        }

        private void AddIssuerSerialNode(XmlDocument xmlDocument, XmlElement certNode, string issuer, string serialNumber)
        {
            XmlElement result = xmlDocument.CreateElement(XAdESPrefix, "IssuerSerial", XadesNamespaceUrl);
            certNode.AppendChild(result);
            XmlElement issuerSerialNode = result;
            XmlElement result1 = xmlDocument.CreateElement(XmlDsigNamespacePrefix, "X509IssuerName", SignedXml.XmlDsigNamespaceUrl);
            issuerSerialNode.AppendChild(result1);
            XmlElement newNode = result1;
            newNode.InnerText = issuer;
            XmlElement result2 = xmlDocument.CreateElement(XmlDsigNamespacePrefix, "X509SerialNumber", SignedXml.XmlDsigNamespaceUrl);
            issuerSerialNode.AppendChild(result2);
            XmlElement newNode1 = result2;
            newNode1.InnerText = serialNumber;
        }

        private XmlElement AddSignedDataObjectPropertiesNode(XmlDocument xmlDocument, XmlElement propertiesNode)
        {
            XmlElement result = xmlDocument.CreateElement(XAdESPrefix, "SignedDataObjectProperties", XadesNamespaceUrl);
            propertiesNode.AppendChild(result);
            return result;
        }

        private XmlElement AddDataObjectFormatNode(XmlDocument xmlDocument, XmlElement signedDataObjectPropertiesNode, string id)
        {
            XmlElement result = xmlDocument.CreateElement(XAdESPrefix, "DataObjectFormat", XadesNamespaceUrl);
            signedDataObjectPropertiesNode.AppendChild(result);
            XmlElement dataObjectFormatNode =  result;
            dataObjectFormatNode.SetAttribute("ObjectReference", string.Format("#{0}", id));
            return dataObjectFormatNode;
        }

        private void AddMimeTypeNode(XmlDocument xmlDocument, XmlElement dataObject, string mimeType)
        {
            XmlElement result = xmlDocument.CreateElement(XAdESPrefix, "MimeType", XadesNamespaceUrl);
            dataObject.AppendChild(result);
            XmlElement newNode = result;
            newNode.InnerText = mimeType;
        }

        public bool VerifySignature(XmlDocument xml)
        {
            var signedXml = new ExtendedSignedXml(xml);
            XmlElement signatureNode = GetSignatureNode(xml);
            
            signedXml.LoadXml(signatureNode);
            
            return signedXml.CheckSignature();
        }

        public static XmlElement GetSignatureNode(XmlDocument xml)
        {
            
            XmlNamespaceManager nsMgr = new XmlNamespaceManager(xml.NameTable);
            nsMgr.AddNamespace(XmlDsigNamespacePrefix, SignedXml.XmlDsigNamespaceUrl);

            string nodeName = $"{XmlDsigNamespacePrefix}:Signature";
            XmlElement signatureElement = xml.SelectSingleNode(nodeName, nsMgr) as XmlElement;

            XmlNodeList elems = xml.GetElementsByTagName("Signature", SignedXml.XmlDsigNamespaceUrl);
            if(elems.Count > 0)
            {
                signatureElement = (XmlElement)elems[0];
            }

            return signatureElement;
        }
    }
}
