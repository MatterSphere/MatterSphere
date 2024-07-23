namespace FWBS.OMS.UI.Windows.DocumentManagement
{
    partial class OfflineDocumentPicker
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OfflineDocumentPicker));
            this.listView1 = new FWBS.OMS.UI.ListView();
            this.colDocId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colVerLabel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDocDesc = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDocType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colClientFileNo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colclname = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colFileDesc = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colCacheDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colCreated = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDocCheckedOut = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDocCheckedOutLocation = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.pnlList = new System.Windows.Forms.Panel();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.chkChanged = new System.Windows.Forms.CheckBox();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.storage = new FWBS.OMS.UI.Windows.ucFormStorage(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblCount = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pnlDesign = new System.Windows.Forms.Panel();
            this.pnDetails = new FWBS.OMS.UI.Windows.ucPanelNav();
            this.ucNavRichText1 = new FWBS.OMS.UI.Windows.ucNavRichText();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pnlDTFilter = new System.Windows.Forms.Panel();
            this.cboDocType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pnlDBFilter = new System.Windows.Forms.Panel();
            this.cboDatabase = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.pnlList.SuspendLayout();
            this.pnlTop.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlDesign.SuspendLayout();
            this.pnDetails.SuspendLayout();
            this.panel2.SuspendLayout();
            this.pnlDTFilter.SuspendLayout();
            this.pnlDBFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colDocId,
            this.colVerLabel,
            this.colDocDesc,
            this.colDocType,
            this.colClientFileNo,
            this.colclname,
            this.colFileDesc,
            this.colCacheDate,
            this.colCreated,
            this.colDocCheckedOut,
            this.colDocCheckedOutLocation});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(611, 322);
            this.listView1.SmallImageList = this.imageList1;
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.VirtualMode = true;
            this.listView1.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.ListView_ColumnClick);
            this.listView1.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.listView1_RetrieveVirtualItem);
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            this.listView1.VirtualItemsSelectionRangeChanged += new System.Windows.Forms.ListViewVirtualItemsSelectionRangeChangedEventHandler(this.ListView_VirtualItemsSelectionRangeChanged);
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            this.listView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listView1_KeyDown);
            // 
            // colDocId
            // 
            this.colDocId.Tag = "DocId";
            this.colDocId.Text = "Document Id";
            this.colDocId.Width = 101;
            // 
            // colVerLabel
            // 
            this.colVerLabel.Tag = "VerLabel";
            this.colVerLabel.Text = "Version";
            // 
            // colDocDesc
            // 
            this.colDocDesc.Tag = "DocDesc";
            this.colDocDesc.Text = "Description";
            this.colDocDesc.Width = 200;
            // 
            // colDocType
            // 
            this.colDocType.Tag = "DocType";
            this.colDocType.Text = "Document Type";
            this.colDocType.Width = 100;
            // 
            // colClientFileNo
            // 
            this.colClientFileNo.Tag = "ClientFileNo";
            this.colClientFileNo.Text = "Reference";
            this.colClientFileNo.Width = 100;
            // 
            // colclname
            // 
            this.colclname.Tag = "clname";
            this.colclname.Text = "Client Name";
            this.colclname.Width = 150;
            // 
            // colFileDesc
            // 
            this.colFileDesc.Tag = "FileDesc";
            this.colFileDesc.Text = "File Description";
            this.colFileDesc.Width = 200;
            // 
            // colCacheDate
            // 
            this.colCacheDate.Tag = "CacheDate";
            this.colCacheDate.Text = "Cached";
            this.colCacheDate.Width = 120;
            // 
            // colCreated
            // 
            this.colCreated.Tag = "created";
            this.colCreated.Text = "Created";
            this.colCreated.Width = 120;
            // 
            // colDocCheckedOut
            // 
            this.colDocCheckedOut.Tag = "DocCheckedOut";
            this.colDocCheckedOut.Text = "Checked Out";
            this.colDocCheckedOut.Width = 120;
            // 
            // colDocCheckedOutLocation
            // 
            this.colDocCheckedOutLocation.Tag = "DocCheckedOutLocation";
            this.colDocCheckedOutLocation.Text = "Location";
            this.colDocCheckedOutLocation.Width = 500;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // pnlList
            // 
            this.pnlList.Controls.Add(this.listView1);
            this.pnlList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlList.Location = new System.Drawing.Point(5, 134);
            this.pnlList.Name = "pnlList";
            this.pnlList.Size = new System.Drawing.Size(611, 322);
            this.pnlList.TabIndex = 1;
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.btnDelete);
            this.pnlTop.Controls.Add(this.btnOK);
            this.pnlTop.Controls.Add(this.btnCancel);
            this.pnlTop.Controls.Add(this.checkBox1);
            this.pnlTop.Controls.Add(this.chkChanged);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(5, 5);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(611, 33);
            this.pnlTop.TabIndex = 2;
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Location = new System.Drawing.Point(367, 4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 24);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "&Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(529, 4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 24);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "&Open";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(448, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 24);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cance&l";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox1.Location = new System.Drawing.Point(0, 8);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(96, 19);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "Checked Out";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // chkChanged
            // 
            this.chkChanged.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkChanged.Location = new System.Drawing.Point(104, 8);
            this.chkChanged.Name = "chkChanged";
            this.chkChanged.Size = new System.Drawing.Size(74, 19);
            this.chkChanged.TabIndex = 1;
            this.chkChanged.Text = "Changed";
            this.chkChanged.UseVisualStyleBackColor = true;
            this.chkChanged.CheckedChanged += new System.EventHandler(this.chkChanged_CheckedChanged);
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "Tick");
            this.imageList2.Images.SetKeyName(1, "Person");
            // 
            // storage
            // 
            this.storage.DefaultPercentageHeight = 70;
            this.storage.DefaultPercentageWidth = 70;
            this.storage.FormToStore = this;
            this.storage.Position = false;
            this.storage.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.storage.State = false;
            this.storage.UniqueID = "Forms\\OfflineDocuments";
            this.storage.Version = ((long)(1));
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtFilter);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(5, 88);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(611, 25);
            this.panel1.TabIndex = 0;
            // 
            // txtFilter
            // 
            this.txtFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFilter.Location = new System.Drawing.Point(96, 0);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(515, 23);
            this.txtFilter.TabIndex = 0;
            this.txtFilter.TextChanged += new System.EventHandler(this.txtFilter_TextChanged);
            this.txtFilter.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtFilter_KeyDown);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Quick Search";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCount
            // 
            this.lblCount.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblCount.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCount.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblCount.Location = new System.Drawing.Point(5, 113);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(611, 21);
            this.lblCount.TabIndex = 4;
            this.lblCount.Tag = "Document Count : {0}";
            this.lblCount.Text = "Document Count : {0}";
            this.lblCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // pnlDesign
            // 
            this.pnlDesign.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(122)))), ((int)(((byte)(215)))));
            this.pnlDesign.Controls.Add(this.pnDetails);
            this.pnlDesign.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlDesign.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlDesign.Location = new System.Drawing.Point(0, 0);
            this.pnlDesign.Name = "pnlDesign";
            this.pnlDesign.Padding = new System.Windows.Forms.Padding(8);
            this.pnlDesign.Size = new System.Drawing.Size(183, 461);
            this.pnlDesign.TabIndex = 9;
            // 
            // pnDetails
            // 
            this.pnDetails.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(223)))), ((int)(((byte)(247)))));
            this.pnDetails.BlendColor1 = System.Drawing.Color.Empty;
            this.pnDetails.BlendColor2 = System.Drawing.Color.Empty;
            this.pnDetails.Controls.Add(this.ucNavRichText1);
            this.pnDetails.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnDetails.ExpandedHeight = 42;
            this.pnDetails.HeaderColor = System.Drawing.Color.Empty;
            this.pnDetails.Location = new System.Drawing.Point(8, 8);
            this.pnDetails.Name = "pnDetails";
            this.pnDetails.Size = new System.Drawing.Size(167, 42);
            this.pnDetails.TabIndex = 2;
            this.pnDetails.Text = "Details";
            this.pnDetails.Theme = FWBS.Common.UI.Windows.ExtColorTheme.Auto;
            // 
            // ucNavRichText1
            // 
            this.ucNavRichText1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucNavRichText1.Location = new System.Drawing.Point(0, 24);
            this.ucNavRichText1.Name = "ucNavRichText1";
            this.ucNavRichText1.Padding = new System.Windows.Forms.Padding(3);
            this.ucNavRichText1.PanelBackColor = System.Drawing.Color.Empty;
            this.ucNavRichText1.Rtf = "{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang2057{\\fonttbl{\\f0\\fnil\\fcharset0 Segoe UI;" +
    "}}\r\n\\viewkind4\\uc1\\pard\\f0\\fs18\\par\r\n}\r\n";
            this.ucNavRichText1.Size = new System.Drawing.Size(167, 11);
            this.ucNavRichText1.TabIndex = 15;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.pnlList);
            this.panel2.Controls.Add(this.lblCount);
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Controls.Add(this.pnlDTFilter);
            this.panel2.Controls.Add(this.pnlDBFilter);
            this.panel2.Controls.Add(this.pnlTop);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.panel2.Location = new System.Drawing.Point(183, 0);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(5);
            this.panel2.Size = new System.Drawing.Size(621, 461);
            this.panel2.TabIndex = 0;
            // 
            // pnlDTFilter
            // 
            this.pnlDTFilter.Controls.Add(this.cboDocType);
            this.pnlDTFilter.Controls.Add(this.label3);
            this.pnlDTFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDTFilter.Location = new System.Drawing.Point(5, 63);
            this.pnlDTFilter.Name = "pnlDTFilter";
            this.pnlDTFilter.Size = new System.Drawing.Size(611, 25);
            this.pnlDTFilter.TabIndex = 4;
            // 
            // cboDocType
            // 
            this.cboDocType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDocType.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cboDocType.FormattingEnabled = true;
            this.cboDocType.Location = new System.Drawing.Point(96, 0);
            this.cboDocType.Name = "cboDocType";
            this.cboDocType.Size = new System.Drawing.Size(220, 23);
            this.cboDocType.TabIndex = 0;
            this.cboDocType.SelectedIndexChanged += new System.EventHandler(this.cboDocType_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 23);
            this.label3.TabIndex = 0;
            this.label3.Text = "Document Type";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlDBFilter
            // 
            this.pnlDBFilter.Controls.Add(this.cboDatabase);
            this.pnlDBFilter.Controls.Add(this.label2);
            this.pnlDBFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDBFilter.Location = new System.Drawing.Point(5, 38);
            this.pnlDBFilter.Name = "pnlDBFilter";
            this.pnlDBFilter.Size = new System.Drawing.Size(611, 25);
            this.pnlDBFilter.TabIndex = 3;
            // 
            // cboDatabase
            // 
            this.cboDatabase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDatabase.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cboDatabase.FormattingEnabled = true;
            this.cboDatabase.Location = new System.Drawing.Point(96, 0);
            this.cboDatabase.Name = "cboDatabase";
            this.cboDatabase.Size = new System.Drawing.Size(220, 23);
            this.cboDatabase.TabIndex = 0;
            this.cboDatabase.SelectedIndexChanged += new System.EventHandler(this.cboDatabase_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 23);
            this.label2.TabIndex = 0;
            this.label2.Text = "Database";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // OfflineDocumentPicker
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(804, 461);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.pnlDesign);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OfflineDocumentPicker";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Offline Documents";
            this.Load += new System.EventHandler(this.OfflineDocumentPicker_Load);
            this.Shown += new System.EventHandler(this.OfflineDocumentPicker_Shown);
            this.pnlList.ResumeLayout(false);
            this.pnlTop.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlDesign.ResumeLayout(false);
            this.pnDetails.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.pnlDTFilter.ResumeLayout(false);
            this.pnlDBFilter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private FWBS.OMS.UI.ListView listView1;
        private System.Windows.Forms.Panel pnlList;
        private System.Windows.Forms.ColumnHeader colDocId;
        private System.Windows.Forms.ColumnHeader colVerLabel;
        private System.Windows.Forms.ColumnHeader colDocDesc;
        private System.Windows.Forms.ColumnHeader colClientFileNo;
        private System.Windows.Forms.ColumnHeader colclname;
        private System.Windows.Forms.ColumnHeader colFileDesc;
        private System.Windows.Forms.ColumnHeader colDocType;
        private System.Windows.Forms.ColumnHeader colCreated;
        private System.Windows.Forms.ColumnHeader colDocCheckedOut;
        private System.Windows.Forms.ColumnHeader colDocCheckedOutLocation;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ImageList imageList2;
        private FWBS.OMS.UI.Windows.ucFormStorage storage;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.TextBox txtFilter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel panel2;
        protected System.Windows.Forms.Panel pnlDesign;
        private ucPanelNav pnDetails;
        private ucNavRichText ucNavRichText1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Panel pnlDTFilter;
        private System.Windows.Forms.ComboBox cboDocType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel pnlDBFilter;
        private System.Windows.Forms.ComboBox cboDatabase;
        private System.Windows.Forms.Label label2;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ColumnHeader colCacheDate;
        private System.Windows.Forms.CheckBox chkChanged;
        private System.Windows.Forms.Button btnDelete;
    }
}