using System.IO;

namespace Models.Interfaces
{
    public interface IBlacklistValidator
    {
        bool FindFile(FileInfo fileInfo);
    }
}
