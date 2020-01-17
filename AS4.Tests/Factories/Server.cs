using System;
using System.Threading.Tasks;
using AS4.Serialization;
using Microsoft.Owin;
using Microsoft.Owin.Hosting;
using MimeKit;
using Owin;

namespace AS4.Tests.Factories
{
    public class Server
    {
        public static IDisposable Start(string url, Func<As4Message, As4Message> handler)
        {
            return WebApp.Start(url, app => app.Run(context =>
            {
                var requestMessage = Read(context.Request);
                var responseMessage = handler(requestMessage);
                return WriteAsync(responseMessage, context.Response);
            }));
        }

        public static IDisposable Start(string url, Action<As4Message> handler)
        {
            return WebApp.Start(url, app => app.Run(context =>
            {
                var requestMessage = Read(context.Request);
                handler(requestMessage);
                return Task.CompletedTask;
            }));
        }

        private static As4Message Read(IOwinRequest request)
        {
            var contentType = ContentType.Parse(request.ContentType);
            var requestMime = MimeEntity.Load(contentType, request.Body);
            return MimeEntityToAs4Message.Deserialize(requestMime);
        }

        private static Task WriteAsync(As4Message message, IOwinResponse response)
        {
            var responseMime = As4MessageToMimeEntity.Serialize(message);
            response.ContentType = responseMime.ContentType.MimeType + responseMime.ContentType.Parameters;
            return responseMime.WriteToAsync(response.Body, true);
        }
    }
}
