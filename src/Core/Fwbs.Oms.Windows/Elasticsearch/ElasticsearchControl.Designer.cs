namespace FWBS.OMS.UI.Elasticsearch
{
    partial class ElasticsearchControl
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.filterPanel = new System.Windows.Forms.Panel();
            this.FilterContainer = new System.Windows.Forms.Panel();
            this.FacetGroupsPanel = new System.Windows.Forms.Panel();
            this.filterHeader = new System.Windows.Forms.Panel();
            this.lblFilter = new System.Windows.Forms.Label();
            this.btnCloseFilter = new System.Windows.Forms.Button();
            this.filterPanelLine = new System.Windows.Forms.Panel();
            this.previewPanel = new System.Windows.Forms.Panel();
            this.previewBackgroundPanel = new System.Windows.Forms.Panel();
            this.previewContainer = new System.Windows.Forms.Panel();
            this.documentPreview = new FWBS.OMS.UI.Windows.DocumentManagement.Addins.DocumentPreviewAddin();
            this.previewHeader = new System.Windows.Forms.Panel();
            this.lblPreview = new System.Windows.Forms.Label();
            this.btnClosePreview = new System.Windows.Forms.Button();
            this.previewPanelLine = new System.Windows.Forms.Panel();
            this.splitter = new System.Windows.Forms.Splitter();
            this.gridPanel = new System.Windows.Forms.Panel();
            this.dataView = new FWBS.OMS.UI.Windows.DataGridViewEx();
            this.searchPagination = new FWBS.OMS.UI.UserControls.Common.ucSearchPagination();
            this.topPanelBottomLine = new System.Windows.Forms.Panel();
            this.topPanel = new System.Windows.Forms.Panel();
            this.TopFilterPanel = new System.Windows.Forms.Panel();
            this.TopFilterArea = new System.Windows.Forms.Panel();
            this.lblNoFilter = new System.Windows.Forms.Label();
            this.flpFilterLabels = new System.Windows.Forms.FlowLayoutPanel();
            this.topButtonsPanel = new System.Windows.Forms.Panel();
            this.btnClear = new FWBS.OMS.UI.Elasticsearch.ucClearButton();
            this.btnFilter = new System.Windows.Forms.CheckBox();
            this.btnPreview = new System.Windows.Forms.CheckBox();
            this.topPanelTopLine = new System.Windows.Forms.Panel();
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.rightPanelContainer = new System.Windows.Forms.Panel();
            this.columnIcon = new System.Windows.Forms.DataGridViewImageColumn();
            this.columnDescription = new FWBS.OMS.UI.Elasticsearch.DataGridViewHighlightsColumn();
            this.columnAuthor = new FWBS.OMS.UI.Elasticsearch.DataGridViewHighlightsColumn();
            this.columnModified = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnDocumentType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnSummary = new FWBS.OMS.UI.Elasticsearch.DataGridViewHighlightsColumn();
            this.columnActions = new FWBS.OMS.UI.DataGridViewControls.DataGridViewActionsColumn();
            this.columnOmsType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnFileId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnClientId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnContactId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnDocumentId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnAssociateId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnExtension = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnMatterSphereId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.filterPanel.SuspendLayout();
            this.FilterContainer.SuspendLayout();
            this.filterHeader.SuspendLayout();
            this.previewPanel.SuspendLayout();
            this.previewBackgroundPanel.SuspendLayout();
            this.previewContainer.SuspendLayout();
            this.previewHeader.SuspendLayout();
            this.gridPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataView)).BeginInit();
            this.topPanel.SuspendLayout();
            this.TopFilterPanel.SuspendLayout();
            this.TopFilterArea.SuspendLayout();
            this.topButtonsPanel.SuspendLayout();
            this.rightPanelContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // filterPanel
            // 
            this.filterPanel.AutoSize = true;
            this.filterPanel.BackColor = System.Drawing.Color.White;
            this.filterPanel.Controls.Add(this.FilterContainer);
            this.filterPanel.Controls.Add(this.filterHeader);
            this.filterPanel.Controls.Add(this.filterPanelLine);
            this.filterPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.filterPanel.Location = new System.Drawing.Point(0, 0);
            this.filterPanel.Margin = new System.Windows.Forms.Padding(0);
            this.filterPanel.Name = "filterPanel";
            this.filterPanel.Size = new System.Drawing.Size(230, 556);
            this.filterPanel.TabIndex = 1;
            // 
            // FilterContainer
            // 
            this.FilterContainer.BackColor = System.Drawing.Color.White;
            this.FilterContainer.Controls.Add(this.FacetGroupsPanel);
            this.FilterContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FilterContainer.Location = new System.Drawing.Point(1, 40);
            this.FilterContainer.Margin = new System.Windows.Forms.Padding(0);
            this.FilterContainer.Name = "FilterContainer";
            this.FilterContainer.Padding = new System.Windows.Forms.Padding(10, 3, 6, 0);
            this.FilterContainer.Size = new System.Drawing.Size(229, 516);
            this.FilterContainer.TabIndex = 1;
            // 
            // FacetGroupsPanel
            // 
            this.FacetGroupsPanel.AutoScroll = true;
            this.FacetGroupsPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.FacetGroupsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FacetGroupsPanel.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.FacetGroupsPanel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.FacetGroupsPanel.Location = new System.Drawing.Point(10, 3);
            this.FacetGroupsPanel.Margin = new System.Windows.Forms.Padding(0);
            this.FacetGroupsPanel.Name = "FacetGroupsPanel";
            this.FacetGroupsPanel.Size = new System.Drawing.Size(213, 513);
            this.FacetGroupsPanel.TabIndex = 0;
            // 
            // filterHeader
            // 
            this.filterHeader.BackColor = System.Drawing.Color.White;
            this.filterHeader.Controls.Add(this.lblFilter);
            this.filterHeader.Controls.Add(this.btnCloseFilter);
            this.filterHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.filterHeader.Location = new System.Drawing.Point(1, 0);
            this.filterHeader.Margin = new System.Windows.Forms.Padding(0);
            this.filterHeader.Name = "filterHeader";
            this.filterHeader.Size = new System.Drawing.Size(229, 40);
            this.filterHeader.TabIndex = 0;
            // 
            // lblFilter
            // 
            this.lblFilter.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblFilter.Font = new System.Drawing.Font("Segoe UI Semibold", 12.75F);
            this.lblFilter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.lblFilter.Location = new System.Drawing.Point(10, 0);
            this.resourceLookup1.SetLookup(this.lblFilter, new FWBS.OMS.UI.Windows.ResourceLookupItem("Filter", "Filter", ""));
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.lblFilter.Size = new System.Drawing.Size(95, 40);
            this.lblFilter.TabIndex = 4;
            this.lblFilter.Text = "Filter";
            this.lblFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnCloseFilter
            // 
            this.btnCloseFilter.BackColor = System.Drawing.Color.Transparent;
            this.btnCloseFilter.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnCloseFilter.FlatAppearance.BorderSize = 0;
            this.btnCloseFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseFilter.Font = new System.Drawing.Font("Segoe UI Symbol", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCloseFilter.Location = new System.Drawing.Point(189, 0);
            this.btnCloseFilter.Margin = new System.Windows.Forms.Padding(0);
            this.btnCloseFilter.Name = "btnCloseFilter";
            this.btnCloseFilter.Size = new System.Drawing.Size(40, 40);
            this.btnCloseFilter.TabIndex = 3;
            this.btnCloseFilter.Text = "X";
            this.btnCloseFilter.UseVisualStyleBackColor = false;
            this.btnCloseFilter.Click += new System.EventHandler(this.btnCloseFilter_Click);
            // 
            // filterPanelLine
            // 
            this.filterPanelLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(216)))), ((int)(((byte)(216)))));
            this.filterPanelLine.Dock = System.Windows.Forms.DockStyle.Left;
            this.filterPanelLine.Location = new System.Drawing.Point(0, 0);
            this.filterPanelLine.Margin = new System.Windows.Forms.Padding(0);
            this.filterPanelLine.Name = "filterPanelLine";
            this.filterPanelLine.Size = new System.Drawing.Size(1, 556);
            this.filterPanelLine.TabIndex = 0;
            // 
            // previewPanel
            // 
            this.previewPanel.AutoSize = true;
            this.previewPanel.BackColor = System.Drawing.Color.White;
            this.previewPanel.Controls.Add(this.previewBackgroundPanel);
            this.previewPanel.Controls.Add(this.previewHeader);
            this.previewPanel.Controls.Add(this.previewPanelLine);
            this.previewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.previewPanel.Location = new System.Drawing.Point(0, 0);
            this.previewPanel.Margin = new System.Windows.Forms.Padding(0);
            this.previewPanel.Name = "previewPanel";
            this.previewPanel.Size = new System.Drawing.Size(230, 556);
            this.previewPanel.TabIndex = 2;
            // 
            // previewBackgroundPanel
            // 
            this.previewBackgroundPanel.Controls.Add(this.previewContainer);
            this.previewBackgroundPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.previewBackgroundPanel.Location = new System.Drawing.Point(1, 40);
            this.previewBackgroundPanel.Name = "previewBackgroundPanel";
            this.previewBackgroundPanel.Padding = new System.Windows.Forms.Padding(10, 3, 10, 0);
            this.previewBackgroundPanel.Size = new System.Drawing.Size(229, 516);
            this.previewBackgroundPanel.TabIndex = 2;
            // 
            // previewContainer
            // 
            this.previewContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.previewContainer.Controls.Add(this.documentPreview);
            this.previewContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.previewContainer.Location = new System.Drawing.Point(10, 3);
            this.previewContainer.Name = "previewContainer";
            this.previewContainer.Size = new System.Drawing.Size(209, 513);
            this.previewContainer.TabIndex = 0;
            // 
            // documentPreview
            // 
            this.documentPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.documentPreview.Location = new System.Drawing.Point(0, 0);
            this.documentPreview.Name = "documentPreview";
            this.documentPreview.Padding = new System.Windows.Forms.Padding(5);
            this.documentPreview.Size = new System.Drawing.Size(209, 513);
            this.documentPreview.TabIndex = 0;
            this.documentPreview.ToBeRefreshed = false;
            // 
            // previewHeader
            // 
            this.previewHeader.BackColor = System.Drawing.Color.White;
            this.previewHeader.Controls.Add(this.lblPreview);
            this.previewHeader.Controls.Add(this.btnClosePreview);
            this.previewHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.previewHeader.Location = new System.Drawing.Point(1, 0);
            this.previewHeader.Margin = new System.Windows.Forms.Padding(0);
            this.previewHeader.Name = "previewHeader";
            this.previewHeader.Size = new System.Drawing.Size(229, 40);
            this.previewHeader.TabIndex = 1;
            // 
            // lblPreview
            // 
            this.lblPreview.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblPreview.Font = new System.Drawing.Font("Segoe UI Semibold", 12.75F);
            this.lblPreview.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.lblPreview.Location = new System.Drawing.Point(0, 0);
            this.resourceLookup1.SetLookup(this.lblPreview, new FWBS.OMS.UI.Windows.ResourceLookupItem("Preview", "Preview", ""));
            this.lblPreview.Name = "lblPreview";
            this.lblPreview.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.lblPreview.Size = new System.Drawing.Size(95, 40);
            this.lblPreview.TabIndex = 5;
            this.lblPreview.Text = "Preview";
            this.lblPreview.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnClosePreview
            // 
            this.btnClosePreview.BackColor = System.Drawing.Color.Transparent;
            this.btnClosePreview.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClosePreview.FlatAppearance.BorderSize = 0;
            this.btnClosePreview.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClosePreview.Font = new System.Drawing.Font("Segoe UI Symbol", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClosePreview.Location = new System.Drawing.Point(189, 0);
            this.btnClosePreview.Margin = new System.Windows.Forms.Padding(0);
            this.btnClosePreview.Name = "btnClosePreview";
            this.btnClosePreview.Size = new System.Drawing.Size(40, 40);
            this.btnClosePreview.TabIndex = 4;
            this.btnClosePreview.Text = "X";
            this.btnClosePreview.UseVisualStyleBackColor = false;
            this.btnClosePreview.Click += new System.EventHandler(this.btnClosePreview_Click);
            // 
            // previewPanelLine
            // 
            this.previewPanelLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(216)))), ((int)(((byte)(216)))));
            this.previewPanelLine.Dock = System.Windows.Forms.DockStyle.Left;
            this.previewPanelLine.Location = new System.Drawing.Point(0, 0);
            this.previewPanelLine.Margin = new System.Windows.Forms.Padding(0);
            this.previewPanelLine.Name = "previewPanelLine";
            this.previewPanelLine.Size = new System.Drawing.Size(1, 556);
            this.previewPanelLine.TabIndex = 0;
            // 
            // splitter
            // 
            this.splitter.BackColor = System.Drawing.Color.White;
            this.splitter.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter.Location = new System.Drawing.Point(967, 44);
            this.splitter.MinExtra = 700;
            this.splitter.Name = "splitter";
            this.splitter.Size = new System.Drawing.Size(3, 556);
            this.splitter.TabIndex = 3;
            this.splitter.TabStop = false;
            // 
            // gridPanel
            // 
            this.gridPanel.BackColor = System.Drawing.Color.White;
            this.gridPanel.Controls.Add(this.dataView);
            this.gridPanel.Controls.Add(this.searchPagination);
            this.gridPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridPanel.Location = new System.Drawing.Point(0, 44);
            this.gridPanel.Margin = new System.Windows.Forms.Padding(0);
            this.gridPanel.Name = "gridPanel";
            this.gridPanel.Size = new System.Drawing.Size(967, 556);
            this.gridPanel.TabIndex = 4;
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
            this.columnDescription,
            this.columnAuthor,
            this.columnModified,
            this.columnDocumentType,
            this.columnSummary,
            this.columnActions,
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
            this.dataView.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            this.dataView.Location = new System.Drawing.Point(0, 0);
            this.dataView.Margin = new System.Windows.Forms.Padding(0);
            this.dataView.MultiSelect = false;
            this.dataView.Name = "dataView";
            this.dataView.ReadOnly = true;
            this.dataView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dataView.RowHeadersVisible = false;
            this.dataView.RowTemplate.Height = 32;
            this.dataView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataView.Size = new System.Drawing.Size(967, 512);
            this.dataView.TabIndex = 1;
            this.dataView.SortChanged += new System.EventHandler<FWBS.OMS.UI.Windows.DataGridViewEx.SortDataEventArgs>(this.dataView_SortChanged);
            this.dataView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataView_CellDoubleClick);
            this.dataView.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataView_CellMouseEnter);
            this.dataView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataView_KeyDown);
            this.dataView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataView_MouseDown);
            this.dataView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dataView_MouseUp);
            this.dataView.SelectionChanged += new System.EventHandler(this.dataView_SelectionChanged);
            // 
            // searchPagination
            // 
            this.searchPagination.BackColor = System.Drawing.Color.White;
            this.searchPagination.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.searchPagination.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.searchPagination.Location = new System.Drawing.Point(0, 512);
            this.searchPagination.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.searchPagination.Name = "searchPagination";
            this.searchPagination.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.searchPagination.Size = new System.Drawing.Size(967, 44);
            this.searchPagination.TabIndex = 0;
            // 
            // topPanelBottomLine
            // 
            this.topPanelBottomLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(216)))), ((int)(((byte)(216)))));
            this.topPanelBottomLine.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanelBottomLine.Location = new System.Drawing.Point(0, 0);
            this.topPanelBottomLine.Margin = new System.Windows.Forms.Padding(0);
            this.topPanelBottomLine.Name = "topPanelBottomLine";
            this.topPanelBottomLine.Size = new System.Drawing.Size(1200, 1);
            this.topPanelBottomLine.TabIndex = 0;
            // 
            // topPanel
            // 
            this.topPanel.BackColor = System.Drawing.Color.White;
            this.topPanel.Controls.Add(this.TopFilterPanel);
            this.topPanel.Controls.Add(this.topButtonsPanel);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 1);
            this.topPanel.Margin = new System.Windows.Forms.Padding(0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(1200, 42);
            this.topPanel.TabIndex = 0;
            // 
            // TopFilterPanel
            // 
            this.TopFilterPanel.BackColor = System.Drawing.Color.White;
            this.TopFilterPanel.Controls.Add(this.TopFilterArea);
            this.TopFilterPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TopFilterPanel.Location = new System.Drawing.Point(0, 0);
            this.TopFilterPanel.Name = "TopFilterPanel";
            this.TopFilterPanel.Padding = new System.Windows.Forms.Padding(5, 6, 0, 4);
            this.TopFilterPanel.Size = new System.Drawing.Size(970, 42);
            this.TopFilterPanel.TabIndex = 1;
            // 
            // TopFilterArea
            // 
            this.TopFilterArea.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(243)))), ((int)(((byte)(250)))));
            this.TopFilterArea.Controls.Add(this.lblNoFilter);
            this.TopFilterArea.Controls.Add(this.flpFilterLabels);
            this.TopFilterArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TopFilterArea.Location = new System.Drawing.Point(5, 6);
            this.TopFilterArea.Name = "TopFilterArea";
            this.TopFilterArea.Padding = new System.Windows.Forms.Padding(4);
            this.TopFilterArea.Size = new System.Drawing.Size(965, 32);
            this.TopFilterArea.TabIndex = 0;
            // 
            // lblNoFilter
            // 
            this.lblNoFilter.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblNoFilter.BackColor = System.Drawing.Color.Transparent;
            this.lblNoFilter.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.lblNoFilter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.lblNoFilter.Location = new System.Drawing.Point(3, 0);
            this.resourceLookup1.SetLookup(this.lblNoFilter, new FWBS.OMS.UI.Windows.ResourceLookupItem("NoFilters", "No filters applied yet.", ""));
            this.lblNoFilter.Name = "lblNoFilter";
            this.lblNoFilter.Size = new System.Drawing.Size(144, 32);
            this.lblNoFilter.TabIndex = 1;
            this.lblNoFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // flpFilterLabels
            // 
            this.flpFilterLabels.BackColor = System.Drawing.Color.Transparent;
            this.flpFilterLabels.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpFilterLabels.Location = new System.Drawing.Point(4, 4);
            this.flpFilterLabels.Name = "flpFilterLabels";
            this.flpFilterLabels.Size = new System.Drawing.Size(957, 24);
            this.flpFilterLabels.TabIndex = 0;
            this.flpFilterLabels.WrapContents = false;
            // 
            // topButtonsPanel
            // 
            this.topButtonsPanel.BackColor = System.Drawing.Color.White;
            this.topButtonsPanel.Controls.Add(this.btnClear);
            this.topButtonsPanel.Controls.Add(this.btnFilter);
            this.topButtonsPanel.Controls.Add(this.btnPreview);
            this.topButtonsPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.topButtonsPanel.Location = new System.Drawing.Point(970, 0);
            this.topButtonsPanel.Margin = new System.Windows.Forms.Padding(0);
            this.topButtonsPanel.Name = "topButtonsPanel";
            this.topButtonsPanel.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.topButtonsPanel.Size = new System.Drawing.Size(230, 42);
            this.topButtonsPanel.TabIndex = 0;
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.Transparent;
            this.btnClear.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F);
            this.btnClear.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnClear.Location = new System.Drawing.Point(10, 5);
            this.btnClear.Margin = new System.Windows.Forms.Padding(0);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(80, 32);
            this.btnClear.TabIndex = 0;
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnFilter
            // 
            this.btnFilter.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnFilter.BackColor = System.Drawing.Color.Transparent;
            this.btnFilter.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnFilter.FlatAppearance.BorderSize = 0;
            this.btnFilter.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(224)))), ((int)(((byte)(242)))));
            this.btnFilter.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(224)))), ((int)(((byte)(242)))));
            this.btnFilter.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(224)))), ((int)(((byte)(242)))));
            this.btnFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFilter.Location = new System.Drawing.Point(156, 5);
            this.btnFilter.Margin = new System.Windows.Forms.Padding(0);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(32, 32);
            this.btnFilter.TabIndex = 1;
            this.btnFilter.UseVisualStyleBackColor = false;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // btnPreview
            // 
            this.btnPreview.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnPreview.BackColor = System.Drawing.Color.Transparent;
            this.btnPreview.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnPreview.FlatAppearance.BorderSize = 0;
            this.btnPreview.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(224)))), ((int)(((byte)(242)))));
            this.btnPreview.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(224)))), ((int)(((byte)(242)))));
            this.btnPreview.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(224)))), ((int)(((byte)(242)))));
            this.btnPreview.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPreview.Location = new System.Drawing.Point(188, 5);
            this.btnPreview.Margin = new System.Windows.Forms.Padding(0);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(32, 32);
            this.btnPreview.TabIndex = 2;
            this.btnPreview.UseVisualStyleBackColor = false;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // topPanelTopLine
            // 
            this.topPanelTopLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(216)))), ((int)(((byte)(216)))));
            this.topPanelTopLine.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanelTopLine.Location = new System.Drawing.Point(0, 43);
            this.topPanelTopLine.Margin = new System.Windows.Forms.Padding(0);
            this.topPanelTopLine.Name = "topPanelTopLine";
            this.topPanelTopLine.Size = new System.Drawing.Size(1200, 1);
            this.topPanelTopLine.TabIndex = 0;
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
            // columnDescription
            // 
            this.columnDescription.HeaderText = "Description";
            this.columnDescription.MinimumWidth = 50;
            this.columnDescription.Name = "columnDescription";
            this.columnDescription.ReadOnly = true;
            this.columnDescription.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.columnDescription.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.columnDescription.Width = 300;
            // 
            // columnAuthor
            // 
            this.columnAuthor.HeaderText = "Author";
            this.columnAuthor.MinimumWidth = 50;
            this.columnAuthor.Name = "columnAuthor";
            this.columnAuthor.ReadOnly = true;
            this.columnAuthor.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.columnAuthor.Width = 150;
            // 
            // columnModified
            // 
            this.columnModified.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.columnModified.HeaderText = "Modified";
            this.columnModified.MinimumWidth = 50;
            this.columnModified.Name = "columnModified";
            this.columnModified.ReadOnly = true;
            this.columnModified.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.columnModified.Width = 130;
            // 
            // columnDocumentType
            // 
            this.columnDocumentType.HeaderText = "Document Type";
            this.columnDocumentType.MinimumWidth = 50;
            this.columnDocumentType.Name = "columnDocumentType";
            this.columnDocumentType.ReadOnly = true;
            this.columnDocumentType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.columnDocumentType.Width = 150;
            // 
            // columnSummary
            // 
            this.columnSummary.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnSummary.HeaderText = "Summary";
            this.columnSummary.MinimumWidth = 150;
            this.columnSummary.Name = "columnSummary";
            this.columnSummary.ReadOnly = true;
            this.columnSummary.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // columnActions
            // 
            this.columnActions.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.columnActions.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.columnActions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.columnActions.GetActions = null;
            this.columnActions.HeaderText = "Actions";
            this.columnActions.MinimumWidth = 100;
            this.columnActions.Name = "columnActions";
            this.columnActions.ReadOnly = true;
            this.columnActions.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.columnActions.Text = "...";
            this.columnActions.UseColumnTextForButtonValue = true;
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
            // rightPanelContainer
            // 
            this.rightPanelContainer.Controls.Add(this.previewPanel);
            this.rightPanelContainer.Controls.Add(this.filterPanel);
            this.rightPanelContainer.Dock = System.Windows.Forms.DockStyle.Right;
            this.rightPanelContainer.Location = new System.Drawing.Point(970, 44);
            this.rightPanelContainer.Name = "rightPanelContainer";
            this.rightPanelContainer.Size = new System.Drawing.Size(230, 556);
            this.rightPanelContainer.TabIndex = 1;
            // 
            // ElasticsearchControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.gridPanel);
            this.Controls.Add(this.splitter);
            this.Controls.Add(this.rightPanelContainer);
            this.Controls.Add(this.topPanelTopLine);
            this.Controls.Add(this.topPanel);
            this.Controls.Add(this.topPanelBottomLine);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ElasticsearchControl";
            this.Size = new System.Drawing.Size(1200, 600);
            this.DpiChangedAfterParent += new System.EventHandler(this.ElasticsearchControl_DpiChangedAfterParent);
            this.filterPanel.ResumeLayout(false);
            this.FilterContainer.ResumeLayout(false);
            this.filterHeader.ResumeLayout(false);
            this.previewPanel.ResumeLayout(false);
            this.previewBackgroundPanel.ResumeLayout(false);
            this.previewContainer.ResumeLayout(false);
            this.previewHeader.ResumeLayout(false);
            this.gridPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataView)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.TopFilterPanel.ResumeLayout(false);
            this.TopFilterArea.ResumeLayout(false);
            this.topButtonsPanel.ResumeLayout(false);
            this.rightPanelContainer.ResumeLayout(false);
            this.rightPanelContainer.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel filterPanel;
        private System.Windows.Forms.Panel filterPanelLine;
        private System.Windows.Forms.Panel previewPanel;
        private System.Windows.Forms.Panel previewPanelLine;
        private System.Windows.Forms.Splitter splitter;
        private System.Windows.Forms.Panel gridPanel;
        private System.Windows.Forms.Panel topPanelBottomLine;
        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.Panel topPanelTopLine;
        private FWBS.OMS.UI.Elasticsearch.ucClearButton btnClear;
        private System.Windows.Forms.CheckBox btnFilter;
        private System.Windows.Forms.CheckBox btnPreview;
        private System.Windows.Forms.Panel filterHeader;
        private System.Windows.Forms.Panel topButtonsPanel;
        private System.Windows.Forms.Button btnCloseFilter;
        private System.Windows.Forms.Panel previewHeader;
        private System.Windows.Forms.Button btnClosePreview;
        private System.Windows.Forms.Label lblPreview;
        private FWBS.OMS.UI.Windows.DataGridViewEx dataView;
        private FWBS.OMS.UI.UserControls.Common.ucSearchPagination searchPagination;
        private System.Windows.Forms.Label lblFilter;
        private System.Windows.Forms.Panel FilterContainer;
        private System.Windows.Forms.Panel FacetGroupsPanel;
        private System.Windows.Forms.Panel TopFilterPanel;
        private System.Windows.Forms.Panel TopFilterArea;
        private System.Windows.Forms.FlowLayoutPanel flpFilterLabels;
        private System.Windows.Forms.Panel previewBackgroundPanel;
        private System.Windows.Forms.Panel previewContainer;
        private Windows.DocumentManagement.Addins.DocumentPreviewAddin documentPreview;
        private System.Windows.Forms.Label lblNoFilter;
        private Windows.ResourceLookup resourceLookup1;
        private System.Windows.Forms.DataGridViewImageColumn columnIcon;
        private DataGridViewHighlightsColumn columnDescription;
        private DataGridViewHighlightsColumn columnAuthor;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnModified;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnDocumentType;
        private DataGridViewHighlightsColumn columnSummary;
        private DataGridViewControls.DataGridViewActionsColumn columnActions;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnOmsType;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnFileId;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnClientId;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnContactId;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnDocumentId;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnAssociateId;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnExtension;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnMatterSphereId;
        private System.Windows.Forms.Panel rightPanelContainer;
    }
}
