using System;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.Common.UI.Windows
{
    public class ucHorizontalNavigationPanelItem : Button, IHorizontalItem
    {
        public event EventHandler onClicked;

        public ucHorizontalNavigationPanelItem()
        {
            InitializeComponent();

            this.Height = LogicalToDeviceUnits(40);
        }

        public string Code { get; set; }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ucHorizontalNavigationPanelItem
            // 
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(0, 255, 255, 255);
            this.FlatAppearance.BorderSize = 0;
            this.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.Margin = new Padding(0);
            this.TextChanged += new System.EventHandler(this.Item_TextChanged);
            this.Click += new System.EventHandler(this.Item_Click);
            this.ResumeLayout(false);

        }

        private void Item_TextChanged(object sender, EventArgs e)
        {
            using (var graph = this.CreateGraphics())
            {
                this.Size = new Size((int)graph.MeasureString(this.Text, this.Font).Width + LogicalToDeviceUnits(24), LogicalToDeviceUnits(40));
            }
        }

        private void Item_Click(object sender, System.EventArgs e)
        {
            onClicked?.Invoke(this, e);
        }

        public void SelectItem()
        {
            _isSelected = true;
            this.Font = new Font("Segoe UI Semibold", Parent.Font.Size);
            this.ForeColor = Color.FromArgb(51, 51, 51);
        }

        public void UnselectItem()
        {
            _isSelected = false;
            this.Font = Parent.Font;
            this.ForeColor = Color.FromArgb(102, 102, 102);
        }

        private bool _isSelected;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (_isSelected)
            {
                SetLine(e);
            }
        }

        private void SetLine(PaintEventArgs e)
        {
            using (var pen = new Pen(Color.FromArgb(21, 101, 192), LogicalToDeviceUnits(2)))
            {
                e.Graphics.DrawLine(pen, new Point(0, Height - LogicalToDeviceUnits(1)), new Point(Width, Height - LogicalToDeviceUnits(1))); 
            }
        }

        protected override void OnDpiChangedAfterParent(EventArgs e)
        {
            base.OnDpiChangedAfterParent(e);
            this.Height = LogicalToDeviceUnits(40);
        }
    }
}
