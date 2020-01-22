using System.Xml;
using AS4.EESSI.Security;
using AS4.EESSI.Tests.Factories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AS4.EESSI.Tests
{
    [TestClass]
    public class XadesTests
    {
        [TestMethod]
        public void SignAndVerify()
        {
            var xml= new XmlDocument();
            xml.LoadXml(
                @"<sbdh:StandardBusinessDocument xmlns:sbdh=""http://eessi.dgempl.ec.europa.eu/namespaces/sbdh"">
                    <sbdh:StandardBusinessDocumentHeader>
                    </sbdh:StandardBusinessDocumentHeader>
                    <sed:StructuredElectronicDocument xmlns:sed=""http://ec.europa.eu/eessi/ns/sed"">
                    </sed:StructuredElectronicDocument>
                </sbdh:StandardBusinessDocument>");
            
            var xadesSigner = new XadesSigner
            {
                Certificate = Certificates.CreateSelfSigned(),
                Xml = xml
            };
            xadesSigner.Sign();

            var xadesVerifier = new XadesVerifier
            {
                Xml = xml
            };
            var isSignatureCorrect = xadesVerifier.Verify();
            Assert.IsTrue(isSignatureCorrect);
        }
    }
}
