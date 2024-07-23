using System.IO;
using Aspose.Words;

namespace DocumentReader
{
    public class AsposeWordReader : TextReader
    {
        private readonly string _filePath;

        static AsposeWordReader()
        {
            var lic = new Aspose.Words.License();
            lic.SetLicense("Aspose.Total.lic");
        }

        public AsposeWordReader(string filePath)
        {
            _filePath = filePath;
        }

        public override string ReadToEnd()
        {
            string content = string.Empty;
            using (MemoryStream stream = new MemoryStream())
            {
                Document document = new Document(_filePath);
                document.AcceptAllRevisions();
                document.Save(stream, SaveFormat.Text);
                stream.Position = 0;
                using (StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8, true, 1024, true))
                {
                    content = reader.ReadToEnd();
                }
            }

            return content;
        }

        public override string ReadLine()
        {
            throw new System.NotImplementedException();
        }
    }
}
