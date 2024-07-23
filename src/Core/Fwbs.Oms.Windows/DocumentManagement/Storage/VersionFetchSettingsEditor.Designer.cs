namespace FWBS.OMS.UI.Windows.DocumentManagement.Storage
{
    partial class VersionFetchSettingsEditor
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
            this.label1 = new System.Windows.Forms.Label();
            this.versions = new FWBS.OMS.UI.Windows.DocumentManagement.DocumentVersions();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(10, 10);
            this.Resources.SetLookup(this.label1, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblSpecDocReq", "Please specify the particular version of the document you would like to open.", ""));
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(436, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Please specify the particular version of the document you would like to open.";
            // 
            // versions
            // 
            this.versions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.versions.Location = new System.Drawing.Point(10, 30);
            this.versions.Name = "versions";
            this.versions.Size = new System.Drawing.Size(436, 320);
            this.versions.TabIndex = 2;
            // 
            // VersionFetchSettingsEditor
            // 
            this.Controls.Add(this.versions);
            this.Controls.Add(this.label1);
            this.Name = "VersionFetchSettingsEditor";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Size = new System.Drawing.Size(456, 360);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private DocumentVersions versions;



    }
}
