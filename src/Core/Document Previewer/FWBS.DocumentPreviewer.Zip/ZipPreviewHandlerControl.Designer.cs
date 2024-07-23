namespace Fwbs.Documents.Preview.Zip
{
    partial class ZipPreviewHandlerControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ZipPreviewHandlerControl));
            this.Files = new System.Windows.Forms.TreeView();
            this.iconList = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // Files
            // 
            this.Files.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Files.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Files.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Files.ImageIndex = 0;
            this.Files.ImageList = this.iconList;
            this.Files.Indent = 20;
            this.Files.Location = new System.Drawing.Point(0, 0);
            this.Files.Name = "Files";
            this.Files.SelectedImageIndex = 0;
            this.Files.Size = new System.Drawing.Size(249, 177);
            this.Files.TabIndex = 0;
            // 
            // iconList
            // 
            this.iconList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("iconList.ImageStream")));
            this.iconList.TransparentColor = System.Drawing.Color.Transparent;
            this.iconList.Images.SetKeyName(0, "Folder");
            // 
            // ZipPreviewHandlerControl
            // 
            this.Controls.Add(this.Files);
            this.Name = "ZipPreviewHandlerControl";
            this.Size = new System.Drawing.Size(249, 177);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView Files;
        private System.Windows.Forms.ImageList iconList;
    }
}
