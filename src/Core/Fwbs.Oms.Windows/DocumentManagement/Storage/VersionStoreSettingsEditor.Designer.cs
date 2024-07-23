namespace FWBS.OMS.UI.Windows.DocumentManagement.Storage
{
    partial class VersionStoreSettingsEditor
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
            this.rdoNewVersion = new System.Windows.Forms.RadioButton();
            this.rdoNewSubVersion = new System.Windows.Forms.RadioButton();
            this.rdoOverwrite = new System.Windows.Forms.RadioButton();
            this.txtComments = new System.Windows.Forms.TextBox();
            this.chkCurrent = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pnlNewVersion = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rdoMajorVersion = new System.Windows.Forms.RadioButton();
            this.lblMajorVersion = new System.Windows.Forms.Label();
            this.lblNewSubVersion = new System.Windows.Forms.Label();
            this.lblNewVersion = new System.Windows.Forms.Label();
            this.lblOverwrite = new System.Windows.Forms.Label();
            this.pnlNew = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.pnlComments = new System.Windows.Forms.Panel();
            this.cboStatus = new FWBS.OMS.UI.Windows.eXPComboBoxCodeLookup();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlNewVersion.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlNew.SuspendLayout();
            this.pnlComments.SuspendLayout();
            this.SuspendLayout();
            // 
            // rdoNewVersion
            // 
            this.rdoNewVersion.Dock = System.Windows.Forms.DockStyle.Top;
            this.rdoNewVersion.Location = new System.Drawing.Point(5, 30);
            this.Resources.SetLookup(this.rdoNewVersion, new FWBS.OMS.UI.Windows.ResourceLookupItem("rdoNewVersion", "New Version", ""));
            this.rdoNewVersion.Name = "rdoNewVersion";
            this.rdoNewVersion.Size = new System.Drawing.Size(294, 25);
            this.rdoNewVersion.TabIndex = 0;
            this.rdoNewVersion.TabStop = true;
            this.rdoNewVersion.Text = "New Version";
            this.rdoNewVersion.UseVisualStyleBackColor = true;
            this.rdoNewVersion.CheckedChanged += new System.EventHandler(this.rdoNewVersion_CheckedChanged);
            // 
            // rdoNewSubVersion
            // 
            this.rdoNewSubVersion.Dock = System.Windows.Forms.DockStyle.Top;
            this.rdoNewSubVersion.Location = new System.Drawing.Point(5, 55);
            this.Resources.SetLookup(this.rdoNewSubVersion, new FWBS.OMS.UI.Windows.ResourceLookupItem("rdoNewSubVer", "New Revision", ""));
            this.rdoNewSubVersion.Name = "rdoNewSubVersion";
            this.rdoNewSubVersion.Size = new System.Drawing.Size(294, 25);
            this.rdoNewSubVersion.TabIndex = 1;
            this.rdoNewSubVersion.TabStop = true;
            this.rdoNewSubVersion.Text = "New Revision";
            this.rdoNewSubVersion.UseVisualStyleBackColor = true;
            this.rdoNewSubVersion.CheckedChanged += new System.EventHandler(this.rdoNewVersion_CheckedChanged);
            // 
            // rdoOverwrite
            // 
            this.rdoOverwrite.Dock = System.Windows.Forms.DockStyle.Top;
            this.rdoOverwrite.Location = new System.Drawing.Point(5, 80);
            this.Resources.SetLookup(this.rdoOverwrite, new FWBS.OMS.UI.Windows.ResourceLookupItem("rdoOverwrite", "Overwrite", ""));
            this.rdoOverwrite.Name = "rdoOverwrite";
            this.rdoOverwrite.Size = new System.Drawing.Size(294, 25);
            this.rdoOverwrite.TabIndex = 2;
            this.rdoOverwrite.TabStop = true;
            this.rdoOverwrite.Text = "Overwrite";
            this.rdoOverwrite.UseVisualStyleBackColor = true;
            this.rdoOverwrite.CheckedChanged += new System.EventHandler(this.rdoNewVersion_CheckedChanged);
            // 
            // txtComments
            // 
            this.txtComments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtComments.Location = new System.Drawing.Point(13, 22);
            this.txtComments.MaxLength = 500;
            this.txtComments.Multiline = true;
            this.txtComments.Name = "txtComments";
            this.txtComments.Size = new System.Drawing.Size(316, 47);
            this.txtComments.TabIndex = 3;
            this.txtComments.TextChanged += new System.EventHandler(this.txtComments_TextChanged);
            // 
            // chkCurrent
            // 
            this.chkCurrent.AutoSize = true;
            this.chkCurrent.Location = new System.Drawing.Point(199, 79);
            this.Resources.SetLookup(this.chkCurrent, new FWBS.OMS.UI.Windows.ResourceLookupItem("chkCurrent", "Flag as Latest Version", ""));
            this.chkCurrent.Name = "chkCurrent";
            this.chkCurrent.Size = new System.Drawing.Size(130, 17);
            this.chkCurrent.TabIndex = 5;
            this.chkCurrent.Text = "Flag as Latest Version";
            this.chkCurrent.UseVisualStyleBackColor = true;
            this.chkCurrent.CheckedChanged += new System.EventHandler(this.chkCurrent_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(14, 3);
            this.Resources.SetLookup(this.label3, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblsaveworkver", "Save Working Version As", ""));
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(136, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Save Working Version As";
            // 
            // pnlNewVersion
            // 
            this.pnlNewVersion.Controls.Add(this.panel1);
            this.pnlNewVersion.Controls.Add(this.label3);
            this.pnlNewVersion.Controls.Add(this.lblMajorVersion);
            this.pnlNewVersion.Controls.Add(this.lblNewSubVersion);
            this.pnlNewVersion.Controls.Add(this.lblNewVersion);
            this.pnlNewVersion.Controls.Add(this.lblOverwrite);
            this.pnlNewVersion.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlNewVersion.Location = new System.Drawing.Point(10, 10);
            this.pnlNewVersion.Name = "pnlNewVersion";
            this.pnlNewVersion.Padding = new System.Windows.Forms.Padding(10);
            this.pnlNewVersion.Size = new System.Drawing.Size(347, 161);
            this.pnlNewVersion.TabIndex = 11;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rdoOverwrite);
            this.panel1.Controls.Add(this.rdoNewSubVersion);
            this.panel1.Controls.Add(this.rdoNewVersion);
            this.panel1.Controls.Add(this.rdoMajorVersion);
            this.panel1.Location = new System.Drawing.Point(12, 17);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5);
            this.panel1.Size = new System.Drawing.Size(304, 100);
            this.panel1.TabIndex = 15;
            // 
            // rdoMajorVersion
            // 
            this.rdoMajorVersion.Dock = System.Windows.Forms.DockStyle.Top;
            this.rdoMajorVersion.Location = new System.Drawing.Point(5, 5);
            this.Resources.SetLookup(this.rdoMajorVersion, new FWBS.OMS.UI.Windows.ResourceLookupItem("rdoMajorVersion", "New Major Version", ""));
            this.rdoMajorVersion.Name = "rdoMajorVersion";
            this.rdoMajorVersion.Size = new System.Drawing.Size(294, 25);
            this.rdoMajorVersion.TabIndex = 13;
            this.rdoMajorVersion.TabStop = true;
            this.rdoMajorVersion.Text = "New Major Version";
            this.rdoMajorVersion.UseVisualStyleBackColor = true;
            this.rdoMajorVersion.CheckedChanged += new System.EventHandler(this.rdoNewVersion_CheckedChanged);
            // 
            // lblMajorVersion
            // 
            this.lblMajorVersion.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblMajorVersion.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblMajorVersion.Location = new System.Drawing.Point(10, 27);
            this.Resources.SetLookup(this.lblMajorVersion, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblMajorVersion", "The document is going to be saved as the next available major version.", ""));
            this.lblMajorVersion.Name = "lblMajorVersion";
            this.lblMajorVersion.Size = new System.Drawing.Size(327, 31);
            this.lblMajorVersion.TabIndex = 14;
            this.lblMajorVersion.Text = "The document is going to be saved as the next available major version.";
            this.lblMajorVersion.Visible = false;
            // 
            // lblNewSubVersion
            // 
            this.lblNewSubVersion.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblNewSubVersion.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblNewSubVersion.Location = new System.Drawing.Point(10, 58);
            this.Resources.SetLookup(this.lblNewSubVersion, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblNewSubVer", "The document is going to be saved as a new sub version under the current working " + 
                                                                                                                       "document.", ""));
            this.lblNewSubVersion.Name = "lblNewSubVersion";
            this.lblNewSubVersion.Size = new System.Drawing.Size(327, 31);
            this.lblNewSubVersion.TabIndex = 11;
            this.lblNewSubVersion.Text = "The document is going to be saved as a new sub version under the current working " +
    "document.";
            this.lblNewSubVersion.Visible = false;
            // 
            // lblNewVersion
            // 
            this.lblNewVersion.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblNewVersion.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblNewVersion.Location = new System.Drawing.Point(10, 89);
            this.Resources.SetLookup(this.lblNewVersion, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblNewVersion", "The document is going to be saved as a new version of the current working documen" +
                                                                                                                     "t.", ""));
            this.lblNewVersion.Name = "lblNewVersion";
            this.lblNewVersion.Size = new System.Drawing.Size(327, 31);
            this.lblNewVersion.TabIndex = 10;
            this.lblNewVersion.Text = "The document is going to be saved as a new version of the current working documen" +
    "t.";
            this.lblNewVersion.Visible = false;
            // 
            // lblOverwrite
            // 
            this.lblOverwrite.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblOverwrite.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblOverwrite.ForeColor = System.Drawing.Color.Red;
            this.lblOverwrite.Location = new System.Drawing.Point(10, 120);
            this.Resources.SetLookup(this.lblOverwrite, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblOverwrite", "Warning! The document is going to overwrite the current working document.", ""));
            this.lblOverwrite.Name = "lblOverwrite";
            this.lblOverwrite.Size = new System.Drawing.Size(327, 31);
            this.lblOverwrite.TabIndex = 12;
            this.lblOverwrite.Text = "Warning! The document is going to overwrite the current working document.";
            this.lblOverwrite.Visible = false;
            // 
            // pnlNew
            // 
            this.pnlNew.Controls.Add(this.label5);
            this.pnlNew.Controls.Add(this.label4);
            this.pnlNew.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlNew.Location = new System.Drawing.Point(10, 171);
            this.pnlNew.Name = "pnlNew";
            this.pnlNew.Size = new System.Drawing.Size(347, 45);
            this.pnlNew.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(10, 2);
            this.Resources.SetLookup(this.label5, new FWBS.OMS.UI.Windows.ResourceLookupItem("NewDoc", "New Document", ""));
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(88, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "New Document";
            // 
            // label4
            // 
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label4.Location = new System.Drawing.Point(14, 19);
            this.Resources.SetLookup(this.label4, new FWBS.OMS.UI.Windows.ResourceLookupItem("NewDocDesc", "This is a new document and will be stored as version 1.", ""));
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(327, 18);
            this.label4.TabIndex = 0;
            this.label4.Text = "This is a new document and will be stored as version 1.";
            // 
            // pnlComments
            // 
            this.pnlComments.Controls.Add(this.cboStatus);
            this.pnlComments.Controls.Add(this.chkCurrent);
            this.pnlComments.Controls.Add(this.label1);
            this.pnlComments.Controls.Add(this.txtComments);
            this.pnlComments.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlComments.Location = new System.Drawing.Point(10, 216);
            this.pnlComments.Name = "pnlComments";
            this.pnlComments.Size = new System.Drawing.Size(347, 134);
            this.pnlComments.TabIndex = 13;
            // 
            // cboStatus
            // 
            this.cboStatus.ActiveSearchEnabled = true;
            this.cboStatus.AddNotSet = true;
            this.cboStatus.CaptionWidth = 50;
            this.cboStatus.IsDirty = false;
            this.cboStatus.Location = new System.Drawing.Point(12, 75);
            this.Resources.SetLookup(this.cboStatus, new FWBS.OMS.UI.Windows.ResourceLookupItem("cboStatus", "Status", ""));
            this.cboStatus.MaxLength = 0;
            this.cboStatus.Name = "cboStatus";
            this.cboStatus.NotSetCode = "NOTSET";
            this.cboStatus.NotSetType = "RESOURCE";
            this.cboStatus.Size = new System.Drawing.Size(174, 23);
            this.cboStatus.TabIndex = 4;
            this.cboStatus.TerminologyParse = false;
            this.cboStatus.Text = "Status";
            this.cboStatus.Type = "DOCSTATUS";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 1);
            this.Resources.SetLookup(this.label1, new FWBS.OMS.UI.Windows.ResourceLookupItem("optVerCom", "Optional Version Related Comments", ""));
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(176, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Optional Version Related Comments";
            // 
            // VersionStoreSettingsEditor
            // 
            this.Controls.Add(this.pnlComments);
            this.Controls.Add(this.pnlNew);
            this.Controls.Add(this.pnlNewVersion);
            this.Name = "VersionStoreSettingsEditor";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Size = new System.Drawing.Size(367, 363);
            this.pnlNewVersion.ResumeLayout(false);
            this.pnlNewVersion.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.pnlNew.ResumeLayout(false);
            this.pnlNew.PerformLayout();
            this.pnlComments.ResumeLayout(false);
            this.pnlComments.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton rdoNewVersion;
        private System.Windows.Forms.RadioButton rdoNewSubVersion;
        private System.Windows.Forms.RadioButton rdoOverwrite;
        private System.Windows.Forms.TextBox txtComments;
        private System.Windows.Forms.CheckBox chkCurrent;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel pnlNewVersion;
        private System.Windows.Forms.Panel pnlNew;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblNewVersion;
        private System.Windows.Forms.Panel pnlComments;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblOverwrite;
        private System.Windows.Forms.Label lblNewSubVersion;
        private System.Windows.Forms.RadioButton rdoMajorVersion;
        private System.Windows.Forms.Label lblMajorVersion;
        private System.Windows.Forms.Panel panel1;
        private FWBS.OMS.UI.Windows.eXPComboBoxCodeLookup cboStatus;


    }
}
