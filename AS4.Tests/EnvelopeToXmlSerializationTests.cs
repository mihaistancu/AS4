using AS4.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AS4.Tests
{
    [TestClass]
    public class EnvelopeToXmlSerializationTests
    {
        [TestMethod]
        public void PullRequestSerializesCorrectly()
        {
            var actual = EnvelopeToXml.ToString(Messages.PullRequest);
            var expected = Resources.PullRequest;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ReceiptSerializesCorrectly()
        {
            var actual = EnvelopeToXml.ToString(Messages.Receipt);
            var expected = Resources.Receipt;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ErrorSerializesCorrectly()
        {
            var actual = EnvelopeToXml.ToString(Messages.Error);
            var expected = Resources.Error;
            Assert.AreEqual(expected, actual);
        }
        
        [TestMethod]
        public void UserMessageSerializesCorrectly()
        {
            var actual = EnvelopeToXml.ToString(Messages.UserMessage);
            var expected = Resources.UserMessage;
            Assert.AreEqual(expected, actual);
        }
    }
}
