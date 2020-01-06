using System.Xml.Serialization;

namespace AS4.Soap
{
    [XmlType(Namespace = Namespaces.SoapEnvelope)]
    public class Envelope
    {
        public Header Header { get; set; }
        public Body Body { get; set; }
    }
}
