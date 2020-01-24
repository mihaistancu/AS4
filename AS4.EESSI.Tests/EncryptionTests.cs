using AS4.EESSI.Security;
using AS4.EESSI.Tests.Factories;
using AS4.Serialization;
using AS4.Soap;
using AS4.Tests.Factories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;

namespace AS4.EESSI.Tests
{
    [TestClass]
    public class EncryptionTests
    {
        [TestMethod]
        public void UserMessageWithAttachmentIsEncryptedSuccessfully()
        {
            CheckEncryption(Envelopes.UserMessage, Attachments.Generate());
        }

        private void CheckEncryption(Envelope envelope, Attachment attachment)
        {
            var xml = ObjectToXml.Serialize(envelope);
            var certificate = Certificates.CreateSelfSigned();

            var expected = Copy(attachment.Stream);

            var encrypter = new EbmsEncrypter
            {
                Xml = xml,
                Attachments = new [] { attachment },
                PublicKeyInAsn1Format = certificate.GetPublicKey()
            };
            encrypter.Encrypt();

            var decrypter = new EbmsDecrypter
            {
                Xml = xml,
                Attachments = new [] { attachment },
                Certificate = certificate
            };
            decrypter.Decrypt();

            var actual = Copy(attachment.Stream);

            Assert.IsTrue(expected.SequenceEqual(actual));
        }

        private byte[] Copy(Stream stream)
        {
            var memoryStream = stream as MemoryStream;
            return memoryStream.ToArray();
        }
    }
}
