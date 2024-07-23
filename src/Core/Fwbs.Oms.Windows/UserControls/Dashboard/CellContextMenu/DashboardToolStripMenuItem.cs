using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls;

namespace FWBS.OMS.UI.UserControls.Dashboard
{
    internal partial class DashboardToolStripMenuItem : ToolStripMenuItem
    {

        private bool _isDisabled;

        private bool _isRootItem;

        internal DashboardToolStripMenuItem()
        {
            InitializeComponent();
        }

        internal DashboardToolStripMenuItem(ucEmptyCell ucEmptyCell, DashboardCell dashboardCell) : base(dashboardCell.Description.Replace("&", "&&"))
        {
            InitializeComponent();

            DashboardCell = dashboardCell;
            UcEmptyCell = ucEmptyCell;

            if (DashboardCell != null && UcEmptyCell != null)
            {
                ValidateAvailableSpace();
            }
        }

        internal DashboardToolStripMenuItem(ucEmptyCell ucEmptyCell, string text) : base(text)
        {
            InitializeComponent();
            DashboardCell = null;
            UcEmptyCell = ucEmptyCell;

            _isRootItem = true;
        }

        public DashboardToolStripMenuItem(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        internal DashboardCell DashboardCell { get; }

        internal ucEmptyCell UcEmptyCell { get; }

        public override Size GetPreferredSize(Size proposedSize)
        {
            return new Size(base.GetPreferredSize(proposedSize).Width, UcEmptyCell.LogicalToDeviceUnits(46));
        }

        protected override void OnClick(EventArgs e)
        {
            if (!_isDisabled && !_isRootItem)
            {
                UcEmptyCell.InsertItem(this, DashboardCell);
                base.OnClick(e);
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            Parent.Cursor = _isDisabled ? Cursors.No : Cursors.Hand;
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            Parent.Cursor = Cursors.Default;
            base.OnMouseLeave(e);
        }

        private void ValidateAvailableSpace()
        {
            _isDisabled = !UcEmptyCell.AvailableSizes.Any(s => s.Height == DashboardCell.MinimalSize.Height && s.Width == DashboardCell.MinimalSize.Width);
            if (_isDisabled)
            {
                ToolTipText = CodeLookup.GetLookup("DASHBOARD", "NOSPACE", "Not enough space");
            }
        }

    }
}
