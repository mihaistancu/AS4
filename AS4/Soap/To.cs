using System.Xml.Serialization;

namespace AS4.Soap
{
    public class To
    {
        [XmlAttribute(AttributeName = "role", Namespace = Namespaces.SoapEnvelope)]
        public string Role { get; set; }

        [XmlText]
        public string Value {get; set; }
    }
}
