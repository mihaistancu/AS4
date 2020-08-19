using System;
using System.Security.Cryptography.X509Certificates;

namespace AS4.EESSI.Endpoints
{
    public class Institution : IParticipant
    {
        public string Code { get; set; }
        public X509Certificate2 Business { get; set; }
        public X509Certificate2 Tls { get; set; }
        public X509Certificate2 Ebms { get; set; }
        public Uri Uri { get; set; }

        public As4Message Send(AccessPoint accessPoint, As4Message message)
        {
            var client = new As4Client
            {
                Certificate = Tls
            };
            return client.Send(accessPoint.Outbox, message);
        }
    }
}
