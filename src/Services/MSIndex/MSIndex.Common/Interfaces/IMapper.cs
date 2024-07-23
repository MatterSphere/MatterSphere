using System.Dynamic;

namespace MSIndex.Common.Interfaces
{
    public interface IMapper
    {
        void Map<T>(ExpandoObject source, T destination) where T : class;
    }
}
