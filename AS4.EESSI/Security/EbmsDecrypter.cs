using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;
using AS4.Soap;
using XmlSecurityExtensions;

namespace AS4.EESSI.Security
{
    public class EbmsDecrypter
    {
        public Attachment[] Attachments { get; set; }
        public X509Certificate2 Certificate { get; set; }
        public XmlDocument Xml { get; set; }

        public void Decrypt()
        {
            var encryptedKeyXml = GetEncryptedKey();
            var encryptedKey = new EncryptedKey();
            encryptedKey.LoadXml(encryptedKeyXml);
        
            byte[] symmetricKey = RsaOaepSha256.Decrypt(encryptedKey.CipherData.CipherValue, Certificate);
            var symmetricAlgorithm = new AesGcm {Key = symmetricKey};

            foreach (var attachment in Attachments)
            {
                attachment.Stream.Position = 0;
                var nonce = new byte[AesGcm.NonceSize / 8];
                attachment.Stream.Read(nonce, 0, nonce.Length);
                symmetricAlgorithm.Nonce = nonce;

                var decryptedStream = new MemoryStream();
                var cryptoStream = new CryptoStream(attachment.Stream, symmetricAlgorithm.CreateDecryptor(), CryptoStreamMode.Read);
                cryptoStream.CopyTo(decryptedStream);
                if (!cryptoStream.HasFlushedFinalBlock)
                {
                    cryptoStream.FlushFinalBlock();
                }
                attachment.Stream = decryptedStream;
            }
        }

        private XmlElement GetEncryptedKey()
        {
            var namespaces = new XmlNamespaceManager(Xml.NameTable);
            namespaces.AddNamespace("s", Soap.Namespaces.SoapEnvelope);
            namespaces.AddNamespace("wsse", XmlSecurityExtensions.Namespaces.WebServiceSecurityExtensions);
            namespaces.AddNamespace("xenc", EncryptedXml.XmlEncNamespaceUrl);
            return Xml.SelectSingleNode("/s:Envelope/s:Header/wsse:Security/xenc:EncryptedKey", namespaces) as XmlElement;
        }
    }
}
