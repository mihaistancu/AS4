using System;

namespace AS4.Factory
{
    public class UserMessageDetails
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
