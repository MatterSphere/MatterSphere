using System.Collections.Generic;
using Models.Common;

namespace Models.Interfaces
{
    public interface ICacheProvider
    {
        void SaveMessage(Message message);
        IEnumerable<Message> ReadMessages();
        void FailMessage(Message message);
        void ClearCache(Message message);
    }
}
