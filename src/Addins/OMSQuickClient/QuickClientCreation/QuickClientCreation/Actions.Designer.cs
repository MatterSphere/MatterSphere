namespace QuickClientCreation
{
    partial class Actions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Actions));
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnCreateClient = new System.Windows.Forms.Button();
            this.btnCreateMatter = new System.Windows.Forms.Button();
            this.lblConnectionInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(12, 60);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(125, 24);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "&Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnCreateClient
            // 
            this.btnCreateClient.Location = new System.Drawing.Point(12, 89);
            this.btnCreateClient.Name = "btnCreateClient";
            this.btnCreateClient.Size = new System.Drawing.Size(125, 24);
            this.btnCreateClient.TabIndex = 1;
            this.btnCreateClient.Text = "Create C&lient";
            this.btnCreateClient.UseVisualStyleBackColor = true;
            this.btnCreateClient.Click += new System.EventHandler(this.btnCreateClient_Click);
            // 
            // btnCreateMatter
            // 
            this.btnCreateMatter.Location = new System.Drawing.Point(143, 89);
            this.btnCreateMatter.Name = "btnCreateMatter";
            this.btnCreateMatter.Size = new System.Drawing.Size(125, 24);
            this.btnCreateMatter.TabIndex = 2;
            this.btnCreateMatter.Text = "Create &Matter";
            this.btnCreateMatter.UseVisualStyleBackColor = true;
            this.btnCreateMatter.Click += new System.EventHandler(this.btnCreateMatter_Click);
            // 
            // lblConnectionInfo
            // 
            this.lblConnectionInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblConnectionInfo.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblConnectionInfo.Location = new System.Drawing.Point(13, 10);
            this.lblConnectionInfo.Name = "lblConnectionInfo";
            this.lblConnectionInfo.Size = new System.Drawing.Size(297, 46);
            this.lblConnectionInfo.TabIndex = 3;
            // 
            // Actions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(322, 123);
            this.Controls.Add(this.lblConnectionInfo);
            this.Controls.Add(this.btnCreateMatter);
            this.Controls.Add(this.btnCreateClient);
            this.Controls.Add(this.btnConnect);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Actions";
            this.Text = "OMS Quick %CLIENT% & %FILE% Creation";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnCreateClient;
        private System.Windows.Forms.Button btnCreateMatter;
        private System.Windows.Forms.Label lblConnectionInfo;
    }
}

