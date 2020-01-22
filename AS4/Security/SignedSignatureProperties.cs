using System;

namespace AS4.Security
{
    public class SignedSignatureProperties
    {
        public DateTime SigningTime { get; set; }

        public SigningCertificate SigningCertificate { get; set; }
    }
}
