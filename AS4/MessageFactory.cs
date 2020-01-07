using AS4.Soap;

namespace AS4
{
    public class MessageFactory
    {
        public static Envelope Create(UserMessage userMessage)
        {
            return new Envelope
            {
                Header = new Header
                {
                    Messaging = new Messaging
                    {
                        UserMessage = new Soap.UserMessage
                        {
                            MessageInfo = new MessageInfo
                            {
                                Timestamp = userMessage.Timestamp,
                                MessageId = userMessage.MessageId
                            },
                            PartyInfo = new PartyInfo
                            {
                                From = new Party
                                {
                                    PartyId = new PartyId
                                    {
                                        Type = "urn:eu:europa:ec:dgempl:eessi:ir",
                                        Value = userMessage.SenderId
                                    },
                                    Role = userMessage.SenderRole
                                },
                                To = new Party
                                {
                                    PartyId = new PartyId
                                    {
                                        Type = "urn:eu:europa:ec:dgempl:eessi:ir",
                                        Value = userMessage.ReceiverId
                                    },
                                    Role = userMessage.ReceiverRole
                                }
                            },
                            CollaborationInfo = new CollaborationInfo
                            {
                                Service = new Service
                                {
                                    Type = "urn:eu:europa:ec:dgempl:eessi",
                                    Value = "BusinessMessaging"
                                },
                                Action = "Send",
                                ConversationId = userMessage.ConversationId
                            },
                            PayloadInfo = new PayloadInfo
                            {
                                PartInfo = new PartInfo
                                {
                                    Reference = "cid:DefaultSED",
                                    PartProperties = new[]
                                    {
                                        new Property {Name = "PartType", Value = "SED"},
                                        new Property {Name = "MimeType", Value = "application/xml"},
                                        new Property {Name = "CompressionType", Value = "application/gzip"}
                                    }
                                }
                            }
                        }
                    }
                },
                Body = new Body
                {
                    Id = "body-id"
                }
            };
        }
    }
}
