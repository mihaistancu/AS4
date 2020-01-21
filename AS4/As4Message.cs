using System.Collections.Generic;
using System.Xml;
using AS4.Soap;
using AS4.Serialization;

namespace AS4
{
    public class As4Message
    {
        public XmlDocument SoapEnvelope { get; set; }
        public List<Attachment> Attachments { get; set; }

        public As4Message()
        {
            Attachments = new List<Attachment>();
        }

        public void Set(Envelope envelope)
        {
            SoapEnvelope = ObjectToXml.Serialize(envelope);
        }
    }
}
