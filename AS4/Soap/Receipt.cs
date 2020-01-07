using System.Xml.Serialization;

namespace AS4.Soap
{
    public class Receipt
    {
        [XmlElement(Namespace = Namespaces.ElectronicBusinessProcess)]
        public NonRepudiationInformation NonRepudiationInformation { get; set; }
    }
}
