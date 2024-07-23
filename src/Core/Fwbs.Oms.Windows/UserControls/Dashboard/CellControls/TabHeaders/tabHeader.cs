using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.TabHeaders
{
    internal class tabHeader : Button
    {
        public tabHeader()
        {
            InitializeComponent();

            this.Height = LogicalToDeviceUnits(40);
        }

        public event EventHandler Clicked;

        public string Code { get; set; }
        public string OmsObjectCode { get; set; }
        public bool IsSelected { get; private set; }

        public string Title { get; private set; }
        public string ShortTitle { get; private set; }
        public int FullSizeWidth { get; private set; }
        public int CompactSizeWidth { get; private set; }

        #region Methods

        public void SetTitle(string title)
        {
            this.Text = title;

            Title = title;
            ShortTitle = string.Concat(title.Where(char.IsUpper));

            using (var graph = this.CreateGraphics())
            {
                FullSizeWidth = (int)graph.MeasureString(Title, this.Font).Width + LogicalToDeviceUnits(24);
                CompactSizeWidth = Math.Max((int)graph.MeasureString(ShortTitle, this.Font).Width + LogicalToDeviceUnits(10), LogicalToDeviceUnits(40));
            }

            this.Width = FullSizeWidth;
        }

        public void SetFullSizeMode()
        {
            this.Text = Title;
            this.Width = FullSizeWidth;
            this.toolTip.RemoveAll();
        }

        public void SetCompactSizeMode()
        {
            this.Text = ShortTitle;
            this.Width = CompactSizeWidth;
            this.toolTip.SetToolTip(this, Title);
        }

        public void SelectItem()
        {
            IsSelected = true;
            this.Font = new Font("Segoe UI Semibold", 10.5F);
            this.ForeColor = Color.FromArgb(51, 51, 51);
        }

        public void UnselectItem()
        {
            IsSelected = false;
            this.Font = new Font("Segoe UI", 10.5F);
            this.ForeColor = Color.FromArgb(102, 102, 102);
        }

        #endregion

        #region UI Events
        

        private void Item_Click(object sender, System.EventArgs e)
        {
            Clicked?.Invoke(this, e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (IsSelected)
            {
                SetLine(e);
            }
           
            using (var pen = new Pen(Color.FromArgb(234, 234, 234), LogicalToDeviceUnits(1)))
            {
                e.Graphics.DrawLine(pen, Width - 1, 0, Width - 1, Height);
            }
        }

        protected override void OnDpiChangedAfterParent(EventArgs e)
        {
            base.OnDpiChangedAfterParent(e);
            this.Height = LogicalToDeviceUnits(40);
        }

        #endregion

        #region Private methods

        private void SetLine(PaintEventArgs e)
        {
            using (var pen = new Pen(Color.FromArgb(21, 101, 192), LogicalToDeviceUnits(2)))
            {
                e.Graphics.DrawLine(pen, 0, Height - LogicalToDeviceUnits(1), Width, Height - LogicalToDeviceUnits(1));
            }
        }

        #endregion

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // ucHorizontalNavigationPanelItem
            // 
            this.AutoEllipsis = true;
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(0, 255, 255, 255);
            this.FlatAppearance.BorderSize = 0;
            this.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.Margin = new Padding(0);
            this.Click += new System.EventHandler(this.Item_Click);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.ToolTip toolTip;
    }
}
