namespace FWBS.OMS.HighQ.UserControls
{
    partial class FolderTreeWindow
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
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnUpload = new System.Windows.Forms.Button();
            this.radTreeView = new Telerik.WinControls.UI.RadTreeView();
            this.windows8Theme = new Telerik.WinControls.Themes.Windows8Theme();
            this.pnlButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radTreeView)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Controls.Add(this.btnUpload);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(8, 225);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(268, 28);
            this.pnlButtons.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(118, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 28);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnUpload
            // 
            this.btnUpload.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnUpload.Location = new System.Drawing.Point(193, 0);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(75, 28);
            this.btnUpload.TabIndex = 0;
            this.btnUpload.Text = "Upload";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // radTreeView
            // 
            this.radTreeView.BackColor = System.Drawing.Color.White;
            this.radTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radTreeView.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.radTreeView.ItemHeight = 22;
            this.radTreeView.Location = new System.Drawing.Point(8, 0);
            this.radTreeView.Name = "radTreeView";
            this.radTreeView.Size = new System.Drawing.Size(268, 225);
            this.radTreeView.SpacingBetweenNodes = -1;
            this.radTreeView.TabIndex = 2;
            this.radTreeView.Text = "radTreeView";
            this.radTreeView.ThemeName = "Windows8";
            this.radTreeView.NodeMouseClick += new Telerik.WinControls.UI.RadTreeView.TreeViewEventHandler(this.radTreeView1_NodeMouseClick);
            ((Telerik.WinControls.UI.RadTreeViewElement)(this.radTreeView.GetChildAt(0))).ShowExpandCollapse = true;
            ((Telerik.WinControls.UI.RadTreeViewElement)(this.radTreeView.GetChildAt(0))).ItemHeight = 22;
            ((Telerik.WinControls.UI.RadTreeViewElement)(this.radTreeView.GetChildAt(0))).NodeSpacing = -1;
            ((Telerik.WinControls.UI.RadTreeViewElement)(this.radTreeView.GetChildAt(0))).BorderColor = System.Drawing.Color.White;
            // 
            // FolderTreeWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.radTreeView);
            this.Controls.Add(this.pnlButtons);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FolderTreeWindow";
            this.Padding = new System.Windows.Forms.Padding(8, 0, 8, 8);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "FolderTreeWindow";
            this.pnlButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radTreeView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnUpload;
        private Telerik.WinControls.Themes.Windows8Theme windows8Theme;
        private Telerik.WinControls.UI.RadTreeView radTreeView;
    }
}