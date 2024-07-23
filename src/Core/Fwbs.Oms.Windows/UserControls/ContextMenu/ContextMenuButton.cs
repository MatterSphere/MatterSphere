using System;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.UserControls.ContextMenu
{
    public class ContextMenuButton : Button
    {
        private System.Windows.Forms.Label icon;

        public ContextMenuButton()
        {
            InitializeComponent();

            icon.Text = "";
            icon.Visible = false;
            PreferredHeight = 32;
        }

        #region Public methods

        private bool _visibleIcon;
        public bool VisibleIcon
        {
            get { return _visibleIcon; }
            set
            {
                _visibleIcon = value;
                icon.Visible = value;
            }
        }

        public int PreferredHeight { get; set; }

        #endregion

        #region Overrides

        protected override void OnClick(EventArgs e)
        {
            if (!VisibleIcon)
            {
                Infragistics.Win.Misc.UltraPeekPopup.FromControl(Parent)?.Close();
            }
            
            base.OnClick(e);
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            Size size = TextRenderer.MeasureText(this.Text, this.Font);
            size.Width += (VisibleIcon ? icon.Width : 0) + LogicalToDeviceUnits(12);
            size.Height = LogicalToDeviceUnits(PreferredHeight);
            return size;
        }

        #endregion

        #region UI events

        private void icon_Click(object sender, EventArgs e)
        {
            this.OnClick(e);
        }

        private void icon_MouseHover(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(237, 243, 250);
        }

        private void icon_MouseEnter(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(208, 224, 242);
        }

        private void icon_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = Color.Transparent;
        }

        #endregion

        private void InitializeComponent()
        {
            this.icon = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // icon
            // 
            this.icon.BackColor = System.Drawing.Color.Transparent;
            this.icon.Dock = System.Windows.Forms.DockStyle.Right;
            this.icon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.icon.Font = new System.Drawing.Font("Segoe UI Symbol", 10.5F);
            this.icon.Location = new System.Drawing.Point(36, 0);
            this.icon.Margin = new System.Windows.Forms.Padding(0);
            this.icon.Name = "icon";
            this.icon.Size = new System.Drawing.Size(24, 32);
            this.icon.TabIndex = 0;
            this.icon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.icon.Click += new System.EventHandler(this.icon_Click);
            this.icon.MouseEnter += new System.EventHandler(this.icon_MouseEnter);
            this.icon.MouseLeave += new System.EventHandler(this.icon_MouseLeave);
            this.icon.MouseHover += new System.EventHandler(this.icon_MouseHover);
            // 
            // ContextMenuButton
            // 
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.icon);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Dock = System.Windows.Forms.DockStyle.Top;
            this.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.FlatAppearance.BorderSize = 0;
            this.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(224)))), ((int)(((byte)(242)))));
            this.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(243)))), ((int)(((byte)(250)))));
            this.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MaximumSize = new System.Drawing.Size(0, 32);
            this.MinimumSize = new System.Drawing.Size(60, 0);
            this.Size = new System.Drawing.Size(60, 32);
            this.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.UseVisualStyleBackColor = false;
            this.ResumeLayout(false);

        }
    }
}
