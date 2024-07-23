namespace FWBS.OMS.Design.CodeBuilder
{
    partial class frmCodeBuilder_References
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCodeBuilder_References));
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Selected", System.Windows.Forms.HorizontalAlignment.Left);
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.dlgBrowse = new System.Windows.Forms.OpenFileDialog();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnBrowse = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.tabControl1 = new FWBS.OMS.UI.TabControl();
            this.tbReferences = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lvwReferences = new System.Windows.Forms.ListViewEx();
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel2 = new System.Windows.Forms.Panel();
            this.lnkRefresh = new System.Windows.Forms.LinkLabel();
            this.lnkIncludeGAC = new System.Windows.Forms.LinkLabel();
            this.prgProcessing = new System.Windows.Forms.ProgressBar();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.panel3.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tbReferences.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(379, 9);
            this.resourceLookup1.SetLookup(this.btnCancel, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnCancel", "Cance&l", ""));
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cance&l";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(460, 9);
            this.resourceLookup1.SetLookup(this.btnOK, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnOK", "&OK", ""));
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 25);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // dlgBrowse
            // 
            this.dlgBrowse.DefaultExt = "dll";
            this.dlgBrowse.Filter = "Assembly (*.dll)|*.dll|All Files (*.*)|*.*";
            this.dlgBrowse.Title = "Browse for Assembly";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "");
            this.imageList1.Images.SetKeyName(1, "");
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(4, 9);
            this.resourceLookup1.SetLookup(this.btnBrowse, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnBrowse", "&Browse", ""));
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 25);
            this.btnBrowse.TabIndex = 3;
            this.btnBrowse.Text = "&Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnOK);
            this.panel3.Controls.Add(this.btnCancel);
            this.panel3.Controls.Add(this.btnBrowse);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.panel3.Location = new System.Drawing.Point(10, 452);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(539, 44);
            this.panel3.TabIndex = 4;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tbReferences);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tabControl1.Location = new System.Drawing.Point(10, 10);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(539, 442);
            this.tabControl1.TabIndex = 2;
            // 
            // tbReferences
            // 
            this.tbReferences.Controls.Add(this.panel1);
            this.tbReferences.Location = new System.Drawing.Point(4, 24);
            this.resourceLookup1.SetLookup(this.tbReferences, new FWBS.OMS.UI.Windows.ResourceLookupItem("REFERENCES", "References", ""));
            this.tbReferences.Name = "tbReferences";
            this.tbReferences.Padding = new System.Windows.Forms.Padding(3);
            this.tbReferences.Size = new System.Drawing.Size(531, 414);
            this.tbReferences.TabIndex = 0;
            this.tbReferences.Text = "References";
            this.tbReferences.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lvwReferences);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.txtSearch);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(525, 408);
            this.panel1.TabIndex = 4;
            // 
            // lvwReferences
            // 
            this.lvwReferences.CheckBoxes = true;
            this.lvwReferences.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName});
            this.lvwReferences.Dock = System.Windows.Forms.DockStyle.Fill;
            listViewGroup1.Header = "Selected";
            listViewGroup1.Name = "grpSelected";
            this.lvwReferences.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1});
            this.lvwReferences.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvwReferences.Location = new System.Drawing.Point(0, 23);
            this.lvwReferences.Name = "lvwReferences";
            this.lvwReferences.Size = new System.Drawing.Size(525, 352);
            this.lvwReferences.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvwReferences.TabIndex = 0;
            this.lvwReferences.UseCompatibleStateImageBehavior = false;
            this.lvwReferences.View = System.Windows.Forms.View.Details;
            this.lvwReferences.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lvwReferences_ItemCheck);
            // 
            // colName
            // 
            this.colName.Width = 500;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lnkRefresh);
            this.panel2.Controls.Add(this.lnkIncludeGAC);
            this.panel2.Controls.Add(this.prgProcessing);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 375);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(525, 33);
            this.panel2.TabIndex = 7;
            // 
            // lnkRefresh
            // 
            this.lnkRefresh.AutoEllipsis = true;
            this.lnkRefresh.AutoSize = true;
            this.lnkRefresh.Location = new System.Drawing.Point(8, 9);
            this.resourceLookup1.SetLookup(this.lnkRefresh, new FWBS.OMS.UI.Windows.ResourceLookupItem("REFRESH", "Refresh", ""));
            this.lnkRefresh.Name = "lnkRefresh";
            this.lnkRefresh.Size = new System.Drawing.Size(46, 15);
            this.lnkRefresh.TabIndex = 1;
            this.lnkRefresh.TabStop = true;
            this.lnkRefresh.Text = "Refresh";
            this.lnkRefresh.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkRefresh_LinkClicked);
            // 
            // lnkIncludeGAC
            // 
            this.lnkIncludeGAC.AutoEllipsis = true;
            this.lnkIncludeGAC.AutoSize = true;
            this.lnkIncludeGAC.Location = new System.Drawing.Point(72, 9);
            this.resourceLookup1.SetLookup(this.lnkIncludeGAC, new FWBS.OMS.UI.Windows.ResourceLookupItem("SHOWALL", "Show All", ""));
            this.lnkIncludeGAC.Name = "lnkIncludeGAC";
            this.lnkIncludeGAC.Size = new System.Drawing.Size(53, 15);
            this.lnkIncludeGAC.TabIndex = 0;
            this.lnkIncludeGAC.TabStop = true;
            this.lnkIncludeGAC.Text = "Show All";
            this.lnkIncludeGAC.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkIncludeGAC_LinkClicked);
            // 
            // prgProcessing
            // 
            this.prgProcessing.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.prgProcessing.Location = new System.Drawing.Point(312, 7);
            this.prgProcessing.Name = "prgProcessing";
            this.prgProcessing.Size = new System.Drawing.Size(201, 18);
            this.prgProcessing.TabIndex = 6;
            // 
            // txtSearch
            // 
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtSearch.Location = new System.Drawing.Point(0, 0);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(525, 23);
            this.txtSearch.TabIndex = 2;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // frmCodeBuilder_References
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(559, 496);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel3);
            this.resourceLookup1.SetLookup(this, new FWBS.OMS.UI.Windows.ResourceLookupItem("REFERENCES", "References", ""));
            this.Name = "frmCodeBuilder_References";
            this.Padding = new System.Windows.Forms.Padding(10, 10, 10, 0);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "References";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCodeBuilder_References_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmCodeBuilder_References_FormClosed);
            this.Load += new System.EventHandler(this.frmCodeBuilder_References_Load);
            this.panel3.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tbReferences.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private FWBS.OMS.UI.Windows.ResourceLookup resourceLookup1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private FWBS.OMS.UI.TabControl tabControl1;
        private System.Windows.Forms.TabPage tbReferences;
        private System.Windows.Forms.OpenFileDialog dlgBrowse;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListViewEx lvwReferences;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.ProgressBar prgProcessing;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.LinkLabel lnkRefresh;
        private System.Windows.Forms.LinkLabel lnkIncludeGAC;
        private System.Windows.Forms.Panel panel3;
    }
}