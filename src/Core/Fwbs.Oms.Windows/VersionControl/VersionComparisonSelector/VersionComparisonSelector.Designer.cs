namespace FWBS.OMS.UI.Windows
{
    partial class VersionComparisonSelector
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
            this.tvLinkedObjects = new Telerik.WinControls.UI.RadTreeView();
            this.windows8Theme1 = new Telerik.WinControls.Themes.Windows8Theme();
            this.objectInformation = new FWBS.Common.UI.Windows.eInformation2();
            this.pnlTree = new System.Windows.Forms.Panel();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnRestore = new System.Windows.Forms.Button();
            this.btnCompare = new System.Windows.Forms.Button();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.scSearch = new FWBS.OMS.UI.Windows.ucSearchControl();
            ((System.ComponentModel.ISupportInitialize)(this.tvLinkedObjects)).BeginInit();
            this.pnlTree.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // tvLinkedObjects
            // 
            this.tvLinkedObjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvLinkedObjects.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tvLinkedObjects.LineColor = System.Drawing.Color.WhiteSmoke;
            this.tvLinkedObjects.Location = new System.Drawing.Point(8, 8);
            this.tvLinkedObjects.Name = "tvLinkedObjects";
            this.tvLinkedObjects.Size = new System.Drawing.Size(244, 377);
            this.tvLinkedObjects.SpacingBetweenNodes = -1;
            this.tvLinkedObjects.TabIndex = 0;
            this.tvLinkedObjects.ThemeName = "Windows8";
            this.tvLinkedObjects.ToggleMode = Telerik.WinControls.UI.ToggleMode.SingleClick;
            // 
            // objectInformation
            // 
            this.objectInformation.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(222)))), ((int)(((byte)(214)))));
            this.objectInformation.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.objectInformation.Location = new System.Drawing.Point(8, 385);
            this.objectInformation.Name = "objectInformation";
            this.objectInformation.Padding = new System.Windows.Forms.Padding(1);
            this.objectInformation.Size = new System.Drawing.Size(244, 128);
            this.objectInformation.TabIndex = 3;
            this.objectInformation.Text = "One Line Information Line";
            // 
            // pnlTree
            // 
            this.pnlTree.BackColor = System.Drawing.Color.White;
            this.pnlTree.Controls.Add(this.tvLinkedObjects);
            this.pnlTree.Controls.Add(this.objectInformation);
            this.pnlTree.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlTree.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlTree.Location = new System.Drawing.Point(0, 40);
            this.pnlTree.Name = "pnlTree";
            this.pnlTree.Padding = new System.Windows.Forms.Padding(8);
            this.pnlTree.Size = new System.Drawing.Size(260, 521);
            this.pnlTree.TabIndex = 3;
            // 
            // pnlButtons
            // 
            this.pnlButtons.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlButtons.Controls.Add(this.btnRestore);
            this.pnlButtons.Controls.Add(this.btnCompare);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlButtons.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlButtons.Location = new System.Drawing.Point(0, 0);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(984, 40);
            this.pnlButtons.TabIndex = 2;
            // 
            // btnRestore
            // 
            this.btnRestore.Location = new System.Drawing.Point(132, 8);
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.Size = new System.Drawing.Size(120, 24);
            this.btnRestore.TabIndex = 1;
            this.btnRestore.Text = "Restore";
            this.btnRestore.UseVisualStyleBackColor = true;
            this.btnRestore.Click += new System.EventHandler(this.btnRestore_Click);
            // 
            // btnCompare
            // 
            this.btnCompare.Location = new System.Drawing.Point(8, 8);
            this.btnCompare.Name = "btnCompare";
            this.btnCompare.Size = new System.Drawing.Size(120, 24);
            this.btnCompare.TabIndex = 0;
            this.btnCompare.Text = "Compare";
            this.btnCompare.UseVisualStyleBackColor = true;
            this.btnCompare.Click += new System.EventHandler(this.btnCompare_Click);
            // 
            // pnlSearch
            // 
            this.pnlSearch.BackColor = System.Drawing.Color.White;
            this.pnlSearch.Controls.Add(this.scSearch);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSearch.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlSearch.Location = new System.Drawing.Point(260, 40);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(724, 521);
            this.pnlSearch.TabIndex = 4;
            // 
            // scSearch
            // 
            this.scSearch.BackColor = System.Drawing.Color.White;
            this.scSearch.BackGroundColor = System.Drawing.Color.White;
            this.scSearch.ButtonPanelVisible = true;
            this.scSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scSearch.DoubleClickAction = "None";
            this.scSearch.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.scSearch.GraphicalPanelVisible = true;
            this.scSearch.Location = new System.Drawing.Point(0, 0);
            this.scSearch.Margin = new System.Windows.Forms.Padding(0);
            this.scSearch.MultiSelect = FWBS.Common.TriState.True;
            this.scSearch.Name = "scSearch";
            this.scSearch.NavCommandPanel = null;
            this.scSearch.Padding = new System.Windows.Forms.Padding(5);
            this.scSearch.RefreshOnEnquiryFormRefreshEvent = false;
            this.scSearch.SaveSearch = FWBS.OMS.SearchEngine.SaveSearchType.Never;
            this.scSearch.SearchListCode = "";
            this.scSearch.SearchListType = "";
            this.scSearch.Size = new System.Drawing.Size(724, 521);
            this.scSearch.TabIndex = 1;
            this.scSearch.ToBeRefreshed = false;
            this.scSearch.ItemHover += new FWBS.OMS.UI.Windows.SearchItemHoverEventHandler(this.scSearch_ItemHover);
            // 
            // VersionComparisonSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(984, 561);
            this.Controls.Add(this.pnlSearch);
            this.Controls.Add(this.pnlTree);
            this.Controls.Add(this.pnlButtons);
            this.Name = "VersionComparisonSelector";
            this.Text = "Version Administration";
            ((System.ComponentModel.ISupportInitialize)(this.tvLinkedObjects)).EndInit();
            this.pnlTree.ResumeLayout(false);
            this.pnlButtons.ResumeLayout(false);
            this.pnlSearch.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadTreeView tvLinkedObjects;
        private ucSearchControl scSearch;
        private Telerik.WinControls.Themes.Windows8Theme windows8Theme1;
        private Common.UI.Windows.eInformation2 objectInformation;
        private System.Windows.Forms.Panel pnlTree;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnRestore;
        private System.Windows.Forms.Button btnCompare;
        private System.Windows.Forms.Panel pnlSearch;
    }
}