using System.Xml;
using AS4.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AS4.Tests
{
    [TestClass]
    public class SigningTests
    {
        [TestMethod]
        public void ReceiptIsSignedCorrectly()
        {
            SignAndCheck(Resources.Receipt);
        }

        [TestMethod]
        public void ErrorIsSignedCorrectly()
        {
            SignAndCheck(Resources.Receipt);
        }

        [TestMethod]
        public void PullRequestIsSignedCorrectly()
        {
            SignAndCheck(Resources.Receipt);
        }

        [TestMethod]
        public void UserMessageIsSignedCorrectly()
        {
            SignAndCheck(Resources.Receipt);
        }

        private void SignAndCheck(string content)
        {
            var xml = new XmlDocument();
            xml.LoadXml(content);

            var certificate = Certificate.CreateSelfSigned();
            xml.Sign(certificate, "messaging-id", "body-id");

            xml.VerifySignature();
        }
    }
}
