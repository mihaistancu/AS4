using System;
using System.Security.Cryptography.X509Certificates;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace XmlSecurityExtensions
{
    public class RsaOaepSha256
    {
        public static byte[] Encrypt(byte[] input, byte[] publicKeyInAsn1Format)
        {
            AsymmetricKeyParameter publicKey = GetPublicKey(publicKeyInAsn1Format);
            return Process(input, publicKey, true);
        }

        public static byte[] Decrypt(byte[] input, X509Certificate2 certificate)
        {
            AsymmetricKeyParameter privateKey = GetPrivateKey(certificate);
            return Process(input, privateKey, false);
        }

        private static AsymmetricKeyParameter GetPublicKey(byte[] publicKeyInAsn1Format)
        {
            DerSequence asn1Sequence = Asn1Object.FromByteArray(publicKeyInAsn1Format) as DerSequence;
            if (asn1Sequence == null)
            {
                throw new ArgumentException("Unable to decode the public key. Verify the public key is in the correct format.", nameof(publicKeyInAsn1Format));
            }
            return new RsaKeyParameters(false, ((DerInteger)asn1Sequence[0]).PositiveValue, ((DerInteger)asn1Sequence[1]).PositiveValue);
        }
        
        private static AsymmetricKeyParameter GetPrivateKey(X509Certificate2 certificate)
        {
            var keyPair = DotNetUtilities.GetRsaKeyPair(certificate.GetRSAPrivateKey());
            return keyPair.Private;
        }

        private static byte[] Process(byte[] data, AsymmetricKeyParameter key, bool isEncryption)
        {
            var encoding = new OaepEncoding(new RsaEngine(), new Sha256Digest(), new Sha1Digest(), new byte[0]);
            encoding.Init(isEncryption, key);
            return encoding.ProcessBlock(data, 0, data.Length);
        }
    }
}
