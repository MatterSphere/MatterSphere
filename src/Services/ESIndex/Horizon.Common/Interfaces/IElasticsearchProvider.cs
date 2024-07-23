namespace Horizon.Common.Interfaces
{
    public interface IElasticsearchProvider
    {
        bool CheckIndex(string index);
    }
}
