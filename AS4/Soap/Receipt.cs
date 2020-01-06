using System.Xml.Serialization;

namespace AS4.Soap
{
    public class Receipt
    {
        [XmlElement(Namespace = Namespaces.Ebbp)]
        public NonRepudiationInformation NonRepudiationInformation { get; set; }
    }
}
