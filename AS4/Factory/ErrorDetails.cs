using System;

namespace AS4.Factory
{
    public class ErrorDetails
    {
        public DateTime Timestamp { get; set; }
        public string MessageId { get; set; }
        public string ErrorCode { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string Details { get; set; }
        public UserMessageDetails UserMessage { get; set; }
    }
}