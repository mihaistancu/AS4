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
            
            return message;
        }
    }
}
