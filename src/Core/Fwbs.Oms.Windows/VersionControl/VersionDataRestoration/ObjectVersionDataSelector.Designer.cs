namespace FWBS.OMS.UI.Windows
{
    partial class ObjectVersionDataSelector
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
            this.chkRevertAll = new System.Windows.Forms.CheckBox();
            this.btnRestore = new System.Windows.Forms.Button();
            this.eLabel1 = new FWBS.Common.UI.Windows.eLabel2();
            this.eLabel2 = new FWBS.Common.UI.Windows.eLabel2();
            this.sObjectSet = new FWBS.OMS.UI.Windows.ucSearchControl();
            this.sHeader = new FWBS.OMS.UI.Windows.ucSearchControl();
            this.NoticeBoard = new FWBS.Common.UI.Windows.eMultiTextBox2();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.pnlButtons.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkRevertAll
            // 
            this.chkRevertAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkRevertAll.AutoSize = true;
            this.chkRevertAll.Location = new System.Drawing.Point(303, 15);
            this.chkRevertAll.Name = "chkRevertAll";
            this.chkRevertAll.Size = new System.Drawing.Size(328, 19);
            this.chkRevertAll.TabIndex = 1;
            this.chkRevertAll.Text = "Restore all Objects in Check-in Set as production versions";
            this.chkRevertAll.UseVisualStyleBackColor = true;
            // 
            // btnRestore
            // 
            this.btnRestore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRestore.Location = new System.Drawing.Point(634, 12);
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.Size = new System.Drawing.Size(139, 24);
            this.btnRestore.TabIndex = 0;
            this.btnRestore.Text = "Restore";
            this.btnRestore.UseVisualStyleBackColor = true;
            this.btnRestore.Click += new System.EventHandler(this.btnRestore_Click);
            // 
            // eLabel1
            // 
            this.eLabel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.eLabel1.CaptionWidth = 500;
            this.eLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.eLabel1.Format = "";
            this.eLabel1.IsDirty = false;
            this.eLabel1.Location = new System.Drawing.Point(0, 0);
            this.eLabel1.Margin = new System.Windows.Forms.Padding(0);
            this.eLabel1.Name = "eLabel1";
            this.eLabel1.ReadOnly = true;
            this.eLabel1.Size = new System.Drawing.Size(784, 30);
            this.eLabel1.TabIndex = 0;
            this.eLabel1.Text = "Check-in Sets";
            this.eLabel1.Value = null;
            // 
            // eLabel2
            // 
            this.eLabel2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.eLabel2.CaptionWidth = 300;
            this.eLabel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.eLabel2.Format = "";
            this.eLabel2.IsDirty = false;
            this.eLabel2.Location = new System.Drawing.Point(0, 241);
            this.eLabel2.Margin = new System.Windows.Forms.Padding(0);
            this.eLabel2.Name = "eLabel2";
            this.eLabel2.ReadOnly = true;
            this.eLabel2.Size = new System.Drawing.Size(784, 30);
            this.eLabel2.TabIndex = 1;
            this.eLabel2.Text = "Objects in the Selected Checked-in set";
            this.eLabel2.Value = null;
            // 
            // sObjectSet
            // 
            this.sObjectSet.BackColor = System.Drawing.Color.White;
            this.sObjectSet.BackGroundColor = System.Drawing.Color.White;
            this.sObjectSet.ButtonPanelVisible = true;
            this.sObjectSet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sObjectSet.DoubleClickAction = "None";
            this.sObjectSet.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.sObjectSet.GraphicalPanelVisible = true;
            this.sObjectSet.Location = new System.Drawing.Point(0, 271);
            this.sObjectSet.Name = "sObjectSet";
            this.sObjectSet.NavCommandPanel = null;
            this.sObjectSet.Padding = new System.Windows.Forms.Padding(5);
            this.sObjectSet.RefreshOnEnquiryFormRefreshEvent = false;
            this.sObjectSet.SaveSearch = FWBS.OMS.SearchEngine.SaveSearchType.Never;
            this.sObjectSet.SearchListCode = "";
            this.sObjectSet.SearchListType = "";
            this.sObjectSet.Size = new System.Drawing.Size(784, 141);
            this.sObjectSet.TabIndex = 5;
            this.sObjectSet.ToBeRefreshed = false;
            // 
            // sHeader
            // 
            this.sHeader.BackColor = System.Drawing.Color.White;
            this.sHeader.BackGroundColor = System.Drawing.Color.White;
            this.sHeader.ButtonPanelVisible = true;
            this.sHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sHeader.DoubleClickAction = "None";
            this.sHeader.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.sHeader.GraphicalPanelVisible = true;
            this.sHeader.Location = new System.Drawing.Point(0, 30);
            this.sHeader.Margin = new System.Windows.Forms.Padding(0);
            this.sHeader.Name = "sHeader";
            this.sHeader.NavCommandPanel = null;
            this.sHeader.Padding = new System.Windows.Forms.Padding(5);
            this.sHeader.RefreshOnEnquiryFormRefreshEvent = false;
            this.sHeader.SaveSearch = FWBS.OMS.SearchEngine.SaveSearchType.Never;
            this.sHeader.SearchListCode = "";
            this.sHeader.SearchListType = "";
            this.sHeader.Size = new System.Drawing.Size(784, 211);
            this.sHeader.TabIndex = 4;
            this.sHeader.ToBeRefreshed = false;
            this.sHeader.ItemHovered += new System.EventHandler(this.sHeader_ItemHovered);
            // 
            // NoticeBoard
            // 
            this.NoticeBoard.BackColor = System.Drawing.Color.WhiteSmoke;
            this.NoticeBoard.CaptionWidth = 0;
            this.NoticeBoard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NoticeBoard.ForeColor = System.Drawing.Color.Red;
            this.NoticeBoard.IsDirty = false;
            this.NoticeBoard.Location = new System.Drawing.Point(3, 414);
            this.NoticeBoard.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.NoticeBoard.MaxLength = 0;
            this.NoticeBoard.Name = "NoticeBoard";
            this.NoticeBoard.ReadOnly = true;
            this.NoticeBoard.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.NoticeBoard.Size = new System.Drawing.Size(778, 96);
            this.NoticeBoard.TabIndex = 0;
            this.NoticeBoard.Text = " ";
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.chkRevertAll);
            this.pnlButtons.Controls.Add(this.btnRestore);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlButtons.Location = new System.Drawing.Point(0, 512);
            this.pnlButtons.Margin = new System.Windows.Forms.Padding(0);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(784, 49);
            this.pnlButtons.TabIndex = 6;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.pnlButtons, 0, 5);
            this.tableLayoutPanel.Controls.Add(this.NoticeBoard, 0, 4);
            this.tableLayoutPanel.Controls.Add(this.sObjectSet, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.eLabel2, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.sHeader, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.eLabel1, 0, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 6;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(784, 561);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // ObjectVersionDataSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.tableLayoutPanel);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "ObjectVersionDataSelector";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Object Version Data Selector";
            this.Load += new System.EventHandler(this.ObjectVersionDataSelector_Load);
            this.pnlButtons.ResumeLayout(false);
            this.pnlButtons.PerformLayout();
            this.tableLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnRestore;
        private Common.UI.Windows.eLabel2 eLabel1;
        private Windows.ucSearchControl sHeader;
        private Common.UI.Windows.eLabel2 eLabel2;
        private ucSearchControl sObjectSet;
        private System.Windows.Forms.CheckBox chkRevertAll;
        private Common.UI.Windows.eMultiTextBox2 NoticeBoard;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
    }
}