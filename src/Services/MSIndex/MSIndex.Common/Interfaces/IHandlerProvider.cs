using MSIndex.Common.Handlers;
using MSIndex.Common.Models;

namespace MSIndex.Common.Interfaces
{
    public interface IHandlerProvider
    {
        EntityHandler GetHandler(EntityType entityType);
    }
}
