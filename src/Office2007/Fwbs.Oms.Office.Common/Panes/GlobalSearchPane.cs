using System.Windows.Forms;

namespace Fwbs.Oms.Office.Common.Panes
{
    public partial class GlobalSearchPane : BasePane
    {
        public GlobalSearchPane()
        {
            InitializeComponent();
        }

        protected override void InternalRefresh(object activeDoc)
        {
            if (Pane == null)
            {
                Pane = Panes.Add(this, this.globalSearch.Tag.ToString());
                Pane.DockPosition = Microsoft.Office.Core.MsoCTPDockPosition.msoCTPDockPositionRight;
                Pane.DockPositionRestrict = Microsoft.Office.Core.MsoCTPDockPositionRestrict.msoCTPDockPositionRestrictNoHorizontal;
                Pane.Width = LogicalToDeviceUnits(600);
            }

            Pane.Visible = Visible;
        }

        protected override void OnVisibleChanged()
        {
            if (!Visible)
            {
                this.globalSearch.ClearResults();
            }
        }
    }
}
