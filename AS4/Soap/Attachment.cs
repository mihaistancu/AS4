using System.IO;

namespace AS4.Soap
{
    public class Attachment
    {
        public string ContentId { get; set; }
        public string ContentType { get; set; }
        public Stream Stream { get; set; }
    }
}
