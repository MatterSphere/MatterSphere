using System.IO;

namespace Fwbs.Documents
{
    public interface IRawDocument
    {
        void Open(FileInfo file);
        void Save();
        void Close();
        bool IsOpen { get; }
    }
}
