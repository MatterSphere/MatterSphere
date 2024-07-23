using MSIndex.Common.Models;

namespace MSIndex.Common.Interfaces
{
    public interface IConverter
    {
        QueueMessage Convert(byte[] message);
    }
}
