using System;
using System.IO;
using OcrDocumentReader;

namespace DocumentReader
{
    public class OcrContentReader : ContentReader
    {
        public OcrContentReader(int timeout) : base(timeout)
        { }

        protected override void Read(string path)
        {
            try
            {
                using (var ocrReader = OcrTextReader.GetOcrTextReader(new FileInfo(path)))
                {
                    _content = ocrReader.GetContent();
                    _completed = true;
                }
            }
            catch (ArgumentOutOfRangeException) { }
        }
    }
}
