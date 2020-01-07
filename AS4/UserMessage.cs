using System;

namespace AS4
{
    public class UserMessage
    {
        public DateTime Timestamp { get; set; }
        public string MessageId { get; set; }
        public string SenderId { get; set; }
        public string SenderRole { get; set; }
        public string ReceiverId { get; set; }
        public string ReceiverRole { get; set; }
        public string ConversationId { get; set; }
    }
}
