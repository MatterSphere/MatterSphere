namespace FWBS.OMS.UI.Windows
{
    public interface IOBjectDirty
    {
        bool IsDirty { get; }

        bool IsObjectDirty();
    }
}
