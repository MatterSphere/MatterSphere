namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls
{
    partial class ucPageControl
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
            this.resourceLookup = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.btnPrevPage = new System.Windows.Forms.Button();
            this.btnNextPage = new System.Windows.Forms.Button();
            this.leftSpace = new System.Windows.Forms.Panel();
            this.pnlCurrentPage = new System.Windows.Forms.Panel();
            this.tlpCurrentPage = new System.Windows.Forms.TableLayoutPanel();
            this.tbCurrentPage = new System.Windows.Forms.TextBox();
            this.pnlNavPages = new System.Windows.Forms.Panel();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.pnlCurrentPageBorder = new System.Windows.Forms.Panel();
            this.lblOf = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.pnlCurrentPage.SuspendLayout();
            this.tlpCurrentPage.SuspendLayout();
            this.pnlNavPages.SuspendLayout();
            this.pnlCurrentPageBorder.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnPrevPage
            // 
            this.btnPrevPage.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnPrevPage.FlatAppearance.BorderSize = 0;
            this.btnPrevPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrevPage.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnPrevPage.Location = new System.Drawing.Point(0, 0);
            this.btnPrevPage.Name = "btnPrevPage";
            this.btnPrevPage.Padding = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.btnPrevPage.Size = new System.Drawing.Size(30, 30);
            this.btnPrevPage.TabIndex = 0;
            this.btnPrevPage.UseVisualStyleBackColor = true;
            this.btnPrevPage.Click += new System.EventHandler(this.btnPrevPage_Click);
            // 
            // btnNextPage
            // 
            this.btnNextPage.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnNextPage.FlatAppearance.BorderSize = 0;
            this.btnNextPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNextPage.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnNextPage.Location = new System.Drawing.Point(180, 0);
            this.btnNextPage.Name = "btnNextPage";
            this.btnNextPage.Padding = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.btnNextPage.Size = new System.Drawing.Size(30, 30);
            this.btnNextPage.TabIndex = 0;
            this.btnNextPage.UseVisualStyleBackColor = true;
            this.btnNextPage.Click += new System.EventHandler(this.btnNextPage_Click);
            // 
            // leftSpace
            // 
            this.leftSpace.Dock = System.Windows.Forms.DockStyle.Left;
            this.leftSpace.Location = new System.Drawing.Point(30, 0);
            this.leftSpace.Margin = new System.Windows.Forms.Padding(0);
            this.leftSpace.Name = "leftSpace";
            this.leftSpace.Size = new System.Drawing.Size(15, 30);
            this.leftSpace.TabIndex = 0;
            this.leftSpace.TabStop = true;
            // 
            // pnlCurrentPage
            // 
            this.pnlCurrentPage.BackColor = System.Drawing.Color.White;
            this.pnlCurrentPage.Controls.Add(this.tlpCurrentPage);
            this.pnlCurrentPage.Controls.Add(this.pnlNavPages);
            this.pnlCurrentPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCurrentPage.Location = new System.Drawing.Point(1, 1);
            this.pnlCurrentPage.Margin = new System.Windows.Forms.Padding(0);
            this.pnlCurrentPage.Name = "pnlCurrentPage";
            this.pnlCurrentPage.Size = new System.Drawing.Size(58, 28);
            this.pnlCurrentPage.TabIndex = 0;
            this.pnlCurrentPage.TabStop = true;
            // 
            // tlpCurrentPage
            // 
            this.tlpCurrentPage.BackColor = System.Drawing.Color.Transparent;
            this.tlpCurrentPage.ColumnCount = 1;
            this.tlpCurrentPage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpCurrentPage.Controls.Add(this.tbCurrentPage, 0, 1);
            this.tlpCurrentPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpCurrentPage.Location = new System.Drawing.Point(0, 0);
            this.tlpCurrentPage.Name = "tlpCurrentPage";
            this.tlpCurrentPage.RowCount = 3;
            this.tlpCurrentPage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpCurrentPage.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpCurrentPage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpCurrentPage.Size = new System.Drawing.Size(28, 28);
            this.tlpCurrentPage.TabIndex = 0;
            this.tlpCurrentPage.TabStop = true;
            // 
            // tbCurrentPage
            // 
            this.tbCurrentPage.BackColor = System.Drawing.Color.White;
            this.tbCurrentPage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbCurrentPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbCurrentPage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.tbCurrentPage.Location = new System.Drawing.Point(3, 6);
            this.tbCurrentPage.Name = "tbCurrentPage";
            this.tbCurrentPage.Size = new System.Drawing.Size(22, 16);
            this.tbCurrentPage.TabIndex = 1;
            this.tbCurrentPage.TabStop = false;
            this.tbCurrentPage.TextChanged += new System.EventHandler(this.tbCurrentPage_TextChanged);
            this.tbCurrentPage.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.tbCurrentPage_PreviewKeyDown);
            // 
            // pnlNavPages
            // 
            this.pnlNavPages.Controls.Add(this.btnDown);
            this.pnlNavPages.Controls.Add(this.btnUp);
            this.pnlNavPages.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlNavPages.Location = new System.Drawing.Point(28, 0);
            this.pnlNavPages.Name = "pnlNavPages";
            this.pnlNavPages.Size = new System.Drawing.Size(30, 28);
            this.pnlNavPages.TabIndex = 0;
            this.pnlNavPages.TabStop = true;
            // 
            // btnDown
            // 
            this.btnDown.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnDown.FlatAppearance.BorderSize = 0;
            this.btnDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDown.Font = new System.Drawing.Font("Segoe UI", 7F);
            this.btnDown.Location = new System.Drawing.Point(0, 14);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(30, 14);
            this.btnDown.TabIndex = 0;
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnUp
            // 
            this.btnUp.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnUp.FlatAppearance.BorderSize = 0;
            this.btnUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUp.Font = new System.Drawing.Font("Segoe UI", 7F);
            this.btnUp.Location = new System.Drawing.Point(0, 0);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(30, 14);
            this.btnUp.TabIndex = 0;
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // pnlCurrentPageBorder
            // 
            this.pnlCurrentPageBorder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.pnlCurrentPageBorder.Controls.Add(this.pnlCurrentPage);
            this.pnlCurrentPageBorder.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlCurrentPageBorder.Location = new System.Drawing.Point(45, 0);
            this.pnlCurrentPageBorder.Name = "pnlCurrentPageBorder";
            this.pnlCurrentPageBorder.Padding = new System.Windows.Forms.Padding(1);
            this.pnlCurrentPageBorder.Size = new System.Drawing.Size(60, 30);
            this.pnlCurrentPageBorder.TabIndex = 0;
            this.pnlCurrentPageBorder.TabStop = true;
            // 
            // lblOf
            // 
            this.lblOf.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblOf.Location = new System.Drawing.Point(105, 0);
            this.resourceLookup.SetLookup(this.lblOf, new FWBS.OMS.UI.Windows.ResourceLookupItem("OF", "of", ""));
            this.lblOf.Name = "lblOf";
            this.lblOf.Size = new System.Drawing.Size(30, 30);
            this.lblOf.TabIndex = 0;
            this.lblOf.Text = "of";
            this.lblOf.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTotal
            // 
            this.lblTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTotal.Location = new System.Drawing.Point(135, 0);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(45, 30);
            this.lblTotal.TabIndex = 0;
            this.lblTotal.Text = "1";
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ucPageControl
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.lblOf);
            this.Controls.Add(this.pnlCurrentPageBorder);
            this.Controls.Add(this.leftSpace);
            this.Controls.Add(this.btnNextPage);
            this.Controls.Add(this.btnPrevPage);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ucPageControl";
            this.Size = new System.Drawing.Size(210, 30);
            this.pnlCurrentPage.ResumeLayout(false);
            this.tlpCurrentPage.ResumeLayout(false);
            this.tlpCurrentPage.PerformLayout();
            this.pnlNavPages.ResumeLayout(false);
            this.pnlCurrentPageBorder.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Windows.ResourceLookup resourceLookup;
        private System.Windows.Forms.Button btnPrevPage;
        private System.Windows.Forms.Button btnNextPage;
        private System.Windows.Forms.Panel leftSpace;
        private System.Windows.Forms.Panel pnlCurrentPage;
        private System.Windows.Forms.Panel pnlNavPages;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Panel pnlCurrentPageBorder;
        private System.Windows.Forms.TableLayoutPanel tlpCurrentPage;
        private System.Windows.Forms.TextBox tbCurrentPage;
        private System.Windows.Forms.Label lblOf;
        private System.Windows.Forms.Label lblTotal;
    }
}
