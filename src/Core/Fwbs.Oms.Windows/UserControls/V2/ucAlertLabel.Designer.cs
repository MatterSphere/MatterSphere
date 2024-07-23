namespace FWBS.OMS.UI.Windows
{
    partial class ucAlertLabel
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
            this.pbAlertIcon = new System.Windows.Forms.PictureBox();
            this.lblAlert = new System.Windows.Forms.Label();
            this.pnlIcon = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pbAlertIcon)).BeginInit();
            this.pnlIcon.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbAlertIcon
            // 
            this.pbAlertIcon.Location = new System.Drawing.Point(8, 12);
            this.pbAlertIcon.Name = "pbAlertIcon";
            this.pbAlertIcon.Size = new System.Drawing.Size(16, 16);
            this.pbAlertIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbAlertIcon.TabIndex = 0;
            this.pbAlertIcon.TabStop = false;
            // 
            // lblAlert
            // 
            this.lblAlert.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAlert.Location = new System.Drawing.Point(32, 0);
            this.lblAlert.Name = "lblAlert";
            this.lblAlert.Size = new System.Drawing.Size(606, 40);
            this.lblAlert.TabIndex = 2;
            this.lblAlert.Text = "lblAlert";
            this.lblAlert.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlIcon
            // 
            this.pnlIcon.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlIcon.Controls.Add(this.pbAlertIcon);
            this.pnlIcon.Location = new System.Drawing.Point(0, 0);
            this.pnlIcon.Name = "pnlIcon";
            this.pnlIcon.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.pnlIcon.Size = new System.Drawing.Size(32, 40);
            this.pnlIcon.TabIndex = 1;
            // 
            // ucAlertLabel
            // 
            this.Controls.Add(this.pnlIcon);
            this.Controls.Add(this.lblAlert);
            this.Name = "ucAlertLabel";
            this.Padding = new System.Windows.Forms.Padding(32, 0, 0, 0);
            this.Size = new System.Drawing.Size(638, 40);
            ((System.ComponentModel.ISupportInitialize)(this.pbAlertIcon)).EndInit();
            this.pnlIcon.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbAlertIcon;
        private System.Windows.Forms.Label lblAlert;
        private System.Windows.Forms.Panel pnlIcon;
    }
}
