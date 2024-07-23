namespace FWBS.OMS.UI.Windows.Admin
{
    partial class frmWarningV5
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmWarningV5));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.chkDontShowAgain = new System.Windows.Forms.CheckBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labMatterSphereV6 = new System.Windows.Forms.Label();
            this.labV6Warning = new System.Windows.Forms.Label();
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Controls.Add(this.chkDontShowAgain);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.panel1.Location = new System.Drawing.Point(0, 146);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(403, 46);
            this.panel1.TabIndex = 2;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOK.Location = new System.Drawing.Point(315, 11);
            this.resourceLookup1.SetLookup(this.btnOK, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnOK", "&OK", ""));
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 24);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // chkDontShowAgain
            // 
            this.chkDontShowAgain.AutoSize = true;
            this.chkDontShowAgain.Location = new System.Drawing.Point(12, 15);
            this.resourceLookup1.SetLookup(this.chkDontShowAgain, new FWBS.OMS.UI.Windows.ResourceLookupItem("chkDntShwAgn", "Do not show me again.", ""));
            this.chkDontShowAgain.Name = "chkDontShowAgain";
            this.chkDontShowAgain.Size = new System.Drawing.Size(148, 19);
            this.chkDontShowAgain.TabIndex = 1;
            this.chkDontShowAgain.Text = "Do not show me again.";
            this.chkDontShowAgain.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // labMatterSphereV6
            // 
            this.labMatterSphereV6.AutoSize = true;
            this.labMatterSphereV6.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labMatterSphereV6.Location = new System.Drawing.Point(51, 31);
            this.resourceLookup1.SetLookup(this.labMatterSphereV6, new FWBS.OMS.UI.Windows.ResourceLookupItem("labMttrCntrV7", string.Format("{0} V{1}", FWBS.OMS.Global.ApplicationName, FWBS.OMS.Session.CurrentSession.AssemblyVersion.Major), ""));
            this.labMatterSphereV6.Name = "labMatterSphereV6";
            this.labMatterSphereV6.Size = new System.Drawing.Size(110, 14);
            this.labMatterSphereV6.TabIndex = 4;
            // 
            // labV6Warning
            // 
            this.labV6Warning.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labV6Warning.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labV6Warning.Location = new System.Drawing.Point(12, 58);
            this.resourceLookup1.SetLookup(this.labV6Warning, new FWBS.OMS.UI.Windows.ResourceLookupItem("V7Warning1", resources.GetString("labV6Warning.Lookup"), ""));
            this.labV6Warning.Name = "labV6Warning";
            this.labV6Warning.Size = new System.Drawing.Size(378, 66);
            this.labV6Warning.TabIndex = 5;
            this.labV6Warning.Text = resources.GetString("labV6Warning.Text");
            // 
            // frmWarningV5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(403, 192);
            this.Controls.Add(this.labV6Warning);
            this.Controls.Add(this.labMatterSphereV6);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.panel1);
            this.resourceLookup1.SetLookup(this, new FWBS.OMS.UI.Windows.ResourceLookupItem("frmWarningV6", GetBrandedWarning(), ""));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmWarningV5";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = GetBrandedWarning();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private static string GetBrandedWarning()
        {
            return string.Format("{0} Warning", FWBS.OMS.Global.ApplicationName);
        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnOK; 
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labMatterSphereV6;
        private System.Windows.Forms.Label labV6Warning;
        private ResourceLookup resourceLookup1;
        internal System.Windows.Forms.CheckBox chkDontShowAgain;
    }
}
