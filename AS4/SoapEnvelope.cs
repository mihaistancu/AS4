using System.Xml.Serialization;

namespace AS4
{
    [XmlType(TypeName="Envelope", Namespace = Namespace)]
    public class SoapEnvelope
    {
        public const string Namespace = "http://www.w3.org/2003/05/soap-envelope";

        public SoapHeader Header { get; set; }
        public SoapBody Body { get; set; }
    }
}
