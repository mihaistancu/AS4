using System.Security.Cryptography.Xml;
using System.Xml;
using AS4.Soap;

namespace AS4.Security
{
    public class SignedXmlWithNamespacedId : SignedXml
    {
        public SignedXmlWithNamespacedId(XmlDocument xml) : base(xml)
        {
        }

        public SignedXmlWithNamespacedId(XmlElement xmlElement) : base(xmlElement)
        {       
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
