using System;
using System.Drawing;
using System.Windows.Forms;
using FWBS.OMS.UI.Windows;

namespace FWBS.OMS.UI.UserControls.ContextMenu
{
    public class ContextMenuPopup : UserControl
    {
        public ContextMenuPopup()
        {
            InitializeComponent();
            DefaultMinWidth = 100;
        }

        public int DefaultMinWidth { get; set; }

        public void AddItem(Control item)
        {
            Controls.Add(item, true);
            var maxSize = GetMaxSize();
            var minWidth = Math.Max(LogicalToDeviceUnits(DefaultMinWidth), maxSize.Width);
            MinimumSize = new Size(minWidth, 0);
        }

        private void CellPopup_DpiChangedAfterParent(object sender, System.EventArgs e)
        {
            base.OnDpiChangedAfterParent(e);
            var maxSize = GetMaxSize();
            MinimumSize = new Size(maxSize.Width, 0);
        }

        private Size GetMaxSize()
        {
            var maxWidth = 0;
            var maxHeight = 0;
            foreach (Control item in Controls)
            {
                var size = TextRenderer.MeasureText(item.Text, item.Font);
                maxWidth = System.Math.Max(size.Width + LogicalToDeviceUnits(30) + this.Padding.Left + this.Padding.Right, maxWidth);
                maxHeight = System.Math.Min(size.Height, maxHeight);
            }

            return new Size(maxWidth, maxHeight);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.BackColor = System.Drawing.Color.White;
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(100, 0);
            this.Name = "ucContextMenuPopup";
            this.Padding = new Padding(10);
            this.Size = new System.Drawing.Size(100, 96);
            this.DpiChangedAfterParent += new System.EventHandler(this.CellPopup_DpiChangedAfterParent);
            this.ResumeLayout(false);
        }

        #endregion
    }
}
