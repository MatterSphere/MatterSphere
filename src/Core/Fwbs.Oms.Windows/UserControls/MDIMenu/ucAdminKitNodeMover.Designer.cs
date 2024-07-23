namespace FWBS.OMS.UI.Windows.Admin
{
    partial class ucAdminKitNodeMover
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
            this.SectionTreeView = new Telerik.WinControls.UI.RadTreeView();
            this.SectionList = new Telerik.WinControls.UI.RadTreeView();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnMoveMode = new System.Windows.Forms.Button();
            this.windows8Theme1 = new Telerik.WinControls.Themes.Windows8Theme();
            this.windows8Theme2 = new Telerik.WinControls.Themes.Windows8Theme();
            this.chkSection = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.pnlButtons = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.SectionTreeView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SectionList)).BeginInit();
            this.tableLayoutPanel.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // SectionTreeView
            // 
            this.SectionTreeView.BackColor = System.Drawing.Color.White;
            this.SectionTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SectionTreeView.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.SectionTreeView.Location = new System.Drawing.Point(284, 3);
            this.SectionTreeView.Name = "SectionTreeView";
            // 
            // 
            // 
            this.SectionTreeView.RootElement.ControlBounds = new System.Drawing.Rectangle(0, 0, 150, 250);
            this.SectionTreeView.Size = new System.Drawing.Size(417, 488);
            this.SectionTreeView.TabIndex = 3;
            this.SectionTreeView.ThemeName = "Windows8";
            // 
            // SectionList
            // 
            this.SectionList.BackColor = System.Drawing.Color.White;
            this.SectionList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SectionList.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.SectionList.Location = new System.Drawing.Point(3, 3);
            this.SectionList.Name = "SectionList";
            this.SectionList.RootElement.ControlBounds = new System.Drawing.Rectangle(0, 0, 150, 250);
            this.SectionList.Size = new System.Drawing.Size(275, 488);
            this.SectionList.TabIndex = 2;
            this.SectionList.ThemeName = "Windows8";
            this.SectionList.ToggleMode = Telerik.WinControls.UI.ToggleMode.SingleClick;
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblTitle.Location = new System.Drawing.Point(8, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(704, 40);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Select the console from the left-hand panel, then the destination node from the r" +
    "ight-hand panel.";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnMoveMode
            // 
            this.btnMoveMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMoveMode.Location = new System.Drawing.Point(538, 8);
            this.btnMoveMode.Name = "btnMoveMode";
            this.btnMoveMode.Size = new System.Drawing.Size(166, 24);
            this.btnMoveMode.TabIndex = 5;
            this.btnMoveMode.Text = "Move To Selected Node";
            this.btnMoveMode.UseVisualStyleBackColor = true;
            this.btnMoveMode.Click += new System.EventHandler(this.btnMoveMode_Click);
            // 
            // chkSection
            // 
            this.chkSection.AutoSize = true;
            this.chkSection.Location = new System.Drawing.Point(3, 12);
            this.chkSection.Name = "chkSection";
            this.chkSection.Size = new System.Drawing.Size(155, 19);
            this.chkSection.TabIndex = 4;
            this.chkSection.Text = "Add to Selected Console";
            this.chkSection.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel.Controls.Add(this.SectionList, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.SectionTreeView, 1, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tableLayoutPanel.Location = new System.Drawing.Point(8, 48);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 1;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(704, 494);
            this.tableLayoutPanel.TabIndex = 1;
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.chkSection);
            this.pnlButtons.Controls.Add(this.btnMoveMode);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlButtons.Location = new System.Drawing.Point(8, 542);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(704, 40);
            this.pnlButtons.TabIndex = 6;
            // 
            // ucAdminKitNodeMover
            // 
            this.AcceptButton = this.btnMoveMode;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(720, 590);
            this.Controls.Add(this.tableLayoutPanel);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ucAdminKitNodeMover";
            this.Padding = new System.Windows.Forms.Padding(8);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Node Re-positioning  Tool";
            ((System.ComponentModel.ISupportInitialize)(this.SectionTreeView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SectionList)).EndInit();
            this.tableLayoutPanel.ResumeLayout(false);
            this.pnlButtons.ResumeLayout(false);
            this.pnlButtons.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadTreeView SectionTreeView;
        private Telerik.WinControls.UI.RadTreeView SectionList;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnMoveMode;
        private Telerik.WinControls.Themes.Windows8Theme windows8Theme1;
        private Telerik.WinControls.Themes.Windows8Theme windows8Theme2;
        private System.Windows.Forms.CheckBox chkSection;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Panel pnlButtons;
    }
}