namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar
{
    partial class ucCalendarPickerPopupItem
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel = new System.Windows.Forms.Panel();
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(218, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Title";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblTitle.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PopupItem_MouseClick);
            this.lblTitle.MouseLeave += new System.EventHandler(this.PopupItem_MouseLeave);
            this.lblTitle.MouseHover += new System.EventHandler(this.PopupItem_MouseHover);
            // 
            // panel
            // 
            this.panel.Controls.Add(this.lblTitle);
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Location = new System.Drawing.Point(12, 0);
            this.panel.Margin = new System.Windows.Forms.Padding(0);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(218, 32);
            this.panel.TabIndex = 1;
            // 
            // ucCalendarPickerPopupItem
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "ucCalendarPickerPopupItem";
            this.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.Size = new System.Drawing.Size(230, 32);
            this.panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        protected System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel;

        #endregion
    }
}
