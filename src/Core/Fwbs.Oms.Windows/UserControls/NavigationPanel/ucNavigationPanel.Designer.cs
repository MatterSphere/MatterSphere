namespace FWBS.Common.UI.Windows
{
    partial class ucNavigationPanel
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
            this.HeaderPanel = new System.Windows.Forms.Panel();
            this.NavigationLabel = new System.Windows.Forms.Label();
            this.MenuImage = new System.Windows.Forms.PictureBox();
            this.Splitter = new System.Windows.Forms.Panel();
            this.MainPanel = new FWBS.Common.UI.Windows.ucNavigationContainer();
            this.HeaderPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MenuImage)).BeginInit();
            this.SuspendLayout();
            // 
            // HeaderPanel
            // 
            this.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.HeaderPanel.Controls.Add(this.NavigationLabel);
            this.HeaderPanel.Controls.Add(this.MenuImage);
            this.HeaderPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.HeaderPanel.Font = new System.Drawing.Font("Segoe UI Semilight", 12.75F);
            this.HeaderPanel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.HeaderPanel.Location = new System.Drawing.Point(0, 0);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Padding = new System.Windows.Forms.Padding(13, 0, 0, 0);
            this.HeaderPanel.Size = new System.Drawing.Size(280, 48);
            this.HeaderPanel.TabIndex = 1;
            // 
            // NavigationLabel
            // 
            this.NavigationLabel.AutoEllipsis = true;
            this.NavigationLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NavigationLabel.Location = new System.Drawing.Point(43, 0);
            this.resourceLookup.SetLookup(this.NavigationLabel, new FWBS.OMS.UI.Windows.ResourceLookupItem("NAVIGATION", "Navigation", ""));
            this.NavigationLabel.Name = "NavigationLabel";
            this.NavigationLabel.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.NavigationLabel.Size = new System.Drawing.Size(237, 48);
            this.NavigationLabel.TabIndex = 1;
            this.NavigationLabel.Text = "Navigation";
            this.NavigationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MenuImage
            // 
            this.MenuImage.Dock = System.Windows.Forms.DockStyle.Left;
            this.MenuImage.Location = new System.Drawing.Point(13, 0);
            this.MenuImage.Name = "MenuImage";
            this.MenuImage.Size = new System.Drawing.Size(30, 48);
            this.MenuImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.MenuImage.TabIndex = 0;
            this.MenuImage.TabStop = false;
            this.MenuImage.Click += new System.EventHandler(this.MenuImage_Click);
            // 
            // Splitter
            // 
            this.Splitter.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Splitter.Dock = System.Windows.Forms.DockStyle.Top;
            this.Splitter.Location = new System.Drawing.Point(0, 48);
            this.Splitter.Name = "Splitter";
            this.Splitter.Size = new System.Drawing.Size(280, 1);
            this.Splitter.TabIndex = 2;
            // 
            // MainPanel
            // 
            this.MainPanel.AutoScroll = true;
            this.MainPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainPanel.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.MainPanel.Location = new System.Drawing.Point(0, 49);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(280, 251);
            this.MainPanel.TabIndex = 3;
            // 
            // ucNavigationPanel
            // 
            this.Controls.Add(this.MainPanel);
            this.Controls.Add(this.Splitter);
            this.Controls.Add(this.HeaderPanel);
            this.Name = "ucNavigationPanel";
            this.Size = new System.Drawing.Size(280, 300);
            this.HeaderPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MenuImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private FWBS.OMS.UI.Windows.ResourceLookup resourceLookup;
        private System.Windows.Forms.Panel HeaderPanel;
        private System.Windows.Forms.Panel Splitter;
        private FWBS.Common.UI.Windows.ucNavigationContainer MainPanel;
        private System.Windows.Forms.PictureBox MenuImage;
        private System.Windows.Forms.Label NavigationLabel;
    }
}
