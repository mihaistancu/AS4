using System.Security.Cryptography;

namespace XmlSecurityExtensions
{
    public class AesGcm : SymmetricAlgorithm
    {
        private readonly RandomNumberGenerator random;

        public override KeySizes[] LegalBlockSizes => new[] {new KeySizes(128, 128, 0)};

        public override KeySizes[] LegalKeySizes => new[] {new KeySizes(128, 256, 64)};

        public AesGcm()
        {
            BlockSizeValue = 128;
            random = new RNGCryptoServiceProvider();
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
            byte[] key = new byte[KeySize / 8];
            random.GetBytes(key);
            Key = key;
        }

        public override void GenerateIV()
        {
            byte[] iv = new byte[BlockSize / 8];
            random.GetBytes(iv);
            IV = iv;
        }
    }
}
