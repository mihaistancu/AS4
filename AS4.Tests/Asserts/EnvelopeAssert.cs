using AS4.Soap;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace AS4.Tests.Asserts
{
    public class EnvelopeAssert
    {
        public static void AreEqual(Envelope expected, Envelope actual)
        {
            var expectedJson = JsonConvert.SerializeObject(expected);
            var actualJson = JsonConvert.SerializeObject(actual);
            Assert.AreEqual(expectedJson, actualJson);
        }
    }
}
