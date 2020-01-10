using System.Xml.Serialization;

namespace AS4.Soap
{   
    public class Security
    {
        [XmlAttribute(AttributeName = "mustUnderstand", Namespace = Namespaces.SoapEnvelope)]
        public bool MustUnderstand {get; set; }

        [XmlElement(Namespace = Namespaces.WebServiceSecurityExtensions)]
        public BinarySecurityToken BinarySecurityToken { get; set; }
    }
}
