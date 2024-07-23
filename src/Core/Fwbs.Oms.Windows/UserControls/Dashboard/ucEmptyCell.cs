using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls;

namespace FWBS.OMS.UI.UserControls.Dashboard
{
    public partial class ucEmptyCell : UserControl, IDashboardItem
    {

        private CellMenuBuilder _menuBuilder;
        private ICellsContainer _cellsContainer;

        public ucEmptyCell()
        {
            InitializeComponent();

            SetIcon();
            WireMouseEvents(this.tlpContainer);
        }

        internal EventHandler<DashboardCell> ItemCreating;

        public Point CellLocation { get; set; }
        public Size CellSize { get; set; }

        public List<Size> AvailableSizes
        {
            get
            {
                return _cellsContainer.GetAvailableSizes(CellLocation);
            }
        }

        internal void SetMenuBuilder(CellMenuBuilder builder)
        {
            _menuBuilder = builder;
        }

        internal void SetCellsContainer(ICellsContainer cellsContainer)
        {
            _cellsContainer = cellsContainer;
        }

        #region UI

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            SetLines(e);
        }
        
        private void plus_DpiChangedAfterParent(object sender, EventArgs e)
        {
            SetIcon();
        }

        private void plus_Click(object sender, EventArgs e)
        {
            ShowPopup();
        }

        private void lblAddModule_Click(object sender, EventArgs e)
        {
            ShowPopup();
        }

        private void tlpContainer_MouseHover(object sender, EventArgs e)
        {
            HighlightCell();
        }

        private void tlpContainer_MouseLeave(object sender, EventArgs e)
        {
            UnHighlightCell();
        }

        private void WireMouseEvents(Control container)
        {
            foreach (Control c in container.Controls)
            {
                c.MouseHover += (s, e) => this.tlpContainer_MouseHover(s, e);
                c.MouseLeave += (s, e) => this.tlpContainer_MouseLeave(s, e);

                WireMouseEvents(c);
            };
        }
        #endregion

        #region Private methods

        private void SetIcon()
        {
            this.plus.Image = FWBS.OMS.UI.Windows.Images.GetCommonIcon(DeviceDpi, "grey_plus");
        }

        private void SetLines(PaintEventArgs e)
        {
            using (var pen = new Pen(Color.FromArgb(151, 151, 151), LogicalToDeviceUnits(2)) { DashStyle = DashStyle.Dash })
            {
                e.Graphics.DrawLine(pen, panel.Bounds.X, panel.Bounds.Y, panel.Bounds.X + panel.Bounds.Width, panel.Bounds.Y);
                e.Graphics.DrawLine(pen, panel.Bounds.X + panel.Bounds.Width, panel.Bounds.Y, panel.Bounds.X + panel.Bounds.Width, panel.Bounds.Y + panel.Bounds.Height);
                e.Graphics.DrawLine(pen, panel.Bounds.X + panel.Bounds.Width, panel.Bounds.Y + panel.Bounds.Height, panel.Bounds.X, panel.Bounds.Y + panel.Bounds.Height);
                e.Graphics.DrawLine(pen, panel.Bounds.X, panel.Bounds.Y + panel.Bounds.Height, panel.Bounds.X, panel.Bounds.Y);
            }
        }

        private void ShowPopup()
        {
            _menuBuilder.Build(ContextMenuStrip, this);
            ContextMenuStrip.PerformLayout();
            ContextMenuStrip.Show(PointToScreen(new Point((Width - ContextMenuStrip.Width) / 2, (Height - ContextMenuStrip.Height) / 2)), ToolStripDropDownDirection.Default);
        }

        private void HighlightCell()
        {
            panel.BackColor = Color.FromArgb(237, 243, 250);
        }

        private void UnHighlightCell()
        {
            panel.BackColor = Color.FromArgb(250, 250, 250);
        }

        internal void InsertItem(object sender, DashboardCell e)
        {
           ItemCreating?.Invoke(this, e);
        }
        #endregion
    }
}
