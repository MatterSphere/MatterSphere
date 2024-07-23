namespace Fwbs.Oms.Office.Outlook
{
    partial class ucFolderPage
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
            this.ucOMSTypeBrowser1 = new FWBS.OMS.UI.UserControls.ucOMSTypeBrowser();
            this.SuspendLayout();
            // 
            // ucOMSTypeBrowser1
            // 
            this.ucOMSTypeBrowser1.BrowserSelectedValue = "";
            this.ucOMSTypeBrowser1.BrowserVisible = true;
            this.ucOMSTypeBrowser1.DefaultVisible = false;
            this.ucOMSTypeBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucOMSTypeBrowser1.Location = new System.Drawing.Point(3, 3);
            this.ucOMSTypeBrowser1.Name = "ucOMSTypeBrowser1";
            this.ucOMSTypeBrowser1.ResetViewVisible = true;
            this.ucOMSTypeBrowser1.Size = new System.Drawing.Size(234, 138);
            this.ucOMSTypeBrowser1.TabIndex = 1;
            this.ucOMSTypeBrowser1.TypeCode = "";
            this.ucOMSTypeBrowser1.ResetViewClick += new System.EventHandler(this.ucOMSTypeBrowser1_ResetViewClick);
            this.ucOMSTypeBrowser1.BrowserChanged += new System.EventHandler(this.ucOMSTypeBrowser1_BrowserChanged);
            // 
            // ucFolderPage
            // 
            this.Controls.Add(this.ucOMSTypeBrowser1);
            this.Name = "ucFolderPage";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Size = new System.Drawing.Size(240, 144);
            this.ResumeLayout(false);

        }

 

        #endregion

        private FWBS.OMS.UI.UserControls.ucOMSTypeBrowser ucOMSTypeBrowser1;
    }
}
