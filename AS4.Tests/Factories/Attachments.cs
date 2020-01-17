using System.IO;
using AS4.Soap;

namespace AS4.Tests.Factories
{
    public class Attachments
    {
        public static Attachment Generate()
        {
            return new Attachment
            {
                ContentId = "attachment-id",
                ContentType = "application/gzip",
                Stream = new MemoryStream(new byte[] {0, 1, 2, 3})
            };
        }
    }
}
