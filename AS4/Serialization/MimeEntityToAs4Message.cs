using System.Linq;
using System.Xml;
using AS4.Soap;
using MimeKit;

namespace AS4.Serialization
{
    public class MimeEntityToAs4Message
    {
        public static As4Message Deserialize(MimeEntity mimeEntity)
        {   
            var mimePart = mimeEntity as MimePart;
            if (mimePart != null)
            {
                return ParseMimePart(mimePart);
            }

            var multipart = mimeEntity as Multipart;
            if (multipart != null)
            {
                return ParseMultipart(multipart);
            }

            return null;
        }
        
        public static As4Message ParseMimePart(MimePart mimePart)
        {
            return new As4Message
            {
                SoapEnvelope = ParseXml(mimePart)
            };
        }

        public static As4Message ParseMultipart(Multipart multipart)
        {
            var message = new As4Message();

            var mimeParts = multipart.Cast<MimePart>();

            foreach (var part in mimeParts)
            {
                if (part.IsAttachment)
                {
                    message.Attachments.Add(ParseAttachment(part));
                }
                else
                {
                    message.SoapEnvelope = ParseXml(part);
                }
            }

            return message;
        }
        
        public static XmlDocument ParseXml(MimePart mimePart)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(mimePart.Content.Stream);
            return xmlDocument;
        }

        public static Attachment ParseAttachment(MimePart mimePart)
        {
            return new Attachment
            {
                ContentType = mimePart.ContentType.MimeType,
                ContentId = mimePart.ContentId,
                Stream = mimePart.Content.Stream
            };
        }
    }
}
