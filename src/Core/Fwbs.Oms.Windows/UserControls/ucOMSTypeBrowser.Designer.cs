namespace FWBS.OMS.UI.UserControls
{
    partial class ucOMSTypeBrowser
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
            this.pnlBrowser = new System.Windows.Forms.Panel();
            this.btnDefault = new System.Windows.Forms.Button();
            this.btnResetView = new System.Windows.Forms.Button();
            this.cmbBrowser = new FWBS.OMS.UI.Windows.eXPComboBoxCodeLookup();
            this.pnlBrowser.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBrowser
            // 
            this.pnlBrowser.Controls.Add(this.btnDefault);
            this.pnlBrowser.Controls.Add(this.btnResetView);
            this.pnlBrowser.Controls.Add(this.cmbBrowser);
            this.pnlBrowser.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBrowser.Location = new System.Drawing.Point(0, 0);
            this.pnlBrowser.Name = "pnlBrowser";
            this.pnlBrowser.Padding = new System.Windows.Forms.Padding(0, 6, 7, 7);
            this.pnlBrowser.Size = new System.Drawing.Size(704, 35);
            this.pnlBrowser.TabIndex = 1;
            // 
            // btnDefault
            // 
            this.btnDefault.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnDefault.Location = new System.Drawing.Point(315, 6);
            this.btnDefault.Name = "btnDefault";
            this.btnDefault.Size = new System.Drawing.Size(84, 22);
            this.btnDefault.TabIndex = 3;
            this.btnDefault.Text = "Default";
            this.btnDefault.UseVisualStyleBackColor = true;
            this.btnDefault.Click += new System.EventHandler(this.btnDefault_Click);
            // 
            // btnResetView
            // 
            this.btnResetView.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnResetView.Location = new System.Drawing.Point(231, 6);
            this.btnResetView.Name = "btnResetView";
            this.btnResetView.Size = new System.Drawing.Size(84, 22);
            this.btnResetView.TabIndex = 1;
            this.btnResetView.Text = "Reset View";
            this.btnResetView.UseVisualStyleBackColor = true;
            this.btnResetView.Click += new System.EventHandler(this.btnResetView_Click);
            // 
            // cmbBrowser
            // 
            this.cmbBrowser.ActiveSearchEnabled = true;
            this.cmbBrowser.AddNotSet = false;
            this.cmbBrowser.CaptionWidth = 60;
            this.cmbBrowser.Dock = System.Windows.Forms.DockStyle.Left;
            this.cmbBrowser.IsDirty = false;
            this.cmbBrowser.Location = new System.Drawing.Point(0, 6);
            this.cmbBrowser.MaxLength = 0;
            this.cmbBrowser.Name = "cmbBrowser";
            this.cmbBrowser.NotSetCode = "NOTSET";
            this.cmbBrowser.NotSetType = "RESOURCE";
            this.cmbBrowser.Padding = new System.Windows.Forms.Padding(60, 0, 0, 0);
            this.cmbBrowser.Size = new System.Drawing.Size(231, 22);
            this.cmbBrowser.TabIndex = 2;
            this.cmbBrowser.TerminologyParse = true;
            this.cmbBrowser.Text = "Browser : ";
            this.cmbBrowser.Type = "";
            this.cmbBrowser.ActiveChanged += new System.EventHandler(this.cmbBrowser_ActiveChanged);
            // 
            // ucOMSTypeBrowser
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pnlBrowser);
            this.Name = "ucOMSTypeBrowser";
            this.Size = new System.Drawing.Size(704, 379);
            this.ParentChanged += new System.EventHandler(this.ucOMSTypeBrowser_ParentChanged);
            this.pnlBrowser.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlBrowser;
        private System.Windows.Forms.Button btnResetView;
        private FWBS.OMS.UI.Windows.eXPComboBoxCodeLookup cmbBrowser;
        private System.Windows.Forms.Button btnDefault;
    }
}
