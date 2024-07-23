namespace FWBS.OMS.UI.Windows.Admin
{
    partial class ucPDFImporter
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.grpPrecCats = new System.Windows.Forms.GroupBox();
            this.xpPrecMinorCat = new FWBS.OMS.UI.Windows.eXPComboBoxCodeLookup();
            this.xpPrecSubCat = new FWBS.OMS.UI.Windows.eXPComboBoxCodeLookup();
            this.xpPrecCat = new FWBS.OMS.UI.Windows.eXPComboBoxCodeLookup();
            this.lvPDFList = new FWBS.OMS.UI.ListView();
            this.chkOverwriteAll = new FWBS.Common.UI.Windows.eCheckBox2();
            this.grpPrecCats.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(10, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(268, 21);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "PDF Import Menu";
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(309, 44);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(208, 33);
            this.btnSelect.TabIndex = 4;
            this.btnSelect.Text = "Select PDF Files";
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(309, 303);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(208, 33);
            this.btnClear.TabIndex = 5;
            this.btnClear.Text = "Clear List";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(309, 264);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(208, 33);
            this.btnImport.TabIndex = 6;
            this.btnImport.Text = "Import";
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // grpPrecCats
            // 
            this.grpPrecCats.Controls.Add(this.xpPrecMinorCat);
            this.grpPrecCats.Controls.Add(this.xpPrecSubCat);
            this.grpPrecCats.Controls.Add(this.xpPrecCat);
            this.grpPrecCats.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpPrecCats.Location = new System.Drawing.Point(309, 98);
            this.grpPrecCats.Name = "grpPrecCats";
            this.grpPrecCats.Size = new System.Drawing.Size(410, 147);
            this.grpPrecCats.TabIndex = 7;
            this.grpPrecCats.TabStop = false;
            this.grpPrecCats.Text = "Import into these precedent categories";
            // 
            // xpPrecMinorCat
            // 
            this.xpPrecMinorCat.ActiveSearchEnabled = true;
            this.xpPrecMinorCat.AddNotSet = true;
            this.xpPrecMinorCat.CaptionWidth = 100;
            this.xpPrecMinorCat.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xpPrecMinorCat.IsDirty = false;
            this.xpPrecMinorCat.Location = new System.Drawing.Point(16, 108);
            this.xpPrecMinorCat.MaxLength = 0;
            this.xpPrecMinorCat.Name = "xpPrecMinorCat";
            this.xpPrecMinorCat.NotSetCode = "NOTSET";
            this.xpPrecMinorCat.NotSetType = "RESOURCE";
            this.xpPrecMinorCat.Padding = new System.Windows.Forms.Padding(100, 0, 0, 0);
            this.xpPrecMinorCat.Size = new System.Drawing.Size(356, 21);
            this.xpPrecMinorCat.TabIndex = 2;
            this.xpPrecMinorCat.TerminologyParse = false;
            this.xpPrecMinorCat.Text = "Minor Category";
            this.xpPrecMinorCat.Type = "PRECMINORCAT";
            // 
            // xpPrecSubCat
            // 
            this.xpPrecSubCat.ActiveSearchEnabled = true;
            this.xpPrecSubCat.AddNotSet = true;
            this.xpPrecSubCat.CaptionWidth = 100;
            this.xpPrecSubCat.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xpPrecSubCat.IsDirty = false;
            this.xpPrecSubCat.Location = new System.Drawing.Point(16, 71);
            this.xpPrecSubCat.MaxLength = 0;
            this.xpPrecSubCat.Name = "xpPrecSubCat";
            this.xpPrecSubCat.NotSetCode = "NOTSET";
            this.xpPrecSubCat.NotSetType = "RESOURCE";
            this.xpPrecSubCat.Padding = new System.Windows.Forms.Padding(100, 0, 0, 0);
            this.xpPrecSubCat.Size = new System.Drawing.Size(356, 21);
            this.xpPrecSubCat.TabIndex = 1;
            this.xpPrecSubCat.TerminologyParse = false;
            this.xpPrecSubCat.Text = "Sub Category";
            this.xpPrecSubCat.Type = "PRECSUBCAT";
            // 
            // xpPrecCat
            // 
            this.xpPrecCat.ActiveSearchEnabled = true;
            this.xpPrecCat.AddNotSet = true;
            this.xpPrecCat.CaptionWidth = 100;
            this.xpPrecCat.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xpPrecCat.IsDirty = false;
            this.xpPrecCat.Location = new System.Drawing.Point(16, 33);
            this.xpPrecCat.MaxLength = 0;
            this.xpPrecCat.Name = "xpPrecCat";
            this.xpPrecCat.NotSetCode = "NOTSET";
            this.xpPrecCat.NotSetType = "RESOURCE";
            this.xpPrecCat.Padding = new System.Windows.Forms.Padding(100, 0, 0, 0);
            this.xpPrecCat.Size = new System.Drawing.Size(356, 21);
            this.xpPrecCat.TabIndex = 0;
            this.xpPrecCat.TerminologyParse = false;
            this.xpPrecCat.Text = "Category";
            this.xpPrecCat.Type = "PRECCAT";
            // 
            // lvPDFList
            // 
            this.lvPDFList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvPDFList.Location = new System.Drawing.Point(13, 44);
            this.lvPDFList.Name = "lvPDFList";
            this.lvPDFList.Size = new System.Drawing.Size(265, 462);
            this.lvPDFList.TabIndex = 0;
            this.lvPDFList.UseCompatibleStateImageBehavior = false;
            this.lvPDFList.View = System.Windows.Forms.View.Details;
            // 
            // chkOverwriteAll
            // 
            this.chkOverwriteAll.AutoSize = true;
            this.chkOverwriteAll.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkOverwriteAll.Dirty = false;
            this.chkOverwriteAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkOverwriteAll.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.chkOverwriteAll.IsDirty = false;
            this.chkOverwriteAll.Location = new System.Drawing.Point(523, 273);
            this.chkOverwriteAll.Name = "chkOverwriteAll";
            this.chkOverwriteAll.omsDesignMode = false;
            this.chkOverwriteAll.Size = new System.Drawing.Size(85, 17);
            this.chkOverwriteAll.TabIndex = 8;
            this.chkOverwriteAll.Text = "Overwrite All";
            this.chkOverwriteAll.UseVisualStyleBackColor = true;
            // 
            // ucPDFImporter
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.chkOverwriteAll);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lvPDFList);
            this.Controls.Add(this.grpPrecCats);
            this.Name = "ucPDFImporter";
            this.Size = new System.Drawing.Size(785, 526);
            this.Load += new System.EventHandler(this.ucPDFImporter_Load);
            this.grpPrecCats.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ListView lvPDFList;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.GroupBox grpPrecCats;
        private eXPComboBoxCodeLookup xpPrecCat;
        private eXPComboBoxCodeLookup xpPrecSubCat;
        private Common.UI.Windows.eCheckBox2 chkOverwriteAll;
        private eXPComboBoxCodeLookup xpPrecMinorCat;
    }
}
