using AS4.Soap;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AS4.Tests
{
    public class MessageAssert
    {
        public static void AreEqual(As4Message expected, As4Message actual)
        {
            Assert.AreEqual(expected.SoapEnvelope.OuterXml, actual.SoapEnvelope.OuterXml);
            Assert.AreEqual(expected.Attachments.Count, actual.Attachments.Count);
            for (int i = 0; i < expected.Attachments.Count; i++)
            {
                AssertEqual(expected.Attachments[i], actual.Attachments[i]);
            }
        }

        private static void AssertEqual(Attachment expected, Attachment actual)
        {
            Assert.AreEqual(expected.ContentId, actual.ContentId);
            Assert.AreEqual(expected.ContentType, actual.ContentType);
            Assert.AreEqual(expected.Stream.Length, actual.Stream.Length);
        }
    }
}
