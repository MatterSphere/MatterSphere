using System.IO;
using Aspose.Cells;

namespace DocumentReader
{
    public class AsposeExcelReader : TextReader
    {
        private readonly string _filePath;

        static AsposeExcelReader()
        {
            var lic = new Aspose.Cells.License();
            lic.SetLicense("Aspose.Total.lic");
        }

        public AsposeExcelReader(string filePath)
        {
            _filePath = filePath;
        }

        public override string ReadToEnd()
        {
            string content = string.Empty;
            using (MemoryStream stream = new MemoryStream())
            {
                using (Workbook workbook = new Workbook(_filePath))
                {
                    workbook.AcceptAllRevisions();
                    workbook.Save(stream, SaveFormat.Csv);
                    stream.Position = 0;
                }
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
