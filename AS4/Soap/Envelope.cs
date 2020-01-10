using System.Xml.Serialization;

namespace AS4.Soap
{
    [XmlRoot(Namespace = Namespaces.SoapEnvelope)]
    public class Envelope
    {
        [XmlElement(Namespace = Namespaces.SoapEnvelope)]
        public Header Header { get; set; }

        [XmlElement(Namespace = Namespaces.SoapEnvelope)]
        public Body Body { get; set; }
    }
}
