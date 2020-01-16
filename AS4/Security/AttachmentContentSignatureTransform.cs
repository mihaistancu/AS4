using System;
using System.IO;
using System.Security.Cryptography.Xml;
using System.Xml;
using AS4.Soap;

namespace AS4.Security
{
    public class AttachmentContentSignatureTransform: Transform
    {
        private Stream stream;

        public AttachmentContentSignatureTransform()
        {
            Algorithm = Namespaces.AttachmentTransform;
        }

        public override void LoadInnerXml(XmlNodeList nodeList)
        {
        }

        protected override XmlNodeList GetInnerXml()
        {
            return null;
        }

        public override void LoadInput(object obj)
        {
            stream = obj as Stream;
        }

        public override object GetOutput()
        {
            return stream;
        }

        public override object GetOutput(Type type)
        {
            return stream;
        }

        public override Type[] InputTypes => new [] {typeof(Stream)};
        public override Type[] OutputTypes => new[] {typeof(Stream)};
    }
}
