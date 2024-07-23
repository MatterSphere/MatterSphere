namespace FWBS.OMS.UI.Windows.DocumentManagement.Storage
{
    partial class StorageSettingsContainer
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
            this.components = new System.ComponentModel.Container();
            this.cboStorageProvider = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.tabControl1 = new FWBS.OMS.UI.TabControl();
            this.txtNotSupported = new System.Windows.Forms.TextBox();
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.pnlHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // cboStorageProvider
            // 
            this.cboStorageProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStorageProvider.FormattingEnabled = true;
            this.cboStorageProvider.Location = new System.Drawing.Point(125, 7);
            this.cboStorageProvider.Name = "cboStorageProvider";
            this.cboStorageProvider.Size = new System.Drawing.Size(211, 21);
            this.cboStorageProvider.TabIndex = 1;
            this.cboStorageProvider.SelectedIndexChanged += new System.EventHandler(this.cboStorageProvider_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.resourceLookup1.SetLookup(this.label1, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblstorprov", "Storage Provider", ""));
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Storage Provider";
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.cboStorageProvider);
            this.pnlHeader.Controls.Add(this.label1);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(354, 36);
            this.pnlHeader.TabIndex = 4;
            // 
            // tabControl1
            // 
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 36);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(354, 191);
            this.tabControl1.TabIndex = 5;
            this.tabControl1.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl1_Selected);
            // 
            // txtNotSupported
            // 
            this.txtNotSupported.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNotSupported.BackColor = System.Drawing.SystemColors.Window;
            this.txtNotSupported.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtNotSupported.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNotSupported.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtNotSupported.Location = new System.Drawing.Point(2, 106);
            this.resourceLookup1.SetLookup(this.txtNotSupported, new FWBS.OMS.UI.Windows.ResourceLookupItem("txtNotSupported", "(No Supported Settings Available)", ""));
            this.txtNotSupported.Multiline = true;
            this.txtNotSupported.Name = "txtNotSupported";
            this.txtNotSupported.ReadOnly = true;
            this.txtNotSupported.Size = new System.Drawing.Size(348, 17);
            this.txtNotSupported.TabIndex = 6;
            this.txtNotSupported.Text = "(No Supported Settings Available)";
            this.txtNotSupported.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // StorageSettingsContainer
            // 
            this.Controls.Add(this.txtNotSupported);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.pnlHeader);
            this.Name = "StorageSettingsContainer";
            this.Size = new System.Drawing.Size(354, 227);
            this.VisibleChanged += new System.EventHandler(this.StorageSettingsContainer_VisibleChanged);
            this.ParentChanged += new System.EventHandler(this.StorageSettingsContainer_ParentChanged);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboStorageProvider;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlHeader;
        private FWBS.OMS.UI.TabControl tabControl1;
        private System.Windows.Forms.TextBox txtNotSupported;
        private ResourceLookup resourceLookup1;

    }
}
