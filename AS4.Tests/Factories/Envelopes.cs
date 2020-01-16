using System;
using AS4.Soap;

namespace AS4.Tests.Factories
{
    public class Envelopes
    {
        public static Envelope UserMessage =>
            new Envelope
            {
                Header = new Header
                {
                    Messaging = new Messaging
                    {
                        Id = "messaging-id",
                        UserMessage = new UserMessage
                        {
                            MessageInfo = new MessageInfo
                            {
                                Timestamp = new DateTime(2020, 1, 6),
                                MessageId = "message-id"
                            },
                            PartyInfo = new PartyInfo
                            {
                                From = new Party
                                {
                                    PartyId = new PartyId
                                    {
                                        Type = "urn:eu:europa:ec:dgempl:eessi:ir",
                                        Value = "party-1"
                                    },
                                    Role = "urn:eu:europa:ec:dgempl:eessi:ir:institution"
                                },
                                To = new Party
                                {
                                    PartyId = new PartyId
                                    {
                                        Type = "urn:eu:europa:ec:dgempl:eessi:ir",
                                        Value = "party-2"
                                    },
                                    Role = "urn:eu:europa:ec:dgempl:eessi:ir:institution"
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
                                ConversationId = "conversation-id"
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

        public static Envelope Receipt =>
            new Envelope
            {
                Header = new Header
                {
                    To = new To
                    {
                        Role = Namespaces.NextMessageServiceHandler,
                        Value = Namespaces.Cloud
                    },
                    Action = "http://docs.oasis-open.org/ebxml-msg/ebms/v3.0/ns/core/200704/oneWay.receipt",
                    RoutingInput = new RoutingInput
                    {
                        IsReferenceParameter = true,
                        MustUnderstandSerializedValue = false,
                        Role = Namespaces.NextMessageServiceHandler,
                        UserMessage = new UserMessage
                        {
                            MessagePartitionChannel =
                                "http://docs.oasis-open.org/ebxml-msg/ebms/v3.0/ns/core/200704/defaultMPC.receipt",
                            MessageInfo = new MessageInfo
                            {
                                Timestamp = new DateTime(2020, 1, 5),
                                MessageId = "user-message-id"
                            },
                            PartyInfo = new PartyInfo
                            {
                                From = new Party
                                {
                                    PartyId = new PartyId
                                    {
                                        Type = "urn:eu:europa:ec:dgempl:eessi:ir",
                                        Value = "party-1"
                                    },
                                    Role = "urn:eu:europa:ec:dgempl:eessi:ir:institution"
                                },
                                To = new Party
                                {
                                    PartyId = new PartyId
                                    {
                                        Type = "urn:eu:europa:ec:dgempl:eessi:ir",
                                        Value = "party-2"
                                    },
                                    Role = "urn:eu:europa:ec:dgempl:eessi:ir:institution"
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
                                ConversationId = "conversation-id"
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
                        Id = "messaging-id",
                        SignalMessage = new SignalMessage
                        {
                            MessageInfo = new MessageInfo
                            {
                                Timestamp = new DateTime(2020, 01, 06),
                                MessageId = "receipt-message-id",
                                RefToMessageId = "user-message-id"
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

        public static Envelope Error =>
            new Envelope
            {
                Header = new Header
                {
                    To = new To
                    {
                        Role = Namespaces.NextMessageServiceHandler,
                        Value = Namespaces.Cloud
                    },
                    Action = "http://docs.oasis-open.org/ebxml-msg/ebms/v3.0/ns/core/200704/oneWay.error",
                    RoutingInput = new RoutingInput
                    {
                        IsReferenceParameter = true,
                        MustUnderstandSerializedValue = false,
                        Role = Namespaces.NextMessageServiceHandler,
                        UserMessage = new UserMessage
                        {
                            MessagePartitionChannel =
                                "http://docs.oasis-open.org/ebxml-msg/ebms/v3.0/ns/core/200704/defaultMPC.error",
                            MessageInfo = new MessageInfo
                            {
                                Timestamp = new DateTime(2020, 1, 5),
                                MessageId = "user-message-id"
                            },
                            PartyInfo = new PartyInfo
                            {
                                From = new Party
                                {
                                    PartyId = new PartyId
                                    {
                                        Type = "urn:eu:europa:ec:dgempl:eessi:ir",
                                        Value = "party-1"
                                    },
                                    Role = "urn:eu:europa:ec:dgempl:eessi:ir:institution"
                                },
                                To = new Party
                                {
                                    PartyId = new PartyId
                                    {
                                        Type = "urn:eu:europa:ec:dgempl:eessi:ir",
                                        Value = "party-2"
                                    },
                                    Role = "urn:eu:europa:ec:dgempl:eessi:ir:institution"
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
                                ConversationId = "conversation-id"
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
                        Id = "messaging-id",
                        SignalMessage = new SignalMessage
                        {
                            MessageInfo = new MessageInfo
                            {
                                Timestamp = new DateTime(2020, 1, 6),
                                MessageId = "error-message-id",
                                RefToMessageId = "user-message-id"
                            },
                            Error = new Error
                            {
                                Category = "Content",
                                ErrorCode = "EBMS:0004",
                                Origin = "ebMS",
                                Severity = "failure",
                                ShortDescription = "Other",
                                Description = "error description",
                                ErrorDetail = "error detail"
                            }
                        }
                    }
                },
                Body = new Body
                {
                    Id = "body-id"
                }
            };

        public static Envelope PullRequest =>
            new Envelope
            {
                Header = new Header
                {
                    Messaging = new Messaging
                    {
                        Id = "messaging-id",
                        SignalMessage = new SignalMessage
                        {
                            MessageInfo = new MessageInfo
                            {
                                Timestamp = new DateTime(2020, 1, 6),
                                MessageId = "message-id"
                            },
                            PullRequest = new PullRequest
                            {
                                MessagePartitionChannel = "mpc-id"
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
