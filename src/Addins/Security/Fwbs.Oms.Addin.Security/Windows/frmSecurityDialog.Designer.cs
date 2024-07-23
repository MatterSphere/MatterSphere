namespace FWBS.OMS.Addin.Security.Windows
{
    partial class frmSecurityDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSecurityDialog));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.ucFormStorage1 = new FWBS.OMS.UI.Windows.ucFormStorage(this.components);
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.ucSecurity1 = new FWBS.OMS.Addin.Security.Windows.ucSecurity();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.panel1.Location = new System.Drawing.Point(0, 526);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(10);
            this.panel1.Size = new System.Drawing.Size(899, 45);
            this.panel1.TabIndex = 1;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(734, 10);
            this.resourceLookup1.SetLookup(this.btnOK, new FWBS.OMS.UI.Windows.ResourceLookupItem("OK", "OK", ""));
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 25);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Right;
            this.label1.Location = new System.Drawing.Point(809, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(5, 25);
            this.label1.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(814, 10);
            this.resourceLookup1.SetLookup(this.btnCancel, new FWBS.OMS.UI.Windows.ResourceLookupItem("BTNCANCEL", "Cance&l", ""));
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cance&l";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // ucFormStorage1
            // 
            this.ucFormStorage1.DefaultPercentageHeight = 80;
            this.ucFormStorage1.DefaultPercentageWidth = 80;
            this.ucFormStorage1.FormToStore = this;
            this.ucFormStorage1.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ucFormStorage1.UniqueID = "Security";
            this.ucFormStorage1.Version = ((long)(0));
            // 
            // ucSecurity1
            // 
            this.ucSecurity1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucSecurity1.Location = new System.Drawing.Point(0, 0);
            this.ucSecurity1.Name = "ucSecurity1";
            this.ucSecurity1.Size = new System.Drawing.Size(899, 526);
            this.ucSecurity1.TabIndex = 0;
            this.ucSecurity1.ToBeRefreshed = false;
            // 
            // frmSecurityDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.AcceptButton = this.btnOK;
            this.ClientSize = new System.Drawing.Size(899, 571);
            this.Controls.Add(this.ucSecurity1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSecurityDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Security Permissions";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

            this.FormClosing += FrmSecurityDialog_FormClosing;
        }


        #endregion

        private ucSecurity ucSecurity1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCancel;
        private FWBS.OMS.UI.Windows.ucFormStorage ucFormStorage1;
        private FWBS.OMS.UI.Windows.ResourceLookup resourceLookup1;
    }
}