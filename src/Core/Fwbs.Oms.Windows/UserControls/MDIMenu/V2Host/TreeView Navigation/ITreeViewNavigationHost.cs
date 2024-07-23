namespace FWBS.OMS.UI.Windows
{
    public interface ITreeViewNavigationHost
    {
        Telerik.WinControls.UI.RadTreeView TreeView { get; }

        Infragistics.Win.UltraWinTabControl.UltraTabControl TabControl { get; }

        TreeNavigationActions Actions { get; }

        void RefreshFavorites();

        void RefreshLast10();
    }
}
