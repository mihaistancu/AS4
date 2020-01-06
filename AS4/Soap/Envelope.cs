using System.Xml.Serialization;

namespace AS4.Soap
{
    public class Envelope
    {
        [XmlElement(Namespace = Namespaces.SoapEnvelope)]
        public Header Header { get; set; }

        [XmlElement(Namespace = Namespaces.SoapEnvelope)]
        public Body Body { get; set; }
    }
}
