using System.IO;
using MimeKit;

namespace AS4.Serialization
{
    public class As4MessageToStream
    {
        public static void Serialize(As4Message message, Stream stream)
        {
            if (message.Attachments.Count == 0)
            {
                message.SoapEnvelope.Save(stream);
                return;
            }

            var multipart = new Multipart("related");

            var root = new MemoryStream();
            message.SoapEnvelope.Save(root);

            var mimeRoot = new MimePart("application", "soap+xml")
            {   
                Content = new MimeContent(root)
            };
            multipart.Add(mimeRoot);

            foreach (var messageAttachment in message.Attachments)
            {
                messageAttachment.Stream.Position = 0;
                var mimeAttachment = new MimePart(messageAttachment.ContentType)
                {
                    ContentId = messageAttachment.ContentId,
                    Content = new MimeContent(messageAttachment.Stream),
                    ContentDisposition = new ContentDisposition (ContentDisposition.Attachment),
                    ContentTransferEncoding = ContentEncoding.Binary
                };
                multipart.Add(mimeAttachment);
            }
            
            multipart.WriteTo(stream, contentOnly: true);
        }
    }
}
