using System.Security.Cryptography;

namespace XmlSecurityExtensions
{
    public class AesGcm : Aes
    {
        public const int NonceSize = 96;

        public byte[] Nonce
        {
            get { return IVValue; }
            set { IVValue = value; }
        }

        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
        {
            return new AesGcmEncryptTransform(rgbKey, rgbIV);
        }

        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
        {
            return new AesGcmDecryptTransform(rgbKey, rgbIV);
        }

        public override void GenerateKey()
        {
            KeyValue = GetRandom(KeySize / 8);
        }

        public override void GenerateIV()
        {
            Nonce = GetRandom(NonceSize / 8);
        }

        private static byte[] GetRandom(int length)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var result = new byte[length];
                rng.GetBytes(result);
                return result;
            }
        }
    }
}
