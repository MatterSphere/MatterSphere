namespace FWBS.OMS.UI.DocumentManagement.Addins
{
    partial class frmFolderWalletMappingEditor
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
            this.resourceLookup = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.pnlSelector = new System.Windows.Forms.Panel();
            this.eclDocWallets = new FWBS.OMS.UI.Windows.eCLCollectionSelector();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pnlSelector.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlSelector
            // 
            this.pnlSelector.BackColor = System.Drawing.SystemColors.Control;
            this.pnlSelector.Controls.Add(this.eclDocWallets);
            this.pnlSelector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSelector.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlSelector.Location = new System.Drawing.Point(0, 0);
            this.pnlSelector.Name = "pnlSelector";
            this.pnlSelector.Padding = new System.Windows.Forms.Padding(10);
            this.pnlSelector.Size = new System.Drawing.Size(561, 367);
            this.pnlSelector.TabIndex = 0;
            // 
            // eclDocWallets
            // 
            this.eclDocWallets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.eclDocWallets.IsDirty = true;
            this.eclDocWallets.Location = new System.Drawing.Point(10, 10);
            this.eclDocWallets.Name = "eclDocWallets";
            this.eclDocWallets.Size = new System.Drawing.Size(541, 347);
            this.eclDocWallets.TabIndex = 1;
            this.eclDocWallets.Value = "";
            this.eclDocWallets.ValueSplit = ",";
            // 
            // pnlButtons
            // 
            this.pnlButtons.BackColor = System.Drawing.SystemColors.Control;
            this.pnlButtons.Controls.Add(this.btnOK);
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlButtons.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlButtons.Location = new System.Drawing.Point(561, 0);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(85, 367);
            this.pnlButtons.TabIndex = 1;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(0, 10);
            this.resourceLookup.SetLookup(this.btnOK, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnOK", "&OK", ""));
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 25);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(0, 41);
            this.resourceLookup.SetLookup(this.btnCancel, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnCancel", "Cance&l", ""));
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cance&l";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmFolderWalletMappingEditor
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(646, 367);
            this.ControlBox = false;
            this.Controls.Add(this.pnlSelector);
            this.Controls.Add(this.pnlButtons);
            this.resourceLookup.SetLookup(this, new FWBS.OMS.UI.Windows.ResourceLookupItem("frmFldrWalletMp", "Document Folder / Wallet Mapping Editor", ""));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmFolderWalletMappingEditor";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Document Folder / Wallet Mapping Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmFolderWalletMappingEditor_FormClosing);
            this.Load += new System.EventHandler(this.frmFolderWalletMappingEditor_Load);
            this.pnlSelector.ResumeLayout(false);
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private FWBS.OMS.UI.Windows.ResourceLookup resourceLookup;
        private System.Windows.Forms.Panel pnlSelector;
        public Windows.eCLCollectionSelector eclDocWallets;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
    }
}