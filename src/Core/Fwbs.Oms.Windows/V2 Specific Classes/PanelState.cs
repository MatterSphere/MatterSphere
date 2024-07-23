using Infragistics.Win.UltraWinDock;

namespace FWBS.OMS.UI.Windows
{
    public interface IPanelState
    {
        DockableControlPane Panel { get; set; }
        bool IsPinned { get; set; }
    }


    internal class PanelState : IPanelState
    {
        public DockableControlPane Panel { get; set; }
        public bool IsPinned { get; set; }

        internal PanelState(DockableControlPane panel, bool isPinned)
        {
            this.Panel = panel;
            this.IsPinned = isPinned;
        }
    }


    internal class NavigationPanelState : PanelState, IPanelState
    {
        public NavigationPanelState(DockableControlPane panel, bool isPinned) : base(panel, isPinned) { }
    }


    internal class InformationPanelState : PanelState, IPanelState
    {
        public InformationPanelState(DockableControlPane panel, bool isPinned) : base(panel, isPinned) { }
    }
}
