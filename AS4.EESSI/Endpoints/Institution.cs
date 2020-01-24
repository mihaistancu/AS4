using System;
using System.Security.Cryptography.X509Certificates;

namespace AS4.EESSI.Endpoints
{
    public class Institution
    {
        public string Code { get; set; }
        public X509Certificate2 Business { get; set; }
        public X509Certificate2 Tls { get; set; }
        public X509Certificate2 Ebms { get; set; }
        public Uri Uri { get; set; }
    }
}
