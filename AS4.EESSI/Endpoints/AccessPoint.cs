using System;
using System.Security.Cryptography.X509Certificates;

namespace AS4.EESSI.Endpoints
{
    public class AccessPoint
    {
        public string Code { get; set; }
        public X509Certificate2 TlsExternal { get; set; }
        public X509Certificate2 TlsInternal { get; set; }
        public X509Certificate2 Ebms { get; set; }
        public Uri Uri { get; set; }
    }
}
