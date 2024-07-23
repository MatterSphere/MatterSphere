namespace FWBS.OMS.UI.Windows.Reports
{
    partial class frmOpenReportsRS
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOpenReportsRS));
            this.ucReportsViewRS1 = new FWBS.OMS.UI.Windows.Reports.ucReportsViewRS();
            this.ucFormStorage1 = new FWBS.OMS.UI.Windows.ucFormStorage(this.components);
            this.SuspendLayout();
            // 
            // ucReportsViewRS1
            // 
            this.ucReportsViewRS1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucReportsViewRS1.Location = new System.Drawing.Point(0, 0);
            this.ucReportsViewRS1.Name = "ucReportsViewRS1";
            this.ucReportsViewRS1.Size = new System.Drawing.Size(702, 489);
            this.ucReportsViewRS1.TabIndex = 0;
            // 
            // ucFormStorage1
            // 
            this.ucFormStorage1.DefaultPercentageHeight = 90;
            this.ucFormStorage1.DefaultPercentageWidth = 90;
            this.ucFormStorage1.FormToStore = this;
            this.ucFormStorage1.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ucFormStorage1.UniqueID = "Forms\\OpenRSReports";
            this.ucFormStorage1.Version = ((long)(2));
            // 
            // frmOpenReportsRS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(702, 489);
            this.Controls.Add(this.ucReportsViewRS1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmOpenReportsRS";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Open Report";
            this.ResumeLayout(false);

        }

        #endregion

        private ucFormStorage ucFormStorage1;
        public ucReportsViewRS ucReportsViewRS1;
    }
}