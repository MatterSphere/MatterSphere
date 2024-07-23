namespace FWBS.OMS.UI.DocumentManagement.Addins
{
    partial class ucDMTVTemplateCreator
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
            this.pnlBase = new System.Windows.Forms.Panel();
            this.pnlTreeView = new System.Windows.Forms.Panel();
            this.templateTreeView = new Telerik.WinControls.UI.RadTreeView();
            this.splitter = new System.Windows.Forms.Splitter();
            this.pnlCommands = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pnlButtonBase = new System.Windows.Forms.Panel();
            this.chbMigrateWalletsToFoldersOnSave = new System.Windows.Forms.CheckBox();
            this.btnNewTemplate = new System.Windows.Forms.Button();
            this.btnDeleteTemplate = new System.Windows.Forms.Button();
            this.btnSaveAsNewTemplate = new System.Windows.Forms.Button();
            this.btnSaveTemplate = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.folderProperties = new System.Windows.Forms.PropertyGrid();
            this.pnlExistingTemplates = new System.Windows.Forms.Panel();
            this.lblTemplates = new System.Windows.Forms.Label();
            this.cboTemplates = new FWBS.OMS.UI.Windows.eXPComboBoxCodeLookup();
            this.resourceLookup = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.pnlBase.SuspendLayout();
            this.pnlTreeView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.templateTreeView)).BeginInit();
            this.pnlCommands.SuspendLayout();
            this.panel2.SuspendLayout();
            this.pnlButtonBase.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlExistingTemplates.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBase
            // 
            this.pnlBase.Controls.Add(this.pnlTreeView);
            this.pnlBase.Controls.Add(this.splitter);
            this.pnlBase.Controls.Add(this.pnlCommands);
            this.pnlBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBase.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlBase.Location = new System.Drawing.Point(0, 0);
            this.pnlBase.Margin = new System.Windows.Forms.Padding(2);
            this.pnlBase.Name = "pnlBase";
            this.pnlBase.Padding = new System.Windows.Forms.Padding(4);
            this.pnlBase.Size = new System.Drawing.Size(788, 683);
            this.pnlBase.TabIndex = 0;
            // 
            // pnlTreeView
            // 
            this.pnlTreeView.Controls.Add(this.templateTreeView);
            this.pnlTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTreeView.Location = new System.Drawing.Point(258, 4);
            this.pnlTreeView.Margin = new System.Windows.Forms.Padding(2);
            this.pnlTreeView.Name = "pnlTreeView";
            this.pnlTreeView.Size = new System.Drawing.Size(526, 675);
            this.pnlTreeView.TabIndex = 2;
            // 
            // templateTreeView
            // 
            this.templateTreeView.AllowDragDrop = true;
            this.templateTreeView.AllowDrop = true;
            this.templateTreeView.BackColor = System.Drawing.Color.White;
            this.templateTreeView.Cursor = System.Windows.Forms.Cursors.Default;
            this.templateTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.templateTreeView.EnableKeyMap = true;
            this.templateTreeView.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.templateTreeView.ForeColor = System.Drawing.SystemColors.ControlText;
            this.templateTreeView.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(251)))), ((int)(((byte)(252)))));
            this.templateTreeView.Location = new System.Drawing.Point(0, 0);
            this.templateTreeView.Margin = new System.Windows.Forms.Padding(0);
            this.templateTreeView.Name = "templateTreeView";
            this.templateTreeView.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // 
            // 
            this.templateTreeView.RootElement.ControlBounds = new System.Drawing.Rectangle(0, 0, 526, 675);
            this.templateTreeView.Size = new System.Drawing.Size(526, 675);
            this.templateTreeView.SpacingBetweenNodes = 5;
            this.templateTreeView.TabIndex = 23;
            this.templateTreeView.ThemeName = "Windows8";
            this.templateTreeView.TreeIndent = 10;
            this.templateTreeView.SelectedNodeChanged += new Telerik.WinControls.UI.RadTreeView.RadTreeViewEventHandler(this.templateTreeView_SelectedNodeChanged);
            // 
            // splitter
            // 
            this.splitter.Location = new System.Drawing.Point(254, 4);
            this.splitter.Margin = new System.Windows.Forms.Padding(2);
            this.splitter.Name = "splitter";
            this.splitter.Size = new System.Drawing.Size(4, 675);
            this.splitter.TabIndex = 1;
            this.splitter.TabStop = false;
            // 
            // pnlCommands
            // 
            this.pnlCommands.Controls.Add(this.panel2);
            this.pnlCommands.Controls.Add(this.panel1);
            this.pnlCommands.Controls.Add(this.pnlExistingTemplates);
            this.pnlCommands.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlCommands.Location = new System.Drawing.Point(4, 4);
            this.pnlCommands.Margin = new System.Windows.Forms.Padding(2);
            this.pnlCommands.Name = "pnlCommands";
            this.pnlCommands.Size = new System.Drawing.Size(250, 675);
            this.pnlCommands.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.pnlButtonBase);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 65);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(250, 345);
            this.panel2.TabIndex = 6;
            // 
            // pnlButtonBase
            // 
            this.pnlButtonBase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.pnlButtonBase.Controls.Add(this.chbMigrateWalletsToFoldersOnSave);
            this.pnlButtonBase.Controls.Add(this.btnNewTemplate);
            this.pnlButtonBase.Controls.Add(this.btnDeleteTemplate);
            this.pnlButtonBase.Controls.Add(this.btnSaveAsNewTemplate);
            this.pnlButtonBase.Controls.Add(this.btnSaveTemplate);
            this.pnlButtonBase.Location = new System.Drawing.Point(38, 0);
            this.pnlButtonBase.Margin = new System.Windows.Forms.Padding(2);
            this.pnlButtonBase.Name = "pnlButtonBase";
            this.pnlButtonBase.Padding = new System.Windows.Forms.Padding(4);
            this.pnlButtonBase.Size = new System.Drawing.Size(173, 340);
            this.pnlButtonBase.TabIndex = 5;
            // 
            // chbMigrateWalletsToFoldersOnSave
            // 
            this.chbMigrateWalletsToFoldersOnSave.AutoSize = true;
            this.chbMigrateWalletsToFoldersOnSave.Location = new System.Drawing.Point(5, 176);
            this.resourceLookup.SetLookup(this.chbMigrateWalletsToFoldersOnSave, new FWBS.OMS.UI.Windows.ResourceLookupItem("RNMIGRATIONONSV", "Run migration on save", ""));
            this.chbMigrateWalletsToFoldersOnSave.Name = "chbMigrateWalletsToFoldersOnSave";
            this.chbMigrateWalletsToFoldersOnSave.Size = new System.Drawing.Size(145, 19);
            this.chbMigrateWalletsToFoldersOnSave.TabIndex = 5;
            this.chbMigrateWalletsToFoldersOnSave.Text = "Run migration on save";
            this.chbMigrateWalletsToFoldersOnSave.UseVisualStyleBackColor = true;
            // 
            // btnNewTemplate
            // 
            this.btnNewTemplate.Location = new System.Drawing.Point(5, 24);
            this.resourceLookup.SetLookup(this.btnNewTemplate, new FWBS.OMS.UI.Windows.ResourceLookupItem("DMTVTemplate_0", "New Template", ""));
            this.btnNewTemplate.Margin = new System.Windows.Forms.Padding(2);
            this.btnNewTemplate.Name = "btnNewTemplate";
            this.btnNewTemplate.Size = new System.Drawing.Size(166, 30);
            this.btnNewTemplate.TabIndex = 3;
            this.btnNewTemplate.Text = "New Template";
            this.btnNewTemplate.UseVisualStyleBackColor = true;
            this.btnNewTemplate.Click += new System.EventHandler(this.btnNewTemplate_Click);
            // 
            // btnDeleteTemplate
            // 
            this.btnDeleteTemplate.Location = new System.Drawing.Point(5, 138);
            this.resourceLookup.SetLookup(this.btnDeleteTemplate, new FWBS.OMS.UI.Windows.ResourceLookupItem("DMTVTemplate_4", "Delete Template", ""));
            this.btnDeleteTemplate.Margin = new System.Windows.Forms.Padding(2);
            this.btnDeleteTemplate.Name = "btnDeleteTemplate";
            this.btnDeleteTemplate.Size = new System.Drawing.Size(166, 30);
            this.btnDeleteTemplate.TabIndex = 2;
            this.btnDeleteTemplate.Text = "Delete Template";
            this.btnDeleteTemplate.UseVisualStyleBackColor = true;
            this.btnDeleteTemplate.Click += new System.EventHandler(this.btnDeleteTemplate_Click);
            // 
            // btnSaveAsNewTemplate
            // 
            this.btnSaveAsNewTemplate.Location = new System.Drawing.Point(5, 100);
            this.resourceLookup.SetLookup(this.btnSaveAsNewTemplate, new FWBS.OMS.UI.Windows.ResourceLookupItem("DMTVTemplate_3", "Save As New Template", ""));
            this.btnSaveAsNewTemplate.Margin = new System.Windows.Forms.Padding(2);
            this.btnSaveAsNewTemplate.Name = "btnSaveAsNewTemplate";
            this.btnSaveAsNewTemplate.Size = new System.Drawing.Size(166, 30);
            this.btnSaveAsNewTemplate.TabIndex = 1;
            this.btnSaveAsNewTemplate.Text = "Save As New Template";
            this.btnSaveAsNewTemplate.UseVisualStyleBackColor = true;
            this.btnSaveAsNewTemplate.Click += new System.EventHandler(this.btnSaveAsNewTemplate_Click);
            // 
            // btnSaveTemplate
            // 
            this.btnSaveTemplate.Location = new System.Drawing.Point(5, 62);
            this.resourceLookup.SetLookup(this.btnSaveTemplate, new FWBS.OMS.UI.Windows.ResourceLookupItem("DMTVTemplate_2", "Save Template", ""));
            this.btnSaveTemplate.Margin = new System.Windows.Forms.Padding(2);
            this.btnSaveTemplate.Name = "btnSaveTemplate";
            this.btnSaveTemplate.Size = new System.Drawing.Size(166, 30);
            this.btnSaveTemplate.TabIndex = 0;
            this.btnSaveTemplate.Text = "Save Template";
            this.btnSaveTemplate.UseVisualStyleBackColor = true;
            this.btnSaveTemplate.Click += new System.EventHandler(this.btnSaveTemplate_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.folderProperties);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 410);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(250, 265);
            this.panel1.TabIndex = 5;
            // 
            // folderProperties
            // 
            this.folderProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.folderProperties.Location = new System.Drawing.Point(0, 0);
            this.folderProperties.Margin = new System.Windows.Forms.Padding(2);
            this.folderProperties.Name = "folderProperties";
            this.folderProperties.Size = new System.Drawing.Size(248, 263);
            this.folderProperties.TabIndex = 1;
            // 
            // pnlExistingTemplates
            // 
            this.pnlExistingTemplates.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlExistingTemplates.Controls.Add(this.lblTemplates);
            this.pnlExistingTemplates.Controls.Add(this.cboTemplates);
            this.pnlExistingTemplates.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlExistingTemplates.Location = new System.Drawing.Point(0, 0);
            this.pnlExistingTemplates.Margin = new System.Windows.Forms.Padding(8, 2, 8, 2);
            this.pnlExistingTemplates.Name = "pnlExistingTemplates";
            this.pnlExistingTemplates.Padding = new System.Windows.Forms.Padding(4, 8, 4, 8);
            this.pnlExistingTemplates.Size = new System.Drawing.Size(250, 65);
            this.pnlExistingTemplates.TabIndex = 1;
            // 
            // lblTemplates
            // 
            this.lblTemplates.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblTemplates.Location = new System.Drawing.Point(4, 14);
            this.resourceLookup.SetLookup(this.lblTemplates, new FWBS.OMS.UI.Windows.ResourceLookupItem("DMTVTemplate_1", "Select an Existing Template", ""));
            this.lblTemplates.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTemplates.Name = "lblTemplates";
            this.lblTemplates.Size = new System.Drawing.Size(240, 18);
            this.lblTemplates.TabIndex = 1;
            this.lblTemplates.Text = "Select an Existing Template";
            this.lblTemplates.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // cboTemplates
            // 
            this.cboTemplates.ActiveSearchEnabled = true;
            this.cboTemplates.AddNotSet = true;
            this.cboTemplates.CaptionWidth = 0;
            this.cboTemplates.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.cboTemplates.IsDirty = false;
            this.cboTemplates.Location = new System.Drawing.Point(4, 32);
            this.cboTemplates.Margin = new System.Windows.Forms.Padding(2);
            this.cboTemplates.MaxLength = 0;
            this.cboTemplates.Name = "cboTemplates";
            this.cboTemplates.NotSetCode = "NOTSET";
            this.cboTemplates.NotSetType = "RESOURCE";
            this.cboTemplates.Size = new System.Drawing.Size(240, 23);
            this.cboTemplates.TabIndex = 0;
            this.cboTemplates.TabStop = false;
            this.cboTemplates.TerminologyParse = false;
            this.cboTemplates.Type = "DFLDR_TEMPLATE";
            this.cboTemplates.ActiveChanged += new System.EventHandler(this.cboTemplates_ActiveChanged);
            // 
            // ucDMTVTemplateCreator
            // 
            this.Controls.Add(this.pnlBase);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ucDMTVTemplateCreator";
            this.Size = new System.Drawing.Size(788, 683);
            this.pnlBase.ResumeLayout(false);
            this.pnlTreeView.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.templateTreeView)).EndInit();
            this.pnlCommands.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.pnlButtonBase.ResumeLayout(false);
            this.pnlButtonBase.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.pnlExistingTemplates.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private FWBS.OMS.UI.Windows.ResourceLookup resourceLookup;
        private System.Windows.Forms.Panel pnlBase;
        private System.Windows.Forms.Panel pnlTreeView;
        private Telerik.WinControls.UI.RadTreeView templateTreeView;
        private System.Windows.Forms.Splitter splitter;
        private System.Windows.Forms.Panel pnlCommands;
        private System.Windows.Forms.Panel pnlExistingTemplates;
        private System.Windows.Forms.Label lblTemplates;
        private FWBS.OMS.UI.Windows.eXPComboBoxCodeLookup cboTemplates;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PropertyGrid folderProperties;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel pnlButtonBase;
        private System.Windows.Forms.Button btnDeleteTemplate;
        private System.Windows.Forms.Button btnSaveAsNewTemplate;
        private System.Windows.Forms.Button btnSaveTemplate;
        private System.Windows.Forms.Button btnNewTemplate;
        private System.Windows.Forms.CheckBox chbMigrateWalletsToFoldersOnSave;
    }
}
