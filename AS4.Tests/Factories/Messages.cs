using System.Collections.Generic;
using AS4.Security;
using AS4.Soap;

namespace AS4.Tests.Factories
{
    public class Messages
    {
        public static As4Message Create(Envelope envelope, params Attachment[] attachments)
        {
            var message = new As4Message();
            
            message.Set(envelope);

            foreach (var attachment in attachments)
            {
                message.Attachments.Add(attachment);
            }
            
            var certificate = Certificate.CreateSelfSigned();
            var uris = new List<string> {envelope.Header.Messaging.Id, envelope.Body.Id};
            message.SoapEnvelope.Sign(certificate, uris, message.Attachments);

            return message;
        }
    }
}
