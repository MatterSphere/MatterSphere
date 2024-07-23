namespace FWBS.OMS.UI.Windows.Admin
{
    partial class frmTestOMSTypeBrowser
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
            this.ucOMSTypeBrowser1 = new FWBS.OMS.UI.UserControls.ucOMSTypeBrowser();
            this.SuspendLayout();
            // 
            // ucOMSTypeBrowser1
            // 
            this.ucOMSTypeBrowser1.BrowserSelectedValue = "";
            this.ucOMSTypeBrowser1.BrowserVisible = true;
            this.ucOMSTypeBrowser1.DefaultVisible = true;
            this.ucOMSTypeBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucOMSTypeBrowser1.Location = new System.Drawing.Point(0, 0);
            this.ucOMSTypeBrowser1.Name = "ucOMSTypeBrowser1";
            this.ucOMSTypeBrowser1.ResetViewVisible = true;
            this.ucOMSTypeBrowser1.Size = new System.Drawing.Size(741, 563);
            this.ucOMSTypeBrowser1.TabIndex = 0;
            this.ucOMSTypeBrowser1.TypeCode = "TYPEVIEWS";
            this.ucOMSTypeBrowser1.BrowserChanged += new System.EventHandler(this.ucOMSTypeBrowser1_BrowserChanged);
            this.ucOMSTypeBrowser1.DefaultClick += new System.EventHandler(this.ucOMSTypeBrowser1_DefaultClick);
            // 
            // frmTestOMSTypeBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(741, 563);
            this.Controls.Add(this.ucOMSTypeBrowser1);
            this.Name = "frmTestOMSTypeBrowser";
            this.Text = "frmTestOMSTypeBrowser";
            this.ResumeLayout(false);

        }

        #endregion

        private FWBS.OMS.UI.UserControls.ucOMSTypeBrowser ucOMSTypeBrowser1;
    }
}