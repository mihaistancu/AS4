using System;
using System.IO;
using System.IO.Compression;
using System.Xml;
using AS4.EESSI.Endpoints;
using AS4.EESSI.Security;
using AS4.Soap;

namespace AS4.EESSI
{
    public class BusinessMessageBuilder
    {
        public Institution Sender { get; set; }
        public Institution Receiver { get; set; }
        public Attachment Sed { get; set; }
        
        public As4Message Build()
        {
            var envelope = CreateSoapEnvelope();
            envelope.Header.Messaging.UserMessage.PartyInfo.From.PartyId.Value = Sender.Code;
            envelope.Header.Messaging.UserMessage.PartyInfo.To.PartyId.Value = Receiver.Code;

            
            
            
            Sed.Stream.Position = 0;
            var compressedStream = new MemoryStream();
            using (var gzip = new GZipStream(compressedStream, CompressionMode.Compress, true))
            {
                Sed.Stream.CopyTo(gzip);
            }
            Sed.Stream = compressedStream;

            envelope.Header.Messaging.UserMessage.PayloadInfo = new PayloadInfo
            {
                PartInfo = new PartInfo
                {
                    Reference = $"cid:{Sed.ContentId}",
                    PartProperties = new[]
                    {
                        new Property {Name = "PartType", Value = "SED"},
                        new Property {Name = "MimeType", Value = "application/xml"},
                        new Property {Name = "CompressionType", Value = "application/gzip"}
                    }
                }
            };

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
                                    Value = "BusinessMessaging"
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
