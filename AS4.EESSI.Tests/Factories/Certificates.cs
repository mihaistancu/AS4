using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace AS4.EESSI.Tests.Factories
{
    public class Certificates
    {
        public static X509Certificate2 CreateSelfSigned()
        {
            var rsa = RSA.Create();
            var req = new CertificateRequest("cn=test", rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            return req.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(5));
        }
    }
}
