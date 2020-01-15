using System.Security.Cryptography.X509Certificates;
using System.Xml;
using AS4.Security;
using AS4.Soap;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AS4.Tests
{
    [TestClass]
    public class SigningTests
    {
        private readonly XmlDocument xml = new XmlDocument();
        private readonly X509Certificate2 certificate = Certificate.CreateSelfSigned();
        private readonly string[] uris = {"messaging-id", "body-id"};
        private readonly Attachment[] noAttachments = { };
        
        [TestMethod]
        public void ReceiptIsSignedCorrectly()
        {
            xml.LoadXml(Resources.Receipt);
            xml.Sign(certificate, uris, noAttachments);
            xml.VerifySignature(noAttachments);
        }

        [TestMethod]
        public void ErrorIsSignedCorrectly()
        {
            xml.LoadXml(Resources.Error);
            xml.Sign(certificate, uris, noAttachments);
            xml.VerifySignature(noAttachments);
        }

        [TestMethod]
        public void PullRequestIsSignedCorrectly()
        {
            xml.LoadXml(Resources.PullRequest);
            xml.Sign(certificate, uris, noAttachments);
            xml.VerifySignature(noAttachments);
        }

        [TestMethod]
        public void UserMessageIsSignedCorrectly()
        {
            xml.LoadXml(Resources.UserMessage);
            xml.Sign(certificate, uris, noAttachments);
            xml.VerifySignature(noAttachments);
        }

        [TestMethod]
        public void UserMessageWithAttachmentIsSignedCorrectly()
        {
            xml.LoadXml(Resources.UserMessage);
            xml.Sign(certificate, uris, Attachments.Generate(2));
            xml.VerifySignature(Attachments.Generate(2));
        }
    }
}
