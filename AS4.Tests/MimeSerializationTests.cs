using System.IO;
using System.Linq;
using AS4.Serialization;
using AS4.Soap;
using AS4.Tests.Asserts;
using AS4.Tests.Factories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MimeKit;

namespace AS4.Tests
{
    [TestClass]
    public class MimeSerializationTests
    {
        [TestMethod]
        public void PullRequestSerializesSuccessfully()
        {
            CheckSerialization(Envelopes.PullRequest);
        }

        [TestMethod]
        public void ReceiptSerializesSuccessfully()
        {
            CheckSerialization(Envelopes.Receipt);
        }

        [TestMethod]
        public void ErrorSerializesSuccessfully()
        {
            CheckSerialization(Envelopes.Error);
        }

        [TestMethod]
        public void UserMessageSerializesSuccessfully()
        {
            CheckSerialization(Envelopes.UserMessage);
        }

        [TestMethod]
        public void UserMessageWithAttachmentsSerializesSuccessfully()
        {
            CheckSerialization(Envelopes.UserMessage, Attachments.Generate(2).ToArray());
        }

        private void CheckSerialization(Envelope envelope, params Attachment[] attachments)
        {
            var message = Messages.Create(envelope, attachments);
            var mimeEntity = As4MessageToMimeEntity.Serialize(message);
            var contentType = mimeEntity.ContentType;
            var stream = new MemoryStream();
            mimeEntity.WriteTo(stream, true);
            stream.Position = 0;
            var decodedEntity = MimeEntity.Load(contentType, stream);
            var decodedMessage = MimeEntityToAs4Message.Deserialize(decodedEntity);
            MessageAssert.AreEqual(message, decodedMessage);
        }
    }
}
