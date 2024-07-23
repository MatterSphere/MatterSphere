namespace Fwbs.Oms.Office.Common.Panes
{
    partial class GlobalSearchPane
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
            globalSearch = new FWBS.OMS.UI.Elasticsearch.GlobalSearch.ucGlobalSearch();
            this.globalSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // globalSearch
            // 
            this.globalSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.globalSearch.Location = new System.Drawing.Point(0, 121);
            this.globalSearch.Name = "globalSearch";
            this.globalSearch.Size = new System.Drawing.Size(150, 29);
            this.globalSearch.TabIndex = 0;
            // 
            // GlobalSearchPane
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.globalSearch);
            this.Name = "globalSearchPane";
            this.globalSearch.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private FWBS.OMS.UI.Elasticsearch.GlobalSearch.ucGlobalSearch globalSearch;
    }
}
