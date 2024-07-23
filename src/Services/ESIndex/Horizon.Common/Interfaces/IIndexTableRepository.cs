namespace Horizon.Common.Interfaces
{
    public interface IIndexTableRepository
    {
        bool GetObjectTypeFullCopyRequired(string objectTypeName);
        void SetObjectTypeFullCopyRequired(string objectTypeName, bool fullCopyRequired);
    }
}
