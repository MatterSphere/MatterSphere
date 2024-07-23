namespace FWBS.OMS.UI.Windows
{
    partial class eADUserSelector
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
            if (disposing)
            {
                if (components != null)
                    components.Dispose();

                backgroundWorker.DoWork -= backgroundWorker_DoWork;
                backgroundWorker.RunWorkerCompleted -= backgroundWorker_RunWorkerCompleted;
                backgroundWorker.Dispose();

                DisposeTreeNodes(null);
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
            this.tvGroups = new FWBS.OMS.UI.TreeView();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // tvGroups
            // 
            this.tvGroups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvGroups.HideSelection = false;
            this.tvGroups.Location = new System.Drawing.Point(0, 0);
            this.tvGroups.Name = "tvGroups";
            this.tvGroups.Size = new System.Drawing.Size(190, 300);
            this.tvGroups.TabIndex = 0;
            this.tvGroups.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvGroups_AfterSelect);
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            // 
            // eADUserSelector
            // 
            this.Controls.Add(this.tvGroups);
            this.Name = "eADUserSelector";
            this.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.Size = new System.Drawing.Size(200, 300);
            this.ResumeLayout(false);

        }

        #endregion

        private FWBS.OMS.UI.TreeView tvGroups;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
    }
}
