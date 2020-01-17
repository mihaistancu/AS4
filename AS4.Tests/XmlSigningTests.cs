using System.Linq;
using AS4.Security;
using AS4.Serialization;
using AS4.Soap;
using AS4.Tests.Factories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AS4.Tests
{
    [TestClass]
    public class XmlSigningTests
    {   
        [TestMethod]
        public void ReceiptIsSignedCorrectly()
        {
            CheckSigning(Envelopes.Receipt);
        }

        [TestMethod]
        public void ErrorIsSignedCorrectly()
        {
            CheckSigning(Envelopes.Error);
        }

        [TestMethod]
        public void PullRequestIsSignedCorrectly()
        {
            CheckSigning(Envelopes.PullRequest);
        }

        [TestMethod]
        public void UserMessageIsSignedCorrectly()
        {
            CheckSigning(Envelopes.UserMessage);
        }

        [TestMethod]
        public void UserMessageWithAttachmentIsSignedCorrectly()
        {
            CheckSigning(Envelopes.UserMessage, Attachments.Generate());
        }

        private void CheckSigning(Envelope envelope, params Attachment[] attachments)
        {
            var xml = EnvelopeToXml.Serialize(envelope);
            var certificate = Certificates.CreateSelfSigned();
            var uris = new []{envelope.Header.Messaging.Id, envelope.Body.Id};
            xml.Sign(certificate, uris, attachments);
            xml.VerifySignature(attachments);
        }
    }
}
