using System.Collections.Generic;
using System.IO;
using AS4.Soap;

namespace AS4.Tests
{
    public class Attachments
    {
        public static IEnumerable<Attachment> Generate(int n)
        {
            for (int i = 0; i < n; i++)
            {
                yield return new Attachment
                {
                    ContentId = "attachment-" + i,
                    ContentType = "application/gzip",
                    Stream = new MemoryStream(new byte[] {0, 1, 2, 3})
                };
            } 
        }
    }
}
