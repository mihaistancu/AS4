using AS4.Serialization;
using AS4.Soap;
using AS4.Tests.Asserts;
using AS4.Tests.Factories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AS4.Tests
{
    [TestClass]
    public class XmlSerializationTests
    {
        [TestMethod]
        public void PullRequestSerializesCorrectly()
        {
            CheckSerialization(Envelopes.PullRequest);
        }

        [TestMethod]
        public void ReceiptSerializesCorrectly()
        {
            CheckSerialization(Envelopes.Receipt);
        }

        [TestMethod]
        public void ErrorSerializesCorrectly()
        {
            CheckSerialization(Envelopes.Error);
        }
        
        [TestMethod]
        public void UserMessageSerializesCorrectly()
        {
            CheckSerialization(Envelopes.UserMessage);
        }

        private void CheckSerialization(Envelope expected)
        {
            var xml = EnvelopeToXml.Serialize(expected);
            var actual = XmlToEnvelope.Deserialize(xml);
            EnvelopeAssert.AreEqual(expected, actual);
        }
    }
}
