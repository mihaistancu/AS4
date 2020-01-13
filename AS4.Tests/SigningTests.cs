using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using AS4.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AS4.Tests
{
    [TestClass]
    public class SigningTests
    {
        [TestMethod]
        public void ReceiptIsSignedCorrectly()
        {
            var xml = new XmlDocument();
            xml.LoadXml(Resources.Receipt);

            var rsa = RSA.Create();
            var req = new CertificateRequest("cn=test", rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            var certificate = req.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(5));
            
            xml.Sign(certificate, "messaging-id", "body-id");

            xml.VerifySignature();
        }
    }
}
