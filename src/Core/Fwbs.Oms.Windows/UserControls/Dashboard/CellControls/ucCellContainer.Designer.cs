namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls
{
    partial class ucCellContainer
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
            this.topPanel = new System.Windows.Forms.Panel();
            this.topPanelTable = new System.Windows.Forms.TableLayoutPanel();
            this.btnFilter = new System.Windows.Forms.Button();
            this.btnFullScreen = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnActions = new System.Windows.Forms.Button();
            this.titlePanel = new System.Windows.Forms.Panel();
            this.topLine = new System.Windows.Forms.Panel();
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.pnlPageControlContainer = new System.Windows.Forms.Panel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.bottomLine = new System.Windows.Forms.Panel();
            this.container = new System.Windows.Forms.Panel();
            this.mainContainer = new System.Windows.Forms.Panel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.searchPanel = new FWBS.OMS.UI.UserControls.Dashboard.ucDashboardItemSearch();
            this.pageControl = new FWBS.OMS.UI.UserControls.Dashboard.CellControls.ucPageControl();
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.topPanel.SuspendLayout();
            this.topPanelTable.SuspendLayout();
            this.bottomPanel.SuspendLayout();
            this.pnlPageControlContainer.SuspendLayout();
            this.container.SuspendLayout();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.Controls.Add(this.topPanelTable);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Margin = new System.Windows.Forms.Padding(0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(459, 40);
            this.topPanel.TabIndex = 0;
            // 
            // topPanelTable
            // 
            this.topPanelTable.ColumnCount = 5;
            this.topPanelTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.topPanelTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.topPanelTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.topPanelTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.topPanelTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.topPanelTable.Controls.Add(this.btnFilter, 1, 0);
            this.topPanelTable.Controls.Add(this.btnFullScreen, 2, 0);
            this.topPanelTable.Controls.Add(this.btnSearch, 3, 0);
            this.topPanelTable.Controls.Add(this.btnActions, 4, 0);
            this.topPanelTable.Controls.Add(this.titlePanel, 0, 0);
            this.topPanelTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.topPanelTable.Location = new System.Drawing.Point(0, 0);
            this.topPanelTable.Margin = new System.Windows.Forms.Padding(0);
            this.topPanelTable.Name = "topPanelTable";
            this.topPanelTable.RowCount = 1;
            this.topPanelTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.topPanelTable.Size = new System.Drawing.Size(459, 40);
            this.topPanelTable.TabIndex = 0;
            // 
            // btnFilter
            // 
            this.btnFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFilter.FlatAppearance.BorderSize = 0;
            this.btnFilter.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.btnFilter.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.btnFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFilter.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.btnFilter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.btnFilter.Location = new System.Drawing.Point(311, 5);
            this.btnFilter.Margin = new System.Windows.Forms.Padding(0, 5, 5, 3);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(32, 32);
            this.btnFilter.TabIndex = 4;
            this.btnFilter.UseVisualStyleBackColor = true;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // btnFullScreen
            // 
            this.btnFullScreen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFullScreen.FlatAppearance.BorderSize = 0;
            this.btnFullScreen.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.btnFullScreen.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.btnFullScreen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFullScreen.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.btnFullScreen.Location = new System.Drawing.Point(348, 5);
            this.btnFullScreen.Margin = new System.Windows.Forms.Padding(0, 5, 5, 3);
            this.btnFullScreen.Name = "btnFullScreen";
            this.btnFullScreen.Size = new System.Drawing.Size(32, 32);
            this.btnFullScreen.TabIndex = 3;
            this.btnFullScreen.UseVisualStyleBackColor = true;
            this.btnFullScreen.Click += new System.EventHandler(this.btnFullScreen_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.btnSearch.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.btnSearch.Location = new System.Drawing.Point(385, 5);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(0, 5, 5, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(32, 32);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnActions
            // 
            this.btnActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnActions.FlatAppearance.BorderSize = 0;
            this.btnActions.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.btnActions.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.btnActions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnActions.Font = new System.Drawing.Font("Segoe UI Symbol", 10.5F);
            this.btnActions.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.btnActions.Location = new System.Drawing.Point(422, 5);
            this.btnActions.Margin = new System.Windows.Forms.Padding(0, 5, 5, 3);
            this.btnActions.Name = "btnActions";
            this.btnActions.Size = new System.Drawing.Size(32, 32);
            this.btnActions.TabIndex = 0;
            this.btnActions.UseVisualStyleBackColor = true;
            this.btnActions.Click += new System.EventHandler(this.btnActions_Click);
            // 
            // titlePanel
            // 
            this.titlePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.titlePanel.Location = new System.Drawing.Point(0, 0);
            this.titlePanel.Margin = new System.Windows.Forms.Padding(0);
            this.titlePanel.Name = "titlePanel";
            this.titlePanel.Size = new System.Drawing.Size(311, 40);
            this.titlePanel.TabIndex = 2;
            // 
            // topLine
            // 
            this.topLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            this.topLine.Dock = System.Windows.Forms.DockStyle.Top;
            this.topLine.Location = new System.Drawing.Point(0, 40);
            this.topLine.Margin = new System.Windows.Forms.Padding(0);
            this.topLine.Name = "topLine";
            this.topLine.Size = new System.Drawing.Size(459, 1);
            this.topLine.TabIndex = 1;
            // 
            // bottomPanel
            // 
            this.bottomPanel.Controls.Add(this.pnlPageControlContainer);
            this.bottomPanel.Controls.Add(this.btnAdd);
            this.bottomPanel.Controls.Add(this.btnDelete);
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomPanel.Location = new System.Drawing.Point(0, 290);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(459, 48);
            this.bottomPanel.TabIndex = 2;
            // 
            // pnlPageControlContainer
            // 
            this.pnlPageControlContainer.Controls.Add(this.pageControl);
            this.pnlPageControlContainer.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlPageControlContainer.Location = new System.Drawing.Point(0, 0);
            this.pnlPageControlContainer.Name = "pnlPageControlContainer";
            this.pnlPageControlContainer.Padding = new System.Windows.Forms.Padding(15, 9, 0, 9);
            this.pnlPageControlContainer.Size = new System.Drawing.Size(210, 48);
            this.pnlPageControlContainer.TabIndex = 4;
            // 
            // btnAdd
            // 
            this.btnAdd.AutoSize = true;
            this.btnAdd.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.btnAdd.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Location = new System.Drawing.Point(343, 0);
            this.resourceLookup1.SetLookup(this.btnAdd, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnAdd", "Add New", ""));
            this.btnAdd.Margin = new System.Windows.Forms.Padding(0, 5, 5, 3);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(66, 48);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.Text = "Add New";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.AutoSize = true;
            this.btnDelete.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnDelete.Enabled = false;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.btnDelete.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Location = new System.Drawing.Point(409, 0);
            this.resourceLookup1.SetLookup(this.btnDelete, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnDelete", "Delete", ""));
            this.btnDelete.Margin = new System.Windows.Forms.Padding(0, 5, 5, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(50, 48);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // bottomLine
            // 
            this.bottomLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            this.bottomLine.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomLine.Location = new System.Drawing.Point(0, 289);
            this.bottomLine.Name = "bottomLine";
            this.bottomLine.Size = new System.Drawing.Size(459, 1);
            this.bottomLine.TabIndex = 3;
            // 
            // container
            // 
            this.container.BackColor = System.Drawing.Color.White;
            this.container.Controls.Add(this.mainContainer);
            this.container.Controls.Add(this.searchPanel);
            this.container.Controls.Add(this.topLine);
            this.container.Controls.Add(this.topPanel);
            this.container.Controls.Add(this.bottomLine);
            this.container.Controls.Add(this.bottomPanel);
            this.container.Dock = System.Windows.Forms.DockStyle.Fill;
            this.container.Location = new System.Drawing.Point(4, 4);
            this.container.Name = "container";
            this.container.Size = new System.Drawing.Size(459, 338);
            this.container.TabIndex = 4;
            // 
            // mainContainer
            // 
            this.mainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainContainer.Location = new System.Drawing.Point(0, 83);
            this.mainContainer.Name = "mainContainer";
            this.mainContainer.Size = new System.Drawing.Size(459, 206);
            this.mainContainer.TabIndex = 5;
            // 
            // searchPanel
            // 
            this.searchPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.searchPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.searchPanel.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.searchPanel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.searchPanel.Location = new System.Drawing.Point(0, 41);
            this.searchPanel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.searchPanel.Name = "searchPanel";
            this.searchPanel.Size = new System.Drawing.Size(459, 42);
            this.searchPanel.TabIndex = 4;
            this.searchPanel.Visible = false;
            // 
            // pageControl
            // 
            this.pageControl.BackColor = System.Drawing.Color.White;
            this.pageControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pageControl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pageControl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.pageControl.Location = new System.Drawing.Point(15, 9);
            this.pageControl.Margin = new System.Windows.Forms.Padding(0);
            this.pageControl.Name = "pageControl";
            this.pageControl.PageSize = 50D;
            this.pageControl.Size = new System.Drawing.Size(195, 30);
            this.pageControl.TabIndex = 0;
            // 
            // ucCellContainer
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.container);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ucCellContainer";
            this.Padding = new System.Windows.Forms.Padding(4);
            this.Size = new System.Drawing.Size(467, 346);
            this.DpiChangedAfterParent += new System.EventHandler(this.ucCellContainer_DpiChangedAfterParent);
            this.topPanel.ResumeLayout(false);
            this.topPanelTable.ResumeLayout(false);
            this.bottomPanel.ResumeLayout(false);
            this.bottomPanel.PerformLayout();
            this.pnlPageControlContainer.ResumeLayout(false);
            this.container.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.Panel topLine;
        private System.Windows.Forms.Panel bottomPanel;
        private System.Windows.Forms.Panel bottomLine;
        private System.Windows.Forms.TableLayoutPanel topPanelTable;
        private System.Windows.Forms.Button btnActions;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Panel container;
        private ucDashboardItemSearch searchPanel;
        private System.Windows.Forms.Panel titlePanel;
        private System.Windows.Forms.Button btnFullScreen;
        private System.Windows.Forms.ToolTip toolTip;
        private Windows.ResourceLookup resourceLookup1;
        private System.Windows.Forms.Panel mainContainer;
        private System.Windows.Forms.Panel pnlPageControlContainer;
        private ucPageControl pageControl;
        private System.Windows.Forms.Button btnFilter;
    }
}
