using System.IO;
using MimeKit;

namespace AS4.Serialization
{
    public class As4MessageToMimeEntity
    {
        public static MimeEntity Serialize(As4Message message)
        {
            var stream = new MemoryStream();
            message.SoapEnvelope.Save(stream);

            var mimeRoot = new MimePart("application", "soap+xml")
            {
                Content = new MimeContent(stream)
            };

            if (message.Attachments.Count == 0)
            {
                return mimeRoot;
            }

            var multipart = new Multipart("related") {mimeRoot};
            
            foreach (var messageAttachment in message.Attachments)
            {
                var mimeAttachment = new MimePart(messageAttachment.ContentType)
                {
                    ContentId = messageAttachment.ContentId,
                    Content = new MimeContent(messageAttachment.Stream),
                    ContentDisposition = new ContentDisposition (ContentDisposition.Attachment),
                    ContentTransferEncoding = ContentEncoding.Binary
                };
                multipart.Add(mimeAttachment);
            }

            return multipart;
        }
    }
}
