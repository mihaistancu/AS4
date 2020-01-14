using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace AS4.Security
{
    public class SignedXmlWithNamespacedId : SignedXml
    {
        public SignedXmlWithNamespacedId(XmlDocument xml) : base(xml)
        {
            CryptoConfig.AddAlgorithm(typeof(AttachmentContentSignatureTransform), Namespaces.AttachmentTransform);

            SafeCanonicalizationMethods.Add(Namespaces.AttachmentTransform);
        }
        
        public override XmlElement GetIdElement(XmlDocument doc, string id)
        {
            var namespaceManager = new XmlNamespaceManager(doc.NameTable);
            namespaceManager.AddNamespace("wsu", Namespaces.WebServiceSecurityUtility);
            var xpath = $@"//*[@wsu:Id=""{id}""]";
            return  doc.SelectSingleNode(xpath, namespaceManager) as XmlElement;
        }
    }
}
