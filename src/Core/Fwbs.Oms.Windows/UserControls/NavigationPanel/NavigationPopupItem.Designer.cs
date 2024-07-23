using System.Drawing;

namespace FWBS.Common.UI.Windows
{
    partial class NavigationPopupItem
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // NavigationPopupItem
            // 
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Dock = System.Windows.Forms.DockStyle.Top;
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Click += new System.EventHandler(this.Item_Clicked);
            this.MouseLeave += new System.EventHandler(this.NavigationPopupItem_MouseLeave);
            this.MouseHover += new System.EventHandler(this.NavigationPopupItem_MouseHover);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
