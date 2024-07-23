using System.IO;
using System.Text;
using System.Xml;
using MSIndex.Common.Interfaces;
using MSIndex.Common.Models;
using Newtonsoft.Json;

namespace MSIndex.Common
{
    public class Converter : IConverter
    {
        public QueueMessage Convert(byte[] message)
        {
            var xmlDocument = new XmlDocument();
            using (MemoryStream memoryStream = new MemoryStream(message))
            {
                xmlDocument.Load(memoryStream);
            }

            return Convert(xmlDocument);
        }

        private QueueMessage Convert(XmlDocument document)
        {
            var builder = new StringBuilder();
            JsonSerializer.Create().Serialize(new CustomJsonWriter(new StringWriter(builder)), document);

            return JsonConvert.DeserializeObject<QueueMessage>(builder.ToString());
        }
    }
}
