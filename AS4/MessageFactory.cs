using AS4.Soap;

namespace AS4
{
    public class MessageFactory
    {
        public static Envelope Create(UserMessageDetails userMessage)
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

        public static Envelope Create(ReceiptDetails receipt)
        {
            return new Envelope
            {
                Header = new Header
                {
                    To = new To
                    {
                        Role = "http://docs.oasis-open.org/ebxml-msg/ebms/v3.0/ns/part2/200811/nextmsh",
                        Value = "http://docs.oasis-open.org/ebxml-msg/ebms/v3.0/ns/part2/200811/icloud"
                    },
                    Action = "http://docs.oasis-open.org/ebxml-msg/ebms/v3.0/ns/core/200704/oneWay.receipt",
                    RoutingInput = new RoutingInput
                    {
                        IsReferenceParameter = true,
                        MustUnderstandSerializedValue = false,
                        Role = "http://docs.oasis-open.org/ebxml-msg/ebms/v3.0/ns/part2/200811/nextmsh",
                        UserMessage = new Soap.UserMessage
                        {
                            MessagePartitionChannel =
                                "http://docs.oasis-open.org/ebxml-msg/ebms/v3.0/ns/core/200704/defaultMPC.receipt",
                            MessageInfo = new MessageInfo
                            {
                                Timestamp = receipt.UserMessage.Timestamp,
                                MessageId = receipt.UserMessage.MessageId
                            },
                            PartyInfo = new PartyInfo
                            {
                                From = new Party
                                {
                                    PartyId = new PartyId
                                    {
                                        Type = "urn:eu:europa:ec:dgempl:eessi:ir",
                                        Value = receipt.UserMessage.SenderId
                                    },
                                    Role = receipt.UserMessage.SenderRole
                                },
                                To = new Party
                                {
                                    PartyId = new PartyId
                                    {
                                        Type = "urn:eu:europa:ec:dgempl:eessi:ir",
                                        Value = receipt.UserMessage.ReceiverId
                                    },
                                    Role = receipt.UserMessage.ReceiverRole
                                }
                            },
                            CollaborationInfo = new CollaborationInfo
                            {
                                Service = new Service
                                {
                                    Type = "urn:eu:europa:ec:dgempl:eessi",
                                    Value = "BusinessMessaging"
                                },
                                Action = "Send.response",
                                ConversationId = receipt.UserMessage.ConversationId
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
                    },
                    Messaging = new Messaging
                    {
                        SignalMessage = new SignalMessage
                        {
                            MessageInfo = new MessageInfo
                            {
                                Timestamp = receipt.Timestamp,
                                MessageId = receipt.MessageId,
                                RefToMessageId = receipt.UserMessage.MessageId
                            },
                            Receipt = new Receipt
                            {
                                NonRepudiationInformation = new NonRepudiationInformation()
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

        public static Envelope Create(ErrorDetails error)
        {
            return new Envelope
            {
                Header = new Header
                {
                    To = new To
                    {
                        Role = "http://docs.oasis-open.org/ebxml-msg/ebms/v3.0/ns/part2/200811/nextmsh",
                        Value = "http://docs.oasis-open.org/ebxml-msg/ebms/v3.0/ns/part2/200811/icloud"
                    },
                    Action = "http://docs.oasis-open.org/ebxml-msg/ebms/v3.0/ns/core/200704/oneWay.error",
                    RoutingInput = new RoutingInput
                    {
                        IsReferenceParameter = true,
                        MustUnderstandSerializedValue = false,
                        Role = "http://docs.oasis-open.org/ebxml-msg/ebms/v3.0/ns/part2/200811/nextmsh",
                        UserMessage = new Soap.UserMessage
                        {
                            MessagePartitionChannel =
                                "http://docs.oasis-open.org/ebxml-msg/ebms/v3.0/ns/core/200704/defaultMPC.error",
                            MessageInfo = new MessageInfo
                            {
                                Timestamp = error.UserMessage.Timestamp,
                                MessageId = error.UserMessage.MessageId
                            },
                            PartyInfo = new PartyInfo
                            {
                                From = new Party
                                {
                                    PartyId = new PartyId
                                    {
                                        Type = "urn:eu:europa:ec:dgempl:eessi:ir",
                                        Value = error.UserMessage.SenderId
                                    },
                                    Role = error.UserMessage.SenderRole
                                },
                                To = new Party
                                {
                                    PartyId = new PartyId
                                    {
                                        Type = "urn:eu:europa:ec:dgempl:eessi:ir",
                                        Value = error.UserMessage.ReceiverId
                                    },
                                    Role = error.UserMessage.ReceiverRole
                                }
                            },
                            CollaborationInfo = new CollaborationInfo
                            {
                                Service = new Service
                                {
                                    Type = "urn:eu:europa:ec:dgempl:eessi",
                                    Value = "BusinessMessaging"
                                },
                                Action = "Send.response",
                                ConversationId = error.UserMessage.ConversationId
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
                    },
                    Messaging = new Messaging
                    {
                        SignalMessage = new SignalMessage
                        {
                            MessageInfo = new MessageInfo
                            {
                                Timestamp = error.Timestamp,
                                MessageId = error.MessageId,
                                RefToMessageId = error.UserMessage.MessageId
                            },
                            Error = new Error
                            {
                                Category = "Content",
                                ErrorCode = error.ErrorCode,
                                Origin = "ebMS",
                                Severity = "failure",
                                ShortDescription = error.ShortDescription,
                                Description = error.Description,
                                ErrorDetail = error.Details
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
