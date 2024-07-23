using System.IO;
using System.Text;
using System.Xml;
using Newtonsoft.Json;
using XmlConverter.Models;

namespace XmlConverter
{
    public class Converter
    {
        public MultiEntities Convert(byte[] message)
        {
            var xmlDocument = new XmlDocument();
            using (MemoryStream memoryStream = new MemoryStream(message))
            {
                try
                {
                    xmlDocument.Load(memoryStream);
                }
                catch (XmlException)
                {
                }
            }

            return Convert(xmlDocument);
        }

        private MultiEntities Convert(XmlDocument document)
        {
            var builder = new StringBuilder();
            JsonSerializer.Create().Serialize(new CustomJsonWriter(new StringWriter(builder)), document);
            var serialized = builder.ToString();
            if (document.HasChildNodes && document.ChildNodes[0].ChildNodes.Count > 1)
            {
                return JsonConvert.DeserializeObject<MultiEntities>(serialized);
            }

            var entity = JsonConvert.DeserializeObject<SingleEntity>(serialized);
            return new MultiEntities(entity);
        }
    }
}
