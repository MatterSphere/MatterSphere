namespace FWBS.OMS.UI.DocumentManagement
{
    partial class VersionOptionsPicker
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VersionOptionsPicker));
            this.rdoNewDoc = new System.Windows.Forms.RadioButton();
            this.rdoMajorVersion = new System.Windows.Forms.RadioButton();
            this.rdoOverwrite = new System.Windows.Forms.RadioButton();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblVersion = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblDesc = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblDocID = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.grpOptions = new System.Windows.Forms.GroupBox();
            this.rdoNewSubVersion = new System.Windows.Forms.RadioButton();
            this.rdoNewVersion = new System.Windows.Forms.RadioButton();
            this.resLookup = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox1.SuspendLayout();
            this.grpOptions.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // rdoNewDoc
            // 
            this.rdoNewDoc.Dock = System.Windows.Forms.DockStyle.Top;
            this.rdoNewDoc.Location = new System.Drawing.Point(8, 19);
            this.resLookup.SetLookup(this.rdoNewDoc, new FWBS.OMS.UI.Windows.ResourceLookupItem("rdoNewDoc", "New Document", ""));
            this.rdoNewDoc.Name = "rdoNewDoc";
            this.rdoNewDoc.Size = new System.Drawing.Size(539, 20);
            this.rdoNewDoc.TabIndex = 1;
            this.rdoNewDoc.TabStop = true;
            this.rdoNewDoc.Text = "New Document";
            this.rdoNewDoc.UseVisualStyleBackColor = true;
            // 
            // rdoMajorVersion
            // 
            this.rdoMajorVersion.Dock = System.Windows.Forms.DockStyle.Top;
            this.rdoMajorVersion.Location = new System.Drawing.Point(8, 39);
            this.resLookup.SetLookup(this.rdoMajorVersion, new FWBS.OMS.UI.Windows.ResourceLookupItem("rdoMajorVersion", "New Major Version", ""));
            this.rdoMajorVersion.Name = "rdoMajorVersion";
            this.rdoMajorVersion.Size = new System.Drawing.Size(539, 20);
            this.rdoMajorVersion.TabIndex = 2;
            this.rdoMajorVersion.TabStop = true;
            this.rdoMajorVersion.Text = "New Major Version";
            this.rdoMajorVersion.UseVisualStyleBackColor = true;
            // 
            // rdoOverwrite
            // 
            this.rdoOverwrite.Dock = System.Windows.Forms.DockStyle.Top;
            this.rdoOverwrite.Location = new System.Drawing.Point(8, 99);
            this.resLookup.SetLookup(this.rdoOverwrite, new FWBS.OMS.UI.Windows.ResourceLookupItem("rdoOverwrite", "Overwrite", ""));
            this.rdoOverwrite.Name = "rdoOverwrite";
            this.rdoOverwrite.Size = new System.Drawing.Size(539, 20);
            this.rdoOverwrite.TabIndex = 5;
            this.rdoOverwrite.TabStop = true;
            this.rdoOverwrite.Text = "Overwrite";
            this.rdoOverwrite.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(397, 8);
            this.resLookup.SetLookup(this.btnOK, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnOK", "&OK", ""));
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 24);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(478, 8);
            this.resLookup.SetLookup(this.btnCancel, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnCancel", "&Cancel", ""));
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 24);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblVersion);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.lblDesc);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.lblDocID);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(5, 5);
            this.resLookup.SetLookup(this.groupBox1, new FWBS.OMS.UI.Windows.ResourceLookupItem("DocInformation", "Document Information", ""));
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(550, 68);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Document Information";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(224, 20);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(58, 15);
            this.lblVersion.TabIndex = 7;
            this.lblVersion.Text = "lblVersion";
            this.lblVersion.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(176, 20);
            this.resLookup.SetLookup(this.label2, new FWBS.OMS.UI.Windows.ResourceLookupItem("Version", "Version", ""));
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 15);
            this.label2.TabIndex = 6;
            this.label2.Text = "Version";
            this.label2.Visible = false;
            // 
            // lblDesc
            // 
            this.lblDesc.AutoSize = true;
            this.lblDesc.Location = new System.Drawing.Point(90, 43);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(45, 15);
            this.lblDesc.TabIndex = 5;
            this.lblDesc.Text = "lblDesc";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 43);
            this.resLookup.SetLookup(this.label5, new FWBS.OMS.UI.Windows.ResourceLookupItem("Description", "Description", ""));
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 15);
            this.label5.TabIndex = 4;
            this.label5.Text = "Description";
            // 
            // lblDocID
            // 
            this.lblDocID.AutoSize = true;
            this.lblDocID.Location = new System.Drawing.Point(90, 20);
            this.lblDocID.Name = "lblDocID";
            this.lblDocID.Size = new System.Drawing.Size(52, 15);
            this.lblDocID.TabIndex = 1;
            this.lblDocID.Text = "lblDocID";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 20);
            this.resLookup.SetLookup(this.label1, new FWBS.OMS.UI.Windows.ResourceLookupItem("DocID", "Doc ID", ""));
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Doc ID";
            // 
            // grpOptions
            // 
            this.grpOptions.Controls.Add(this.rdoOverwrite);
            this.grpOptions.Controls.Add(this.rdoNewSubVersion);
            this.grpOptions.Controls.Add(this.rdoNewVersion);
            this.grpOptions.Controls.Add(this.rdoMajorVersion);
            this.grpOptions.Controls.Add(this.rdoNewDoc);
            this.grpOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpOptions.Location = new System.Drawing.Point(5, 73);
            this.resLookup.SetLookup(this.grpOptions, new FWBS.OMS.UI.Windows.ResourceLookupItem("Options", "Options", ""));
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Padding = new System.Windows.Forms.Padding(8, 3, 3, 3);
            this.grpOptions.Size = new System.Drawing.Size(550, 131);
            this.grpOptions.TabIndex = 7;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "Options";
            // 
            // rdoNewSubVersion
            // 
            this.rdoNewSubVersion.Dock = System.Windows.Forms.DockStyle.Top;
            this.rdoNewSubVersion.Location = new System.Drawing.Point(8, 79);
            this.resLookup.SetLookup(this.rdoNewSubVersion, new FWBS.OMS.UI.Windows.ResourceLookupItem("rdoNewSubVer", "New Revision", ""));
            this.rdoNewSubVersion.Name = "rdoNewSubVersion";
            this.rdoNewSubVersion.Size = new System.Drawing.Size(539, 20);
            this.rdoNewSubVersion.TabIndex = 4;
            this.rdoNewSubVersion.TabStop = true;
            this.rdoNewSubVersion.Text = "New Revision";
            this.rdoNewSubVersion.UseVisualStyleBackColor = true;
            // 
            // rdoNewVersion
            // 
            this.rdoNewVersion.Dock = System.Windows.Forms.DockStyle.Top;
            this.rdoNewVersion.Location = new System.Drawing.Point(8, 59);
            this.resLookup.SetLookup(this.rdoNewVersion, new FWBS.OMS.UI.Windows.ResourceLookupItem("rdoNewVersion", "New Version", ""));
            this.rdoNewVersion.Name = "rdoNewVersion";
            this.rdoNewVersion.Size = new System.Drawing.Size(539, 20);
            this.rdoNewVersion.TabIndex = 3;
            this.rdoNewVersion.TabStop = true;
            this.rdoNewVersion.Text = "New Version";
            this.rdoNewVersion.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.grpOptions);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5);
            this.panel1.Size = new System.Drawing.Size(560, 209);
            this.panel1.TabIndex = 8;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Control;
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.panel2.Location = new System.Drawing.Point(0, 209);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.panel2.Size = new System.Drawing.Size(560, 40);
            this.panel2.TabIndex = 9;
            // 
            // VersionOptionsPicker
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(560, 249);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.resLookup.SetLookup(this, new FWBS.OMS.UI.Windows.ResourceLookupItem("VersOptionsPkr", "Save As Options", ""));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VersionOptionsPicker";
            this.Text = "Save As Options";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grpOptions.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton rdoNewDoc;
        private System.Windows.Forms.RadioButton rdoMajorVersion;
        private System.Windows.Forms.RadioButton rdoOverwrite;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblDocID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblDesc;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox grpOptions;
        private FWBS.OMS.UI.Windows.ResourceLookup resLookup;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rdoNewSubVersion;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton rdoNewVersion;
    }
}