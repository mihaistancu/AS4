using System;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using AS4.Serialization;
using MimeKit;

namespace AS4
{
    public class As4Client
    {
        public X509Certificate2 Certificate { get; set; }

        public As4Message Send(Uri uri, As4Message message)
        {
            var mimeMessage = As4MessageToMimeEntity.Serialize(message);

            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "POST";
            request.ContentType = mimeMessage.ContentType.MimeType + mimeMessage.ContentType.Parameters;

            if (Certificate != null)
            {
                request.ClientCertificates.Add(Certificate);
            }

            using (Stream requestStream = request.GetRequestStream())
            {
                mimeMessage.WriteTo(requestStream, true);
            }
    
            var response = (HttpWebResponse)request.GetResponse();
            if (string.IsNullOrEmpty(response.ContentType))
            {
                return null;
            }

            var contentType = ContentType.Parse(response.ContentType);

            using (Stream responseStream = response.GetResponseStream())
            {
                var mimeEntity = MimeEntity.Load(contentType, responseStream);
                return MimeEntityToAs4Message.Deserialize(mimeEntity);
            }
        }
    }
}
