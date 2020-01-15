using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace AS4.Security
{
    public class SignedXmlWithNamespacedIdAndAttachments : SignedXml
    {
        public SignedXmlWithNamespacedIdAndAttachments(XmlDocument xml) : base(xml)
        {
            CryptoConfig.AddAlgorithm(typeof(AttachmentContentSignatureTransform), Namespaces.AttachmentTransform);

            SafeCanonicalizationMethods.Add(Namespaces.AttachmentTransform);
        }

        public void AddExternalReference(Reference reference)
        {
            var existing = GetReferenceByUri(SignedInfo, reference.Uri);

            if (existing != null)
            {
                reference.DigestValue = existing.DigestValue;
                SignedInfo.References.Remove(existing);
            }

            SignedInfo.AddReference(reference);
        }

        public override XmlElement GetIdElement(XmlDocument doc, string id)
        {
            var namespaceManager = new XmlNamespaceManager(doc.NameTable);
            namespaceManager.AddNamespace("wsu", Namespaces.WebServiceSecurityUtility);
            var xpath = $@"//*[@wsu:Id=""{id}""]";
            return  doc.SelectSingleNode(xpath, namespaceManager) as XmlElement;
        }

        private Reference GetReferenceByUri(SignedInfo signedInfo, string uri)
        {
            return signedInfo.References.Cast<Reference>().FirstOrDefault(reference => reference.Uri == uri);
        }
    }
}
