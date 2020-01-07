using System;

namespace AS4
{
    public class ReceiptDetails
    {
        public DateTime Timestamp { get; set; }
        public string MessageId { get; set; }
        public UserMessageDetails UserMessage { get; set; }
    }
}
