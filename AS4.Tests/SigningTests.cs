using System;
using AS4.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AS4.Tests
{
    [TestClass]
    public class SigningTests
    {
        [TestMethod]
        public void SigningDoesNotThrowException()
        {
            var userMessage = new UserMessageDetails
            {
                Timestamp = new DateTime(2020, 1, 5),
                MessageId = "user-message-id",
                SenderId = "party-1",
                SenderRole = "urn:eu:europa:ec:dgempl:eessi:ir:institution",
                ReceiverId = "party-2",
                ReceiverRole = "urn:eu:europa:ec:dgempl:eessi:ir:institution",
                ConversationId = "conversation-id"
            };

            var receipt = new ReceiptDetails
            {
                Timestamp = new DateTime(2020, 1, 6),
                MessageId = "receipt-message-id",
                UserMessage = userMessage
            };

            var message = MessageFactory.Create(receipt);
            
            var cert = new Certificate(@"c:\users\Mihai\Desktop\ebms.pfx", "1234");
            var signature = cert.Sign(message);
        }
        
    }
}
