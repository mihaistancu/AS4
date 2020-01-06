using System.Xml.Serialization;

namespace AS4
{
    [XmlType(TypeName="Header", Namespace = Namespace)]
    public class SoapHeader
    {
        public const string Namespace = "http://www.w3.org/2003/05/soap-envelope";
    }
}
