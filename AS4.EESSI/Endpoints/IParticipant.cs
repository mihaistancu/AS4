using System.Security.Cryptography.X509Certificates;

namespace AS4.EESSI.Endpoints
{
    public interface IParticipant
    {
        string Code { get; set; }
        X509Certificate2 Ebms { get; set; }
    }
}