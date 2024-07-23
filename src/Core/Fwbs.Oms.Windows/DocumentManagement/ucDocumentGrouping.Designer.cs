namespace FWBS.OMS.UI.Windows.DocumentManagement
{
    partial class ucDocumentGrouping
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
            this.tvGroupings = new FWBS.OMS.UI.TreeView();
            this.SuspendLayout();
            // 
            // tvGroupings
            // 
            this.tvGroupings.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tvGroupings.CheckBoxes = true;
            this.tvGroupings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvGroupings.Indent = 20;
            this.tvGroupings.Location = new System.Drawing.Point(0, 0);
            this.tvGroupings.Name = "tvGroupings";
            this.tvGroupings.Size = new System.Drawing.Size(150, 150);
            this.tvGroupings.TabIndex = 0;
            this.tvGroupings.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvGroupings_AfterCheck);
            this.tvGroupings.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.tvGroupings_AfterExpand);
            // 
            // ucDocumentGrouping
            // 
            this.Controls.Add(this.tvGroupings);
            this.Name = "ucDocumentGrouping";
            this.ResumeLayout(false);

        }

        #endregion

        private FWBS.OMS.UI.TreeView tvGroupings;

    }
}
