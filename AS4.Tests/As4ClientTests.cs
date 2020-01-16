using AS4.Serialization;
using AS4.Tests.Factories;
using Microsoft.Owin;
using Microsoft.Owin.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Owin;
using System;
using System.Threading.Tasks;

namespace AS4.Tests
{
    [TestClass]
    public class As4ClientTests
    {
        [TestMethod]
        public void Bar()
        {
            string baseUrl = "http://localhost:12346/";
            using (WebApp.Start(baseUrl, app => app.Run(Foo)))
            {
                var client = new As4Client();
            
                var message = new As4Message();
                message.Set(Envelopes.UserMessage);
            
                var result = client.Send(new Uri(baseUrl), message);
            }
        }

        private Task Foo(IOwinContext context)
        {
            var message = new As4Message();
            message.Set(Envelopes.Receipt);
            var mimeMessage = As4MessageToMimeEntity.Serialize(message);
            context.Response.ContentType = mimeMessage.ContentType.MimeType + mimeMessage.ContentType.Parameters;
            return mimeMessage.WriteToAsync(context.Response.Body, true);
        }
    }
}