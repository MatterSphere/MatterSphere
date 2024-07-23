using System;
using System.Collections.Generic;

namespace Fwbs.Documents.Preview
{
    public interface IPreviewer : IDisposable
    {
        void PreviewFile(System.IO.FileInfo file);
        void ShowMessage(string message);
        Dictionary<string, string> CultureProperties { get; set; }
        void Load();
        void Unload();

        object UIElement{ get; }
    }
}
