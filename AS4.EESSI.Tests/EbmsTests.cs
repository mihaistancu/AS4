using AS4.EESSI.Security;
using AS4.Serialization;
using AS4.Soap;
using AS4.Tests.Factories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AS4.EESSI.Tests.Factories;

namespace AS4.EESSI.Tests
{
    [TestClass]
    public class EbmsTests
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
            var xml = ObjectToXml.Serialize(envelope);

            var ebmsSigner = new EbmsSigner
            {
                Xml = xml,
                Certificate = Certificates.CreateSelfSigned(),
                Uris = new []{envelope.Header.Messaging.Id, envelope.Body.Id},
                Attachments = attachments
            };
            ebmsSigner.Sign();

            var ebmsVerifier = new EbmsVerifier
            {
                Xml = xml,
                Attachments = attachments
            };
            ebmsVerifier.Verify();
        }
    }
}
