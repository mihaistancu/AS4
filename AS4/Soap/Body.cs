using System.Xml.Serialization;

namespace AS4.Soap
{
    [XmlType(Namespace = Namespace)]
    public class Body
    {
        public const string Namespace = "http://www.w3.org/2003/05/soap-envelope";
    }
}
