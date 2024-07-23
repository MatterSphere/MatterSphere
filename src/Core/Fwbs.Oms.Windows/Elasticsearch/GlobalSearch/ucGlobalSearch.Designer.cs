namespace FWBS.OMS.UI.Elasticsearch.GlobalSearch
{
    partial class ucGlobalSearch
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.TitleContainer = new System.Windows.Forms.Panel();
            this.searchBox = new FWBS.OMS.UI.Windows.ucSearchTextControl();
            this.dataView = new FWBS.OMS.UI.Windows.DataGridViewEx();
            this.columnIcon = new System.Windows.Forms.DataGridViewImageColumn();
            this.columnName = new FWBS.OMS.UI.Elasticsearch.DataGridViewHighlightsColumn();
            this.columnDateModified = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnDocumentType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnDetails = new FWBS.OMS.UI.Elasticsearch.DataGridViewHighlightsColumn();
            this.columnOmsType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnFileId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnClientId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnContactId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnDocumentId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnAssociateId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnExtension = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnMatterSphereId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.searchPagination = new FWBS.OMS.UI.UserControls.Common.ucSearchPagination();
            this.TitleContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataView)).BeginInit();
            this.SuspendLayout();
            // 
            // TitleContainer
            // 
            this.TitleContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(17)))), ((int)(((byte)(76)))));
            this.TitleContainer.Controls.Add(this.searchBox);
            this.TitleContainer.Dock = System.Windows.Forms.DockStyle.Top;
            this.TitleContainer.Location = new System.Drawing.Point(0, 0);
            this.TitleContainer.Name = "TitleContainer";
            this.TitleContainer.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.TitleContainer.Size = new System.Drawing.Size(600, 48);
            this.TitleContainer.TabIndex = 0;
            // 
            // searchBox
            // 
            this.searchBox.BackColor = System.Drawing.Color.White;
            this.searchBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.searchBox.Location = new System.Drawing.Point(12, 8);
            this.searchBox.Name = "searchBox";
            this.searchBox.Size = new System.Drawing.Size(576, 32);
            this.searchBox.TabIndex = 0;
            // 
            // dataView
            // 
            this.dataView.AllowUserToAddRows = false;
            this.dataView.AllowUserToDeleteRows = false;
            this.dataView.AllowUserToResizeRows = false;
            this.dataView.BackgroundColor = System.Drawing.Color.White;
            this.dataView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataView.CaptionLabel = null;
            this.dataView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dataView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataView.ColumnHeadersHeight = 42;
            this.dataView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnIcon,
            this.columnName,
            this.columnDateModified,
            this.columnDocumentType,
            this.columnDetails,
            this.columnOmsType,
            this.columnFileId,
            this.columnClientId,
            this.columnContactId,
            this.columnDocumentId,
            this.columnAssociateId,
            this.columnExtension,
            this.columnMatterSphereId});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(224)))), ((int)(((byte)(242)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataView.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataView.EnableHeadersVisualStyles = false;
            this.dataView.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dataView.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.dataView.Location = new System.Drawing.Point(0, 48);
            this.dataView.Margin = new System.Windows.Forms.Padding(0);
            this.dataView.MultiSelect = false;
            this.dataView.Name = "dataView";
            this.dataView.ReadOnly = true;
            this.dataView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dataView.RowHeadersVisible = false;
            this.dataView.RowTemplate.Height = 42;
            this.dataView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataView.Size = new System.Drawing.Size(600, 508);
            this.dataView.TabIndex = 1;
            this.dataView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataView_CellDoubleClick);
            this.dataView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataView_KeyDown);
            this.dataView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataView_MouseDown);
            this.dataView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dataView_MouseUp);
            this.dataView.SortChanged += new System.EventHandler<FWBS.OMS.UI.Windows.DataGridViewEx.SortDataEventArgs>(this.dataView_SortChanged);
            // 
            // columnIcon
            // 
            this.columnIcon.HeaderText = "";
            this.columnIcon.MinimumWidth = 60;
            this.columnIcon.Name = "columnIcon";
            this.columnIcon.ReadOnly = true;
            this.columnIcon.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.columnIcon.Width = 60;
            // 
            // columnName
            // 
            this.columnName.HeaderText = "columnName";
            this.columnName.MinimumWidth = 50;
            this.columnName.Name = "columnName";
            this.columnName.ReadOnly = true;
            this.columnName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.columnName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.columnName.Width = 150;
            // 
            // columnDateModified
            // 
            this.columnDateModified.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.columnDateModified.HeaderText = "columnDateModified";
            this.columnDateModified.MinimumWidth = 50;
            this.columnDateModified.Name = "columnDateModified";
            this.columnDateModified.ReadOnly = true;
            this.columnDateModified.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.columnDateModified.Width = 130;
            // 
            // columnDocumentType
            // 
            this.columnDocumentType.HeaderText = "columnDocumentType";
            this.columnDocumentType.MinimumWidth = 50;
            this.columnDocumentType.Name = "columnDocumentType";
            this.columnDocumentType.ReadOnly = true;
            this.columnDocumentType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.columnDocumentType.Width = 150;
            // 
            // columnDetails
            // 
            this.columnDetails.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnDetails.HeaderText = "columnDetails";
            this.columnDetails.MinimumWidth = 150;
            this.columnDetails.Name = "columnDetails";
            this.columnDetails.ReadOnly = true;
            this.columnDetails.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // columnOmsType
            // 
            this.columnOmsType.HeaderText = "";
            this.columnOmsType.Name = "columnOmsType";
            this.columnOmsType.ReadOnly = true;
            this.columnOmsType.Visible = false;
            // 
            // columnFileId
            // 
            this.columnFileId.HeaderText = "";
            this.columnFileId.Name = "columnFileId";
            this.columnFileId.ReadOnly = true;
            this.columnFileId.Visible = false;
            // 
            // columnClientId
            // 
            this.columnClientId.HeaderText = "";
            this.columnClientId.Name = "columnClientId";
            this.columnClientId.ReadOnly = true;
            this.columnClientId.Visible = false;
            // 
            // columnContactId
            // 
            this.columnContactId.HeaderText = "";
            this.columnContactId.Name = "columnContactId";
            this.columnContactId.ReadOnly = true;
            this.columnContactId.Visible = false;
            // 
            // columnDocumentId
            // 
            this.columnDocumentId.HeaderText = "";
            this.columnDocumentId.Name = "columnDocumentId";
            this.columnDocumentId.ReadOnly = true;
            this.columnDocumentId.Visible = false;
            // 
            // columnAssociateId
            // 
            this.columnAssociateId.HeaderText = "";
            this.columnAssociateId.Name = "columnAssociateId";
            this.columnAssociateId.ReadOnly = true;
            this.columnAssociateId.Visible = false;
            // 
            // columnExtension
            // 
            this.columnExtension.HeaderText = "";
            this.columnExtension.Name = "columnExtension";
            this.columnExtension.ReadOnly = true;
            this.columnExtension.Visible = false;
            // 
            // columnMatterSphereId
            // 
            this.columnMatterSphereId.HeaderText = "";
            this.columnMatterSphereId.Name = "columnMatterSphereId";
            this.columnMatterSphereId.ReadOnly = true;
            this.columnMatterSphereId.Visible = false;
            // 
            // searchPagination
            // 
            this.searchPagination.BackColor = System.Drawing.Color.White;
            this.searchPagination.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.searchPagination.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.searchPagination.Location = new System.Drawing.Point(0, 556);
            this.searchPagination.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.searchPagination.Name = "searchPagination";
            this.searchPagination.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.searchPagination.Size = new System.Drawing.Size(600, 44);
            this.searchPagination.TabIndex = 0;
            // 
            // ucGlobalSearch
            // 
            this.Controls.Add(this.dataView);
            this.Controls.Add(this.searchPagination);
            this.Controls.Add(this.TitleContainer);
            this.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.Name = "ucGlobalSearch";
            this.Size = new System.Drawing.Size(600, 600);
            this.DpiChangedAfterParent += new System.EventHandler(this.ucGlobalSearch_DpiChangedAfterParent);
            this.TitleContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel TitleContainer;
        private FWBS.OMS.UI.Windows.ucSearchTextControl searchBox;
        private UserControls.Common.ucSearchPagination searchPagination;
        private Windows.DataGridViewEx dataView;
        private System.Windows.Forms.DataGridViewImageColumn columnIcon;
        private DataGridViewHighlightsColumn columnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnDateModified;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnDocumentType;
        private DataGridViewHighlightsColumn columnDetails;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnOmsType;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnFileId;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnClientId;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnContactId;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnDocumentId;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnAssociateId;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnExtension;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnMatterSphereId;
    }
}
