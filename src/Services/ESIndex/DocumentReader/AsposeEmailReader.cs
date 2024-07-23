using System.IO;
using Aspose.Email;

namespace DocumentReader
{
    public class AsposeEmailReader : TextReader
    {
        private readonly string _filePath;

        static AsposeEmailReader()
        {
            var lic = new Aspose.Email.License();
            lic.SetLicense("Aspose.Total.lic");
        }

        public AsposeEmailReader(string filePath)
        {
            _filePath = filePath;
        }

        public override string ReadToEnd()
        {
            string content = string.Empty;
            using (MailMessage message = MailMessage.Load(_filePath))
            {
                content = message.Body;
            }

            return content;
        }

        public override string ReadLine()
        {
            throw new System.NotImplementedException();
        }
    }
}
