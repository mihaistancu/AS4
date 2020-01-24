using System;
using System.Security.Cryptography.X509Certificates;
using AS4.EESSI.Security;

namespace AS4.EESSI.Endpoints
{
    public class AccessPoint
    {
        public string Code { get; set; }
        public X509Certificate2 TlsExternal { get; set; }
        public X509Certificate2 TlsInternal { get; set; }
        public X509Certificate2 Ebms { get; set; }
        public Uri Inbound { get; set; }
        public Uri Inbox { get; set; }
        public Uri Outbox { get; set; }

        public As4Message Send(AccessPoint destination, As4Message message)
        {
            var encrypter = new EbmsEncrypter
            {
                Attachments = message.Attachments,
                PublicKeyInAsn1Format = destination.Ebms.GetPublicKey(),
                Xml = message.SoapEnvelope
            };
            encrypter.Encrypt();

            var client = new As4Client
            {
                Certificate = TlsExternal
            };
            return client.Send(destination.Inbound, message);
        }
    }
}
