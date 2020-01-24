using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Xml;
using AS4.Soap;
using XmlSecurityExtensions;

namespace AS4.EESSI.Security
{
    public class EbmsEncrypter
    {
        public XmlDocument Xml { get; set; }
        public IEnumerable<Attachment> Attachments { get; set; }
        public byte[] PublicKeyInAsn1Format { get; set; }
        
        public void Encrypt()
        {
            var encryptionAlgorithm = new AesGcm {KeySize = 128};

            encryptionAlgorithm.GenerateKey();

            byte[] encryptedSymmetricKey = RsaOaepSha256.Encrypt(encryptionAlgorithm.Key, PublicKeyInAsn1Format);

            var encryptedKey = new EncryptedKey
            {
                Id = "ek-" + Guid.NewGuid(),
                EncryptionMethod = new EncryptionMethod(EncryptedXml.XmlEncRSAOAEPUrl),
                CipherData = new CipherData
                {
                    CipherValue = encryptedSymmetricKey
                }
            };
            
            var encryptedDataList = new List<EncryptedData>();

            foreach (Attachment attachment in Attachments)
            {
                Stream encryptedStream = new MemoryStream();
                encryptedStream.Write(encryptionAlgorithm.IV, 0, encryptionAlgorithm.IV.Length);

                var cryptoStream = new CryptoStream(encryptedStream, encryptionAlgorithm.CreateEncryptor(), CryptoStreamMode.Write);
                attachment.Stream.CopyTo(cryptoStream);
                cryptoStream.FlushFinalBlock();
                attachment.Stream = encryptedStream;

                var encryptedData = new EncryptedData
                {
                    Id = "ed-" + Guid.NewGuid(),
                    Type = "http://docs.oasis-open.org/wss/oasis-wss-SwAProfile-1.1#Attachment-Content-Only",
                    EncryptionMethod = new EncryptionMethod("http://www.w3.org/2009/xmlenc11#aes128-gcm"),
                    CipherData = new CipherData
                    {
                        CipherReference = new CipherReference("cid:" + attachment.ContentId)
                    }
                };
                encryptedData.KeyInfo.AddClause(new SecurityTokenReference( encryptedKey.Id));
                encryptedData.CipherData.CipherReference.TransformChain.Add(new AttachmentCiphertextTransform());
                
                encryptedDataList.Add(encryptedData);
                
                encryptedKey.ReferenceList.Add(new DataReference(encryptedData.Id));
            }

            var securityXml = GetSecurity() ?? CreateSecurity();

            foreach (var encryptedData in encryptedDataList)
            {
                Insert(encryptedData.GetXml(), securityXml);
            }

            Insert(encryptedKey.GetXml(), securityXml);
        }
        
        private static void Insert(XmlNode source, XmlNode destination)
        {
            using (var writer = destination.CreateNavigator().PrependChild())
            {
                source.WriteTo(writer);
            }
        }

        private XmlElement GetSecurity()
        {
            var namespaces = new XmlNamespaceManager(Xml.NameTable);
            namespaces.AddNamespace("s", Soap.Namespaces.SoapEnvelope);
            namespaces.AddNamespace("wsse", XmlSecurityExtensions.Namespaces.WebServiceSecurityExtensions);
            return Xml.SelectSingleNode("/s:Envelope/s:Header/wsse:Security", namespaces) as XmlElement;
        }

        private XmlElement GetHeader()
        {
            var namespaces = new XmlNamespaceManager(Xml.NameTable);
            namespaces.AddNamespace("s", Soap.Namespaces.SoapEnvelope);
            return Xml.SelectSingleNode("/s:Envelope/s:Header", namespaces) as XmlElement;
        }

        private XmlElement CreateSecurity()
        {
            var headerXml = GetHeader();
            var securityXml = Xml.CreateElement("wsse", "Security", Soap.Namespaces.WebServiceSecurityExtensions);
            headerXml.AppendChild(securityXml);
            return securityXml;
        }
    }
}
