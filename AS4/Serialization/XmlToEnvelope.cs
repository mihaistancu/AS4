using System.Xml;
using System.Xml.Serialization;
using AS4.Soap;

namespace AS4.Serialization
{
    public class XmlToEnvelope
    {
        public static Envelope Deserialize(XmlDocument xml)
        {
            var serializer = new XmlSerializer(typeof(Envelope));
            using (var reader = new XmlNodeReader(xml))
            {
                return (Envelope)serializer.Deserialize(reader);
            }
        }
    }
}
