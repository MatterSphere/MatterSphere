namespace FWBS.OMS.Addin.Security.Windows
{
    partial class PermissionItem
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
            this.labPermission = new System.Windows.Forms.Label();
            this.chkDeny = new System.Windows.Forms.CheckBox();
            this.chkAllow = new System.Windows.Forms.CheckBox();
            this.txtFocus = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // labPermission
            // 
            this.labPermission.AutoSize = true;
            this.labPermission.Dock = System.Windows.Forms.DockStyle.Top;
            this.labPermission.Location = new System.Drawing.Point(0, 0);
            this.labPermission.Name = "labPermission";
            this.labPermission.Padding = new System.Windows.Forms.Padding(1, 2, 1, 1);
            this.labPermission.Size = new System.Drawing.Size(61, 16);
            this.labPermission.TabIndex = 0;
            this.labPermission.Text = "Full Control";
            this.labPermission.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labPermission.Click += new System.EventHandler(this.labPermission_Click);
            // 
            // chkDeny
            // 
            this.chkDeny.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkDeny.Dock = System.Windows.Forms.DockStyle.Right;
            this.chkDeny.Location = new System.Drawing.Point(298, 0);
            this.chkDeny.Name = "chkDeny";
            this.chkDeny.Size = new System.Drawing.Size(70, 21);
            this.chkDeny.TabIndex = 2;
            this.chkDeny.UseVisualStyleBackColor = true;
            this.chkDeny.CheckedChanged += new System.EventHandler(this.chkDeny_CheckedChanged);
            this.chkDeny.Click += new System.EventHandler(this.labPermission_Click);
            this.chkDeny.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PermissionItem_KeyDown);
            // 
            // chkAllow
            // 
            this.chkAllow.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkAllow.Dock = System.Windows.Forms.DockStyle.Right;
            this.chkAllow.Location = new System.Drawing.Point(228, 0);
            this.chkAllow.Name = "chkAllow";
            this.chkAllow.Size = new System.Drawing.Size(70, 21);
            this.chkAllow.TabIndex = 1;
            this.chkAllow.UseVisualStyleBackColor = true;
            this.chkAllow.CheckedChanged += new System.EventHandler(this.chkAllow_CheckedChanged);
            this.chkAllow.Click += new System.EventHandler(this.labPermission_Click);
            this.chkAllow.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PermissionItem_KeyDown);
            // 
            // txtFocus
            // 
            this.txtFocus.Location = new System.Drawing.Point(-15, -2);
            this.txtFocus.Multiline = true;
            this.txtFocus.Name = "txtFocus";
            this.txtFocus.ReadOnly = true;
            this.txtFocus.Size = new System.Drawing.Size(10, 20);
            this.txtFocus.TabIndex = 0;
            this.txtFocus.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PermissionItem_KeyDown);
            // 
            // PermissionItem
            // 
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.labPermission);
            this.Controls.Add(this.chkAllow);
            this.Controls.Add(this.chkDeny);
            this.Controls.Add(this.txtFocus);
            this.Name = "PermissionItem";
            this.Size = new System.Drawing.Size(368, 21);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PermissionItem_KeyDown);
            this.ParentChanged += new System.EventHandler(this.PermissionItem_ParentChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labPermission;
        private System.Windows.Forms.CheckBox chkDeny;
        private System.Windows.Forms.CheckBox chkAllow;
        private System.Windows.Forms.TextBox txtFocus;
    }
}
