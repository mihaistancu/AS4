using System;

namespace AS4.Soap
{
    public class MessageInfo
    {
        public DateTime Timestamp { get; set; }
        public string MessageId { get; set; }
        public string RefToMessageId { get; set; }
    }
}
