using System.IO;

namespace AS4.Mime
{
    public class Attachment
    {
        public string ContentId { get; set; }
        public Stream Stream { get; set; }
    }
}
