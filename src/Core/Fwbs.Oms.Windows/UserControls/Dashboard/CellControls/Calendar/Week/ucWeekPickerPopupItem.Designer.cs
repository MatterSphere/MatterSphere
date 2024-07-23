namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Week
{
    partial class ucWeekPickerPopupItem
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
            this.panel = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel
            // 
            this.panel.Controls.Add(this.lblTitle);
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Location = new System.Drawing.Point(0, 0);
            this.panel.Margin = new System.Windows.Forms.Padding(0);
            this.panel.Name = "panel";
            this.panel.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.panel.Size = new System.Drawing.Size(155, 32);
            this.panel.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Location = new System.Drawing.Point(5, 0);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(150, 32);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Title";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblTitle.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lblTitle_MouseClick);
            this.lblTitle.MouseLeave += new System.EventHandler(this.lblTitle_MouseLeave);
            this.lblTitle.MouseHover += new System.EventHandler(this.lblTitle_MouseHover);
            // 
            // ucWeekPickerPopupItem
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panel);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(212)))));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ucWeekPickerPopupItem";
            this.Size = new System.Drawing.Size(155, 32);
            this.panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel;
        protected System.Windows.Forms.Label lblTitle;
    }
}
