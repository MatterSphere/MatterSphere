namespace FWBS.OMS.UI.UserControls.Dashboard
{
    partial class ucDashboardItemSearch
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
            this.panel = new System.Windows.Forms.Panel();
            this.textContainer = new System.Windows.Forms.Panel();
            this.searchBox = new System.Windows.Forms.TextBox();
            this.resetIcon = new System.Windows.Forms.PictureBox();
            this.searchIcon = new System.Windows.Forms.PictureBox();
            this.blueLine = new System.Windows.Forms.Panel();
            this.panel.SuspendLayout();
            this.textContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resetIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // panel
            // 
            this.panel.Controls.Add(this.textContainer);
            this.panel.Controls.Add(this.resetIcon);
            this.panel.Controls.Add(this.searchIcon);
            this.panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel.Location = new System.Drawing.Point(0, 0);
            this.panel.Margin = new System.Windows.Forms.Padding(0);
            this.panel.Name = "panel";
            this.panel.Padding = new System.Windows.Forms.Padding(5, 5, 5, 3);
            this.panel.Size = new System.Drawing.Size(534, 40);
            this.panel.TabIndex = 0;
            // 
            // textContainer
            // 
            this.textContainer.Controls.Add(this.searchBox);
            this.textContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textContainer.Location = new System.Drawing.Point(37, 5);
            this.textContainer.Name = "textContainer";
            this.textContainer.Padding = new System.Windows.Forms.Padding(8, 6, 8, 0);
            this.textContainer.Size = new System.Drawing.Size(460, 32);
            this.textContainer.TabIndex = 2;
            // 
            // searchBox
            // 
            this.searchBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.searchBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.searchBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.searchBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.searchBox.Location = new System.Drawing.Point(8, 6);
            this.searchBox.Name = "searchBox";
            this.searchBox.Size = new System.Drawing.Size(444, 19);
            this.searchBox.TabIndex = 0;
            this.searchBox.TextChanged += new System.EventHandler(this.searchBox_TextChanged);
            this.searchBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.searchBox_PreviewKeyDown);
            // 
            // resetIcon
            // 
            this.resetIcon.Cursor = System.Windows.Forms.Cursors.Hand;
            this.resetIcon.Dock = System.Windows.Forms.DockStyle.Right;
            this.resetIcon.Location = new System.Drawing.Point(497, 5);
            this.resetIcon.Name = "resetIcon";
            this.resetIcon.Size = new System.Drawing.Size(32, 32);
            this.resetIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.resetIcon.TabIndex = 1;
            this.resetIcon.TabStop = false;
            this.resetIcon.Click += new System.EventHandler(this.resetIcon_Click);
            this.resetIcon.DpiChangedAfterParent += new System.EventHandler(this.resetIcon_DpiChangedAfterParent);
            // 
            // searchIcon
            // 
            this.searchIcon.Dock = System.Windows.Forms.DockStyle.Left;
            this.searchIcon.Location = new System.Drawing.Point(5, 5);
            this.searchIcon.Name = "searchIcon";
            this.searchIcon.Size = new System.Drawing.Size(32, 32);
            this.searchIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.searchIcon.TabIndex = 0;
            this.searchIcon.TabStop = false;
            // 
            // blueLine
            // 
            this.blueLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(101)))), ((int)(((byte)(192)))));
            this.blueLine.Dock = System.Windows.Forms.DockStyle.Top;
            this.blueLine.Location = new System.Drawing.Point(0, 40);
            this.blueLine.Margin = new System.Windows.Forms.Padding(0);
            this.blueLine.Name = "blueLine";
            this.blueLine.Size = new System.Drawing.Size(534, 2);
            this.blueLine.TabIndex = 3;
            // 
            // ucDashboardItemSearch
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.Controls.Add(this.blueLine);
            this.Controls.Add(this.panel);
            this.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "ucDashboardItemSearch";
            this.Size = new System.Drawing.Size(534, 61);
            this.panel.ResumeLayout(false);
            this.textContainer.ResumeLayout(false);
            this.textContainer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resetIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchIcon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.PictureBox searchIcon;
        private System.Windows.Forms.PictureBox resetIcon;
        private System.Windows.Forms.Panel textContainer;
        private System.Windows.Forms.TextBox searchBox;
        private System.Windows.Forms.Panel blueLine;
    }
}
