namespace FWBS.OMS.UI.DocumentManagement.DocumentFolderManagement.DocumentFolderControls
{
    partial class ucTreeView
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
            this.pnlOptions = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.chkApplyToAll = new System.Windows.Forms.CheckBox();
            this.ucMatterFoldersTree = new FWBS.OMS.UI.Windows.ucMatterFoldersTree();
            this.pnlOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlOptions
            // 
            this.pnlOptions.BackColor = System.Drawing.SystemColors.Control;
            this.pnlOptions.CausesValidation = false;
            this.pnlOptions.Controls.Add(this.btnClose);
            this.pnlOptions.Controls.Add(this.btnApply);
            this.pnlOptions.Controls.Add(this.chkApplyToAll);
            this.pnlOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlOptions.Location = new System.Drawing.Point(0, 0);
            this.pnlOptions.Name = "pnlOptions";
            this.pnlOptions.Size = new System.Drawing.Size(241, 70);
            this.pnlOptions.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(155, 7);
            this.resourceLookup.SetLookup(this.btnClose, new FWBS.OMS.UI.Windows.ResourceLookupItem("CLOSE", "Close", ""));
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(76, 24);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(10, 7);
            this.resourceLookup.SetLookup(this.btnApply, new FWBS.OMS.UI.Windows.ResourceLookupItem("APPLY", "Apply", ""));
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(76, 24);
            this.btnApply.TabIndex = 1;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // chkApplyToAll
            // 
            this.chkApplyToAll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkApplyToAll.Location = new System.Drawing.Point(11, 38);
            this.resourceLookup.SetLookup(this.chkApplyToAll, new FWBS.OMS.UI.Windows.ResourceLookupItem("ApplyToAll", "Apply to all", ""));
            this.chkApplyToAll.Name = "chkApplyToAll";
            this.chkApplyToAll.Size = new System.Drawing.Size(220, 26);
            this.chkApplyToAll.TabIndex = 0;
            this.chkApplyToAll.Text = "Apply to all";
            this.chkApplyToAll.UseVisualStyleBackColor = true;
            this.chkApplyToAll.Click += new System.EventHandler(this.chkApplyToAll_Click);
            // 
            // ucMatterFoldersTree
            // 
            this.ucMatterFoldersTree.AllowDrop = false;
            this.ucMatterFoldersTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucMatterFoldersTree.Location = new System.Drawing.Point(0, 70);
            this.ucMatterFoldersTree.Name = "ucMatterFoldersTree";
            this.ucMatterFoldersTree.ReadOnly = true;
            this.ucMatterFoldersTree.Size = new System.Drawing.Size(241, 422);
            this.ucMatterFoldersTree.TabIndex = 1;
            this.ucMatterFoldersTree.SelectedNodeChangedEvent += UcMatterFoldersTreeSelectedNodeChangedEvent;
            // 
            // ucTreeView
            // 
            this.Controls.Add(this.ucMatterFoldersTree);
            this.Controls.Add(this.pnlOptions);
            this.Name = "ucTreeView";
            this.Size = new System.Drawing.Size(241, 492);
            this.pnlOptions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private FWBS.OMS.UI.Windows.ResourceLookup resourceLookup;
        private System.Windows.Forms.Panel pnlOptions;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.CheckBox chkApplyToAll;
        private System.Windows.Forms.Button btnClose;
        private Windows.ucMatterFoldersTree ucMatterFoldersTree;
    }
}
