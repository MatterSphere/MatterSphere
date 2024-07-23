namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.MatterList
{
    partial class BudgetPopup
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
            this.pnlContainer = new System.Windows.Forms.Panel();
            this.pnlTitle = new System.Windows.Forms.Panel();
            this.lblBalances = new System.Windows.Forms.Label();
            this.lblSplitter = new System.Windows.Forms.Label();
            this.lblMatterNo = new System.Windows.Forms.Label();
            this.pnlItems = new System.Windows.Forms.Panel();
            this.pnlContainer.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlContainer
            // 
            this.pnlContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.pnlContainer.Controls.Add(this.pnlItems);
            this.pnlContainer.Controls.Add(this.pnlTitle);
            this.pnlContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContainer.Location = new System.Drawing.Point(7, 8);
            this.pnlContainer.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlContainer.Name = "pnlContainer";
            this.pnlContainer.Padding = new System.Windows.Forms.Padding(12, 10, 12, 10);
            this.pnlContainer.Size = new System.Drawing.Size(226, 164);
            this.pnlContainer.TabIndex = 0;
            // 
            // pnlTitle
            // 
            this.pnlTitle.Controls.Add(this.lblMatterNo);
            this.pnlTitle.Controls.Add(this.lblSplitter);
            this.pnlTitle.Controls.Add(this.lblBalances);
            this.pnlTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F);
            this.pnlTitle.Location = new System.Drawing.Point(12, 10);
            this.pnlTitle.Margin = new System.Windows.Forms.Padding(0);
            this.pnlTitle.Name = "pnlTitle";
            this.pnlTitle.Size = new System.Drawing.Size(202, 20);
            this.pnlTitle.TabIndex = 0;
            // 
            // lblBalances
            // 
            this.lblBalances.AutoSize = true;
            this.lblBalances.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblBalances.Location = new System.Drawing.Point(0, 0);
            this.lblBalances.Name = "lblBalances";
            this.lblBalances.Size = new System.Drawing.Size(63, 19);
            this.lblBalances.TabIndex = 0;
            this.lblBalances.Text = "Balances";
            // 
            // lblSplitter
            // 
            this.lblSplitter.AutoSize = true;
            this.lblSplitter.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblSplitter.Location = new System.Drawing.Point(63, 0);
            this.lblSplitter.Name = "lblSplitter";
            this.lblSplitter.Size = new System.Drawing.Size(15, 19);
            this.lblSplitter.TabIndex = 1;
            this.lblSplitter.Text = "-";
            // 
            // lblMatterNo
            // 
            this.lblMatterNo.AutoSize = true;
            this.lblMatterNo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblMatterNo.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblMatterNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(101)))), ((int)(((byte)(192)))));
            this.lblMatterNo.Location = new System.Drawing.Point(78, 0);
            this.lblMatterNo.Name = "lblMatterNo";
            this.lblMatterNo.Size = new System.Drawing.Size(70, 19);
            this.lblMatterNo.TabIndex = 2;
            this.lblMatterNo.Text = "MatterNo";
            this.lblMatterNo.Click += new System.EventHandler(this.lblMatterNo_Click);
            // 
            // pnlItems
            // 
            this.pnlItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlItems.Location = new System.Drawing.Point(12, 30);
            this.pnlItems.Margin = new System.Windows.Forms.Padding(0);
            this.pnlItems.Name = "pnlItems";
            this.pnlItems.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.pnlItems.Size = new System.Drawing.Size(202, 124);
            this.pnlItems.TabIndex = 1;
            // 
            // BudgetPopup
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pnlContainer);
            this.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MinimumSize = new System.Drawing.Size(240, 180);
            this.Name = "BudgetPopup";
            this.Padding = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this.Size = new System.Drawing.Size(240, 180);
            this.pnlContainer.ResumeLayout(false);
            this.pnlTitle.ResumeLayout(false);
            this.pnlTitle.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlContainer;
        private System.Windows.Forms.Panel pnlTitle;
        private System.Windows.Forms.Label lblBalances;
        private System.Windows.Forms.Label lblMatterNo;
        private System.Windows.Forms.Label lblSplitter;
        private System.Windows.Forms.Panel pnlItems;
    }
}
