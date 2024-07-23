namespace FWBS.OMS.UI.Elasticsearch.GlobalSearch
{
    class ucEmptyResult : System.Windows.Forms.UserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public ucEmptyResult()
        {
            InitializeComponent();
            this.pictureBox.Image = Windows.Images.GetNewEntityImage("no-results-found");
        }

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
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.lblNotResults = new System.Windows.Forms.Label();
            this.lblSuggestion = new System.Windows.Forms.Label();
            this.ucNewEntityPanel1 = new FWBS.OMS.UI.Elasticsearch.GlobalSearch.ucNewEntityPanel();
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel.ColumnCount = 3;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Controls.Add(this.pictureBox, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.lblNotResults, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.ucNewEntityPanel1, 0, 7);
            this.tableLayoutPanel.Controls.Add(this.lblSuggestion, 1, 5);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tableLayoutPanel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 8;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 63F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(550, 700);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // pictureBox
            // 
            this.pictureBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanel.SetColumnSpan(this.pictureBox, 3);
            this.pictureBox.Location = new System.Drawing.Point(101, 260);
            this.pictureBox.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(348, 128);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // lblNotResults
            // 
            this.lblNotResults.AutoSize = true;
            this.lblNotResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNotResults.Font = new System.Drawing.Font("Segoe UI", 24F);
            this.lblNotResults.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(133)))), ((int)(((byte)(116)))));
            this.lblNotResults.Location = new System.Drawing.Point(178, 68);
            this.resourceLookup1.SetLookup(this.lblNotResults, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblNoResults", "No Results Found", ""));
            this.lblNotResults.Name = "lblNotResults";
            this.lblNotResults.Size = new System.Drawing.Size(194, 90);
            this.lblNotResults.TabIndex = 2;
            this.lblNotResults.Text = "No Results Found";
            this.lblNotResults.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSuggestion
            // 
            this.lblSuggestion.AutoSize = true;
            this.lblSuggestion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSuggestion.Location = new System.Drawing.Point(178, 434);
            this.resourceLookup1.SetLookup(this.lblSuggestion, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblSuggestion", "Please try your search again.", ""));
            this.lblSuggestion.Name = "lblSuggestion";
            this.lblSuggestion.Size = new System.Drawing.Size(194, 30);
            this.lblSuggestion.TabIndex = 3;
            this.lblSuggestion.Text = "Please try your search again, or start New from below";
            this.lblSuggestion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ucNewEntityPanel1
            // 
            this.ucNewEntityPanel1.AutoSize = true;
            this.ucNewEntityPanel1.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel.SetColumnSpan(this.ucNewEntityPanel1, 3);
            this.ucNewEntityPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucNewEntityPanel1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ucNewEntityPanel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.ucNewEntityPanel1.Location = new System.Drawing.Point(0, 635);
            this.ucNewEntityPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.ucNewEntityPanel1.Name = "ucNewEntityPanel1";
            this.ucNewEntityPanel1.Size = new System.Drawing.Size(550, 65);
            this.ucNewEntityPanel1.TabIndex = 4;
            // 
            // ucEmptyResult
            // 
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "ucEmptyResult";
            this.Size = new System.Drawing.Size(550, 700);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Label lblNotResults;
        private System.Windows.Forms.Label lblSuggestion;
        private ucNewEntityPanel ucNewEntityPanel1;
        private Windows.ResourceLookup resourceLookup1;
    }
}
