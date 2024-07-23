namespace FWBS.OMS.UI.UserControls.Common
{
    partial class ucSearchPagination
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.TotalCount = new System.Windows.Forms.Label();
            this.lblPrevPage = new System.Windows.Forms.Label();
            this.btnPrevPage = new System.Windows.Forms.Button();
            this.lblNext = new System.Windows.Forms.Label();
            this.btnNext = new System.Windows.Forms.Button();
            this.lblOf = new System.Windows.Forms.Label();
            this.cmbPages = new System.Windows.Forms.ComboBox();
            this.lblItemsPerPage = new System.Windows.Forms.Label();
            this.cmbPageSize = new System.Windows.Forms.ComboBox();
            this.lblTotal = new System.Windows.Forms.Label();
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 12;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.TotalCount, 11, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblPrevPage, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnPrevPage, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblNext, 7, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnNext, 8, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblOf, 6, 0);
            this.tableLayoutPanel1.Controls.Add(this.cmbPages, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblItemsPerPage, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.cmbPageSize, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblTotal, 10, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(852, 42);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // TotalCount
            // 
            this.TotalCount.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.TotalCount.AutoSize = true;
            this.TotalCount.Location = new System.Drawing.Point(812, 12);
            this.TotalCount.Name = "TotalCount";
            this.TotalCount.Padding = new System.Windows.Forms.Padding(0, 3, 24, 0);
            this.TotalCount.Size = new System.Drawing.Size(37, 18);
            this.TotalCount.TabIndex = 17;
            this.TotalCount.Text = "0";
            // 
            // lblPrevPage
            // 
            this.lblPrevPage.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblPrevPage.AutoSize = true;
            this.lblPrevPage.Location = new System.Drawing.Point(408, 12);
            this.resourceLookup1.SetLookup(this.lblPrevPage, new FWBS.OMS.UI.Windows.ResourceLookupItem("PrevPage", "Prev Page", ""));
            this.lblPrevPage.Name = "lblPrevPage";
            this.lblPrevPage.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.lblPrevPage.Size = new System.Drawing.Size(0, 18);
            this.lblPrevPage.TabIndex = 13;
            // 
            // btnPrevPage
            // 
            this.btnPrevPage.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnPrevPage.BackColor = System.Drawing.Color.Transparent;
            this.btnPrevPage.FlatAppearance.BorderSize = 0;
            this.btnPrevPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrevPage.Font = new System.Drawing.Font("Segoe UI Symbol", 9F);
            this.btnPrevPage.Location = new System.Drawing.Point(375, 6);
            this.btnPrevPage.Margin = new System.Windows.Forms.Padding(0);
            this.btnPrevPage.Name = "btnPrevPage";
            this.btnPrevPage.Size = new System.Drawing.Size(30, 30);
            this.btnPrevPage.TabIndex = 12;
            this.btnPrevPage.Text = "<";
            this.btnPrevPage.UseVisualStyleBackColor = false;
            this.btnPrevPage.Click += new System.EventHandler(this.btnPrevPage_Click);
            // 
            // lblNext
            // 
            this.lblNext.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblNext.AutoSize = true;
            this.lblNext.Location = new System.Drawing.Point(494, 12);
            this.resourceLookup1.SetLookup(this.lblNext, new FWBS.OMS.UI.Windows.ResourceLookupItem("NextBtn", "Next", ""));
            this.lblNext.Name = "lblNext";
            this.lblNext.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.lblNext.Size = new System.Drawing.Size(0, 18);
            this.lblNext.TabIndex = 9;
            // 
            // btnNext
            // 
            this.btnNext.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnNext.BackColor = System.Drawing.Color.Transparent;
            this.btnNext.FlatAppearance.BorderSize = 0;
            this.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNext.Font = new System.Drawing.Font("Segoe UI Symbol", 9F);
            this.btnNext.Location = new System.Drawing.Point(497, 6);
            this.btnNext.Margin = new System.Windows.Forms.Padding(0);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(30, 30);
            this.btnNext.TabIndex = 7;
            this.btnNext.Text = ">";
            this.btnNext.UseVisualStyleBackColor = false;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // lblOf
            // 
            this.lblOf.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblOf.AutoSize = true;
            this.lblOf.Location = new System.Drawing.Point(484, 12);
            this.lblOf.Name = "lblOf";
            this.lblOf.Padding = new System.Windows.Forms.Padding(2, 3, 2, 0);
            this.lblOf.Size = new System.Drawing.Size(4, 18);
            this.lblOf.TabIndex = 10;
            // 
            // cmbPages
            // 
            this.cmbPages.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cmbPages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPages.FormattingEnabled = true;
            this.cmbPages.Location = new System.Drawing.Point(419, 9);
            this.cmbPages.Name = "cmbPages";
            this.cmbPages.Size = new System.Drawing.Size(54, 23);
            this.cmbPages.TabIndex = 11;
            this.cmbPages.SelectedIndexChanged += new System.EventHandler(this.cmbPages_SelectedIndexChanged);
            // 
            // lblItemsPerPage
            // 
            this.lblItemsPerPage.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblItemsPerPage.AutoSize = true;
            this.lblItemsPerPage.Location = new System.Drawing.Point(3, 12);
            this.resourceLookup1.SetLookup(this.lblItemsPerPage, new FWBS.OMS.UI.Windows.ResourceLookupItem("ItemsPerPage", "Items per page", ""));
            this.lblItemsPerPage.Name = "lblItemsPerPage";
            this.lblItemsPerPage.Padding = new System.Windows.Forms.Padding(24, 3, 0, 0);
            this.lblItemsPerPage.Size = new System.Drawing.Size(24, 18);
            this.lblItemsPerPage.TabIndex = 14;
            // 
            // cmbPageSize
            // 
            this.cmbPageSize.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbPageSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPageSize.FormattingEnabled = true;
            this.cmbPageSize.Location = new System.Drawing.Point(33, 10);
            this.cmbPageSize.Name = "cmbPageSize";
            this.cmbPageSize.Size = new System.Drawing.Size(64, 23);
            this.cmbPageSize.TabIndex = 15;
            this.cmbPageSize.SelectedValueChanged += new System.EventHandler(this.cmbPageSize_SelectedValueChanged);
            // 
            // lblTotal
            // 
            this.lblTotal.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(806, 12);
            this.resourceLookup1.SetLookup(this.lblTotal, new FWBS.OMS.UI.Windows.ResourceLookupItem("Total:", "Total:", ""));
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.lblTotal.Size = new System.Drawing.Size(0, 18);
            this.lblTotal.TabIndex = 16;
            // 
            // ucSearchPagination
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "ucSearchPagination";
            this.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.Size = new System.Drawing.Size(852, 44);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblPrevPage;
        private System.Windows.Forms.Button btnPrevPage;
        private System.Windows.Forms.Label lblNext;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Label lblOf;
        private System.Windows.Forms.ComboBox cmbPages;
        private System.Windows.Forms.Label lblItemsPerPage;
        private System.Windows.Forms.ComboBox cmbPageSize;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label TotalCount;
        private Windows.ResourceLookup resourceLookup1;
    }
}
