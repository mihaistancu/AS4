using System.Security.Cryptography;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace XmlSecurityExtensions
{
    internal class AesGcmEncryptTransform : ICryptoTransform
    {
        private readonly GcmBlockCipher cipher;

        public AesGcmEncryptTransform(byte[] key, byte[] iv)
        {
            cipher = new GcmBlockCipher(new AesEngine());
            var cipherParameters = ParameterUtilities.CreateKeyParameter("AES", key);
            cipher.Init(true, new ParametersWithIV(cipherParameters, iv));
        }

        public void Dispose()
        {
        }

        public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            return cipher.ProcessBytes(inputBuffer, inputOffset, inputCount, outputBuffer, outputOffset);
        }

        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            byte[] output = new byte[cipher.GetOutputSize(inputCount)];

            int numBytesProcessed = cipher.ProcessBytes(inputBuffer, inputOffset, inputCount, output, 0);
            cipher.DoFinal(output, numBytesProcessed);

            return output;
        }

        public int InputBlockSize => cipher.GetBlockSize();

        public int OutputBlockSize => cipher.GetBlockSize();

        public bool CanTransformMultipleBlocks => false;

        public bool CanReuseTransform => false;
    }
}
