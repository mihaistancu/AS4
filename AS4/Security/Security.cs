using System.Xml.Serialization;
using AS4.Soap;

namespace AS4.Security
{
    [XmlRoot(Namespace = Namespaces.WebServiceSecurityExtensions)]
    public class Security
    {
        public BinarySecurityToken BinarySecurityToken { get; set; }
    }
}
