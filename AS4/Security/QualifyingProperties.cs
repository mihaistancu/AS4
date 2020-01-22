using System.Xml.Serialization;
using AS4.Soap;

namespace AS4.Security
{
    [XmlRoot(Namespace = Namespaces.AdvancedElectronicSignature)]
    public class QualifyingProperties
    {
        [XmlAttribute]
        public string Target { get; set; }

        public SignedProperties SignedProperties { get; set; }
    }
}
