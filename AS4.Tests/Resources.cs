using System.IO;
using System.Linq;
using System.Reflection;

namespace AS4.Tests
{
    public class Resources
    {
        public static string Error => Get("Error.xml");
        public static string PullRequest => Get("PullRequest.xml");
        public static string Receipt => Get("Receipt.xml");
        public static string UserMessage => Get("UserMessage.xml");

        private static string Get(string filename)
        {
            var assembly = Assembly.GetExecutingAssembly();

            string resourceName = assembly.GetManifestResourceNames()
                .Single(str => str.EndsWith(filename));

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
