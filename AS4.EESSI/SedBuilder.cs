using System.IO;
using System.Xml;
using AS4.EESSI.Endpoints;
using AS4.EESSI.Security;
using AS4.Soap;

namespace AS4.EESSI
{
    public class SedBuilder
    {
        public XmlDocument Xml { get; set; }
        public Institution Sender { get;set; }

        public Attachment Build()
        {   
            var sedXml = GetSed(Xml);

            var reader = new XmlNodeReader(sedXml);
            var doc = new XmlDocument {PreserveWhitespace = true};
            doc.Load(reader);

            var xadesSigner = new XadesSigner
            {
                Xml = doc,
                Certificate = Sender.Business
            };
            xadesSigner.Sign();

            Xml.DocumentElement.RemoveChild(sedXml);
            var newNode = Xml.ImportNode(doc.DocumentElement, true);
            Xml.DocumentElement.AppendChild(newNode);
            
            var sedStream = new MemoryStream();
            Xml.Save(sedStream);
            sedStream.Position = 0;

            return new Attachment
            {
                ContentId = "DefaultSED",
                ContentType = "application/xml",
                Stream = sedStream,
                IsCompressionRequired = true
            };
        }

        private XmlElement GetSed(XmlDocument xml)
        {
            foreach (XmlNode child in xml.DocumentElement.ChildNodes)
            {
                var element = child as XmlElement;
                if (element != null && element.LocalName != "StandardBusinessDocumentHeader")
                {
                    return element;
                }
            }

            return null;
        }
    }
}
