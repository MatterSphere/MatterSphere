namespace FWBS.OMS.UI.Windows
{
    partial class ucMatterFoldersTree
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
            this.windows8Theme1 = new Telerik.WinControls.Themes.Windows8Theme();
            this.fileTreeView = new FWBS.OMS.UI.UserControls.TreeView.RadTreeViewEx();
            ((System.ComponentModel.ISupportInitialize)(this.fileTreeView)).BeginInit();
            this.SuspendLayout();
            // 
            // fileTreeView
            // 
            this.fileTreeView.AllowDragDrop = true;
            this.fileTreeView.AllowDrop = false;
            this.fileTreeView.BackColor = System.Drawing.Color.White;
            this.fileTreeView.Cursor = System.Windows.Forms.Cursors.Default;
            this.fileTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileTreeView.EnableKeyMap = true;
            this.fileTreeView.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.fileTreeView.ForeColor = System.Drawing.SystemColors.ControlText;
            this.fileTreeView.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(251)))), ((int)(((byte)(252)))));
            this.fileTreeView.Location = new System.Drawing.Point(0, 0);
            this.fileTreeView.Margin = new System.Windows.Forms.Padding(0);
            this.fileTreeView.Name = "fileTreeView";
            this.fileTreeView.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.fileTreeView.RootElement.ControlBounds = new System.Drawing.Rectangle(0, 0, 150, 250);
            this.fileTreeView.Size = new System.Drawing.Size(200, 280);
            this.fileTreeView.SpacingBetweenNodes = 5;
            this.fileTreeView.TabIndex = 23;
            this.fileTreeView.ThemeName = "Windows8";
            this.fileTreeView.TreeIndent = 10;
            this.fileTreeView.DragEnding += new Telerik.WinControls.UI.RadTreeView.DragEndingHandler(this.fileTreeView_DragEnding);
            this.fileTreeView.DragDrop += new System.Windows.Forms.DragEventHandler(this.fileTreeView_DragDrop);
            this.fileTreeView.DragEnter += new System.Windows.Forms.DragEventHandler(this.fileTreeView_DragEnter);
            // 
            // ucMatterFoldersTree
            // 
            this.Controls.Add(this.fileTreeView);
            this.Name = "ucMatterFoldersTree";
            this.Size = new System.Drawing.Size(200, 280);
            ((System.ComponentModel.ISupportInitialize)(this.fileTreeView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
    }
}
