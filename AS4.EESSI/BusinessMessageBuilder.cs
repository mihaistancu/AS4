using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using AS4.EESSI.Endpoints;
using AS4.EESSI.Security;
using AS4.Soap;

namespace AS4.EESSI
{
    public class BusinessMessageBuilder
    {
        public IParticipant Sender { get; set; }
        public IParticipant Receiver { get; set; }
        public string UseCase { get; set; }
        public Attachment Sed { get; set; }
        public List<Attachment> Attachments { get; set; }

        public BusinessMessageBuilder()
        {
            Attachments = new List<Attachment>();
        }

        public As4Message Build()
        {
            var envelope = CreateSoapEnvelope();
            envelope.Header.Messaging.UserMessage.PartyInfo.From.PartyId.Value = Sender.Code;
            envelope.Header.Messaging.UserMessage.PartyInfo.To.PartyId.Value = Receiver.Code;
            envelope.Header.Messaging.UserMessage.CollaborationInfo.Service.Value = UseCase;

            envelope.Header.Messaging.UserMessage.PayloadInfo = new List<PartInfo>();

            if (Sed != null)
            {
                var sedPart = new PartInfo
                {
                    Reference = $"cid:{Sed.ContentId}",
                    PartProperties = new List<Property>
                    {
                        new Property {Name = "PartType", Value = "SED"},
                        new Property {Name = "MimeType", Value = Sed.ContentType}
                    }
                };

                if (Sed.IsCompressionRequired)
                {
                    Sed.Stream = Compress(Sed.Stream);
                    sedPart.PartProperties.Add(new Property
                    {
                        Name = "CompressionType", Value = "application/gzip"
                    });
                }
                
                envelope.Header.Messaging.UserMessage.PayloadInfo.Add(sedPart);
            }
            
            foreach (var attachment in Attachments)
            {
                if (attachment.IsCompressionRequired)
                {
                    attachment.Stream = Compress(attachment.Stream);
                }

                var attachmentPart = new PartInfo
                {
                    Reference = $"cid:{attachment.ContentId}",
                    PartProperties = new List<Property>
                    {
                        new Property {Name = "PartType", Value = "Attachment"},
                        new Property {Name = "MimeType", Value = attachment.ContentType},
                    }
                };

                if (attachment.IsCompressionRequired)
                {
                    attachment.Stream = Compress(attachment.Stream);
                    attachmentPart.PartProperties.Add(new Property
                    {
                        Name = "CompressionType", Value = "application/gzip"
                    });
                }
                envelope.Header.Messaging.UserMessage.PayloadInfo.Add(attachmentPart);
            }
            
            var message= new As4Message();
            message.Set(envelope);
            message.Attachments.Add(Sed);

            Sed.Stream.Position = 0;
            var ebmsSigner = new EbmsSigner
            {
                Xml = message.SoapEnvelope,
                Certificate = Sender.Ebms, 
                Uris = new [] {envelope.Header.Messaging.Id, envelope.Body.Id},
                Attachments = message.Attachments
            };
            ebmsSigner.Sign();
            
            return message;
        }

        private Stream Compress(Stream source)
        {
            source.Position = 0;
            var destination = new MemoryStream();
            using (var gzip = new GZipStream(destination, CompressionMode.Compress, true))
            {
                Sed.Stream.CopyTo(gzip);
            }
            return destination;
        }

        private Envelope CreateSoapEnvelope()
        {
            return new Envelope
            {
                Header = new Header
                {
                    Messaging = new Messaging
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserMessage = new UserMessage
                        {
                            MessageInfo = new MessageInfo
                            {
                                Timestamp = DateTime.Now,
                                MessageId = Guid.NewGuid() + "@" + Environment.MachineName
                            },
                            PartyInfo = new PartyInfo
                            {
                                From = new Party
                                {
                                    PartyId = new PartyId
                                    {
                                        Type = "urn:eu:europa:ec:dgempl:eessi:ir",
                                        Value = string.Empty
                                    },
                                    Role = "urn:eu:europa:ec:dgempl:eessi:ir:institution"
                                },
                                To = new Party
                                {
                                    PartyId = new PartyId
                                    {
                                        Type = "urn:eu:europa:ec:dgempl:eessi:ir",
                                        Value = string.Empty
                                    },
                                    Role = "urn:eu:europa:ec:dgempl:eessi:ir:institution"
                                }
                            },
                            CollaborationInfo = new CollaborationInfo
                            {
                                Service = new Service
                                {
                                    Type = "urn:eu:europa:ec:dgempl:eessi",
                                    Value = string.Empty
                                },
                                Action = "Send",
                                ConversationId = Guid.NewGuid().ToString()
                            },
                            
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
