namespace FWBS.OMS.UI.Windows.Admin
{
    partial class frmFullSystemUpdate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFullSystemUpdate));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.pnlTools = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnUnCheckAll = new System.Windows.Forms.Button();
            this.btnCheckAll = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.pnlFolderLocation = new System.Windows.Forms.Panel();
            this.btnForward = new System.Windows.Forms.Button();
            this.btnParent = new System.Windows.Forms.Button();
            this.labFolder = new System.Windows.Forms.Label();
            this.btnFolderBrowse = new System.Windows.Forms.Button();
            this.txtFolderLocation = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkAutoStart = new System.Windows.Forms.CheckBox();
            this.chkCopy = new System.Windows.Forms.CheckBox();
            this.chkShowReadme = new System.Windows.Forms.CheckBox();
            this.chkMakeBackup = new System.Windows.Forms.CheckBox();
            this.listView1 = new FWBS.OMS.UI.ListView();
            this.pkgName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.LastInstalled = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Result = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pnlTools.SuspendLayout();
            this.pnlFolderLocation.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // pnlTools
            // 
            this.pnlTools.BackColor = System.Drawing.SystemColors.Control;
            this.pnlTools.Controls.Add(this.label1);
            this.pnlTools.Controls.Add(this.btnBrowse);
            this.pnlTools.Controls.Add(this.btnUnCheckAll);
            this.pnlTools.Controls.Add(this.btnCheckAll);
            this.pnlTools.Controls.Add(this.btnStart);
            this.pnlTools.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlTools.Location = new System.Drawing.Point(560, 0);
            this.pnlTools.Name = "pnlTools";
            this.pnlTools.Size = new System.Drawing.Size(106, 513);
            this.pnlTools.TabIndex = 1;
            this.pnlTools.DoubleClick += new System.EventHandler(this.pnlTools_DoubleClick);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(11, 425);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 52);
            this.label1.TabIndex = 3;
            this.label1.Text = "Browse and Install Single Package";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.Visible = false;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnBrowse.Enabled = false;
            this.btnBrowse.Location = new System.Drawing.Point(11, 480);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(84, 25);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "&Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Visible = false;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnUnCheckAll
            // 
            this.btnUnCheckAll.Location = new System.Drawing.Point(11, 66);
            this.btnUnCheckAll.Name = "btnUnCheckAll";
            this.btnUnCheckAll.Size = new System.Drawing.Size(84, 25);
            this.btnUnCheckAll.TabIndex = 0;
            this.btnUnCheckAll.Text = "&Uncheck All";
            this.btnUnCheckAll.UseVisualStyleBackColor = true;
            this.btnUnCheckAll.Click += new System.EventHandler(this.btnUnCheckAll_Click);
            // 
            // btnCheckAll
            // 
            this.btnCheckAll.Location = new System.Drawing.Point(11, 36);
            this.btnCheckAll.Name = "btnCheckAll";
            this.btnCheckAll.Size = new System.Drawing.Size(84, 25);
            this.btnCheckAll.TabIndex = 0;
            this.btnCheckAll.Text = "&Check All";
            this.btnCheckAll.UseVisualStyleBackColor = true;
            this.btnCheckAll.Click += new System.EventHandler(this.btnCheckAll_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(11, 6);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(84, 25);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "&Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.AddExtension = false;
            this.openFileDialog1.Filter = "Manifest.xml|*.manifest.xml";
            this.openFileDialog1.Title = "Browse for a Package";
            // 
            // pnlFolderLocation
            // 
            this.pnlFolderLocation.Controls.Add(this.btnForward);
            this.pnlFolderLocation.Controls.Add(this.btnParent);
            this.pnlFolderLocation.Controls.Add(this.labFolder);
            this.pnlFolderLocation.Controls.Add(this.btnFolderBrowse);
            this.pnlFolderLocation.Controls.Add(this.txtFolderLocation);
            this.pnlFolderLocation.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFolderLocation.Location = new System.Drawing.Point(0, 0);
            this.pnlFolderLocation.Name = "pnlFolderLocation";
            this.pnlFolderLocation.Size = new System.Drawing.Size(560, 36);
            this.pnlFolderLocation.TabIndex = 2;
            // 
            // btnForward
            // 
            this.btnForward.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnForward.Enabled = false;
            this.btnForward.Location = new System.Drawing.Point(454, 6);
            this.btnForward.Name = "btnForward";
            this.btnForward.Size = new System.Drawing.Size(25, 25);
            this.btnForward.TabIndex = 5;
            this.toolTip1.SetToolTip(this.btnForward, "Forward");
            this.btnForward.UseVisualStyleBackColor = true;
            this.btnForward.Click += new System.EventHandler(this.btnForward_Click);
            // 
            // btnParent
            // 
            this.btnParent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnParent.Enabled = false;
            this.btnParent.Location = new System.Drawing.Point(428, 6);
            this.btnParent.Name = "btnParent";
            this.btnParent.Size = new System.Drawing.Size(25, 25);
            this.btnParent.TabIndex = 4;
            this.toolTip1.SetToolTip(this.btnParent, "Parent Folder");
            this.btnParent.UseVisualStyleBackColor = true;
            this.btnParent.Click += new System.EventHandler(this.btnParent_Click);
            // 
            // labFolder
            // 
            this.labFolder.AutoSize = true;
            this.labFolder.Location = new System.Drawing.Point(5, 11);
            this.labFolder.Name = "labFolder";
            this.labFolder.Size = new System.Drawing.Size(49, 15);
            this.labFolder.TabIndex = 3;
            this.labFolder.Text = "Folder : ";
            // 
            // btnFolderBrowse
            // 
            this.btnFolderBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFolderBrowse.Location = new System.Drawing.Point(480, 6);
            this.btnFolderBrowse.Name = "btnFolderBrowse";
            this.btnFolderBrowse.Size = new System.Drawing.Size(75, 25);
            this.btnFolderBrowse.TabIndex = 1;
            this.btnFolderBrowse.Text = "&Browse";
            this.btnFolderBrowse.UseVisualStyleBackColor = true;
            this.btnFolderBrowse.Click += new System.EventHandler(this.btnFolderBrowse_Click);
            // 
            // txtFolderLocation
            // 
            this.txtFolderLocation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFolderLocation.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtFolderLocation.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this.txtFolderLocation.Location = new System.Drawing.Point(60, 8);
            this.txtFolderLocation.Name = "txtFolderLocation";
            this.txtFolderLocation.Size = new System.Drawing.Size(363, 23);
            this.txtFolderLocation.TabIndex = 0;
            this.txtFolderLocation.TextChanged += new System.EventHandler(this.txtFolderLocation_TextChanged);
            this.txtFolderLocation.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtFolderLocation_KeyDown);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chkAutoStart);
            this.panel1.Controls.Add(this.chkCopy);
            this.panel1.Controls.Add(this.chkShowReadme);
            this.panel1.Controls.Add(this.chkMakeBackup);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 477);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(560, 36);
            this.panel1.TabIndex = 3;
            // 
            // chkAutoStart
            // 
            this.chkAutoStart.AutoSize = true;
            this.chkAutoStart.Location = new System.Drawing.Point(280, 10);
            this.chkAutoStart.Name = "chkAutoStart";
            this.chkAutoStart.Size = new System.Drawing.Size(117, 19);
            this.chkAutoStart.TabIndex = 0;
            this.chkAutoStart.Text = "Enable Auto Start";
            this.chkAutoStart.UseVisualStyleBackColor = true;
            // 
            // chkCopy
            // 
            this.chkCopy.AutoSize = true;
            this.chkCopy.Location = new System.Drawing.Point(398, 10);
            this.chkCopy.Name = "chkCopy";
            this.chkCopy.Size = new System.Drawing.Size(101, 19);
            this.chkCopy.TabIndex = 0;
            this.chkCopy.Text = "Copy Package";
            this.chkCopy.UseVisualStyleBackColor = true;
            // 
            // chkShowReadme
            // 
            this.chkShowReadme.AutoSize = true;
            this.chkShowReadme.Location = new System.Drawing.Point(116, 10);
            this.chkShowReadme.Name = "chkShowReadme";
            this.chkShowReadme.Size = new System.Drawing.Size(165, 19);
            this.chkShowReadme.TabIndex = 0;
            this.chkShowReadme.Text = "Show Readme Documents";
            this.chkShowReadme.UseVisualStyleBackColor = true;
            // 
            // chkMakeBackup
            // 
            this.chkMakeBackup.AutoSize = true;
            this.chkMakeBackup.Checked = true;
            this.chkMakeBackup.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMakeBackup.Location = new System.Drawing.Point(12, 10);
            this.chkMakeBackup.Name = "chkMakeBackup";
            this.chkMakeBackup.Size = new System.Drawing.Size(97, 19);
            this.chkMakeBackup.TabIndex = 0;
            this.chkMakeBackup.Text = "Make Backup";
            this.chkMakeBackup.UseVisualStyleBackColor = true;
            // 
            // listView1
            // 
            this.listView1.CheckBoxes = true;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.pkgName,
            this.LastInstalled,
            this.Result});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.Location = new System.Drawing.Point(0, 36);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(560, 441);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            // 
            // pkgName
            // 
            this.pkgName.Text = "Package Name";
            this.pkgName.Width = 151;
            // 
            // LastInstalled
            // 
            this.LastInstalled.Text = "Last Installed";
            this.LastInstalled.Width = 116;
            // 
            // Result
            // 
            this.Result.Text = "Result";
            this.Result.Width = 259;
            // 
            // frmFullSystemUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(666, 513);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlFolderLocation);
            this.Controls.Add(this.pnlTools);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmFullSystemUpdate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "System Update";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmFullSystemUpdate_FormClosing);
            this.Load += new System.EventHandler(this.frmFullSystemUpdate_Load);
            this.Shown += new System.EventHandler(this.frmFullSystemUpdate_Shown);
            this.pnlTools.ResumeLayout(false);
            this.pnlFolderLocation.ResumeLayout(false);
            this.pnlFolderLocation.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private FWBS.OMS.UI.ListView listView1;
        private System.Windows.Forms.Panel pnlTools;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Panel pnlFolderLocation;
        private System.Windows.Forms.Button btnFolderBrowse;
        private System.Windows.Forms.TextBox txtFolderLocation;
        private System.Windows.Forms.ColumnHeader pkgName;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label labFolder;
        private System.Windows.Forms.Button btnCheckAll;
        private System.Windows.Forms.ColumnHeader Result;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox chkShowReadme;
        private System.Windows.Forms.CheckBox chkMakeBackup;
        private System.Windows.Forms.CheckBox chkCopy;
        private System.Windows.Forms.ColumnHeader LastInstalled;
        private System.Windows.Forms.Button btnUnCheckAll;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnParent;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnForward;
        private System.Windows.Forms.CheckBox chkAutoStart;
    }
}