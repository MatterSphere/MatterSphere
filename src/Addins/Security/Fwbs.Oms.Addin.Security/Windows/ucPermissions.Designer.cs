namespace FWBS.OMS.Addin.Security.Windows
{
    partial class ucPermissions
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
            this.resLookup = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.labDesc = new System.Windows.Forms.Label();
            this.labAllow = new System.Windows.Forms.Label();
            this.labDeny = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbPolicy = new FWBS.Common.UI.Windows.eXPComboBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.pnlPermissions = new System.Windows.Forms.Panel();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.pnlFakeScroolBar = new System.Windows.Forms.Panel();
            this.eInformation1 = new FWBS.Common.UI.Windows.eInformation();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // labDesc
            // 
            this.labDesc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labDesc.Location = new System.Drawing.Point(0, 0);
            this.resLookup.SetLookup(this.labDesc, new FWBS.OMS.UI.Windows.ResourceLookupItem("DESCRIPTION", "Description", ""));
            this.labDesc.Name = "labDesc";
            this.labDesc.Size = new System.Drawing.Size(196, 19);
            this.labDesc.TabIndex = 0;
            this.labDesc.Text = "Description";
            this.labDesc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labAllow
            // 
            this.labAllow.Dock = System.Windows.Forms.DockStyle.Right;
            this.labAllow.Location = new System.Drawing.Point(196, 0);
            this.resLookup.SetLookup(this.labAllow, new FWBS.OMS.UI.Windows.ResourceLookupItem("ALLOW", "Allow", ""));
            this.labAllow.Name = "labAllow";
            this.labAllow.Size = new System.Drawing.Size(70, 19);
            this.labAllow.TabIndex = 1;
            this.labAllow.Text = "Allow";
            this.labAllow.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labDeny
            // 
            this.labDeny.Dock = System.Windows.Forms.DockStyle.Right;
            this.labDeny.Location = new System.Drawing.Point(266, 0);
            this.resLookup.SetLookup(this.labDeny, new FWBS.OMS.UI.Windows.ResourceLookupItem("DENY", "Deny", ""));
            this.labDeny.Name = "labDeny";
            this.labDeny.Size = new System.Drawing.Size(54, 19);
            this.labDeny.TabIndex = 2;
            this.labDeny.Text = "Deny";
            this.labDeny.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(5, 28);
            this.resLookup.SetLookup(this.label1, new FWBS.OMS.UI.Windows.ResourceLookupItem("AVAILPERMS", "Available Permissions", ""));
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(338, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "Available Permissions";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbPolicy
            // 
            this.cmbPolicy.ActiveSearchEnabled = true;
            this.cmbPolicy.CaptionWidth = 70;
            this.cmbPolicy.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmbPolicy.IsDirty = false;
            this.cmbPolicy.Location = new System.Drawing.Point(5, 5);
            this.resLookup.SetLookup(this.cmbPolicy, new FWBS.OMS.UI.Windows.ResourceLookupItem("POLICY", "Policy", ""));
            this.cmbPolicy.MaxLength = 0;
            this.cmbPolicy.Name = "cmbPolicy";
            this.cmbPolicy.Size = new System.Drawing.Size(338, 23);
            this.cmbPolicy.TabIndex = 5;
            this.cmbPolicy.Text = "Policy";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.pnlPermissions);
            this.splitContainer2.Panel1.Controls.Add(this.pnlHeader);
            this.splitContainer2.Panel1.Controls.Add(this.label1);
            this.splitContainer2.Panel1.Controls.Add(this.cmbPolicy);
            this.splitContainer2.Panel1.Padding = new System.Windows.Forms.Padding(5);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.eInformation1);
            this.splitContainer2.Panel2.Padding = new System.Windows.Forms.Padding(5);
            this.splitContainer2.Size = new System.Drawing.Size(702, 245);
            this.splitContainer2.SplitterDistance = 348;
            this.splitContainer2.TabIndex = 1;
            // 
            // pnlPermissions
            // 
            this.pnlPermissions.AutoScroll = true;
            this.pnlPermissions.BackColor = System.Drawing.SystemColors.Window;
            this.pnlPermissions.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlPermissions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPermissions.Location = new System.Drawing.Point(5, 74);
            this.pnlPermissions.Name = "pnlPermissions";
            this.pnlPermissions.Padding = new System.Windows.Forms.Padding(1);
            this.pnlPermissions.Size = new System.Drawing.Size(338, 166);
            this.pnlPermissions.TabIndex = 3;
            this.pnlPermissions.SizeChanged += new System.EventHandler(this.pnlPermissions_SizeChanged);
            this.pnlPermissions.ParentChanged += new System.EventHandler(this.pnlPermissions_ParentChanged);
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.SystemColors.Control;
            this.pnlHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlHeader.Controls.Add(this.labDesc);
            this.pnlHeader.Controls.Add(this.labAllow);
            this.pnlHeader.Controls.Add(this.labDeny);
            this.pnlHeader.Controls.Add(this.pnlFakeScroolBar);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(5, 53);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(338, 21);
            this.pnlHeader.TabIndex = 4;
            // 
            // pnlFakeScroolBar
            // 
            this.pnlFakeScroolBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlFakeScroolBar.Location = new System.Drawing.Point(320, 0);
            this.pnlFakeScroolBar.Name = "pnlFakeScroolBar";
            this.pnlFakeScroolBar.Size = new System.Drawing.Size(16, 19);
            this.pnlFakeScroolBar.TabIndex = 3;
            this.pnlFakeScroolBar.Visible = false;
            // 
            // eInformation1
            // 
            this.eInformation1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.eInformation1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.eInformation1.Location = new System.Drawing.Point(5, 5);
            this.eInformation1.Name = "eInformation1";
            this.eInformation1.Padding = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.eInformation1.Size = new System.Drawing.Size(340, 235);
            this.eInformation1.TabIndex = 0;
            this.eInformation1.Text = "Context Help connected with the Available Permissions";
            this.eInformation1.Title = "Tip";
            // 
            // ucPermissions
            // 
            this.Controls.Add(this.splitContainer2);
            this.Name = "ucPermissions";
            this.Size = new System.Drawing.Size(702, 245);
            this.ParentChanged += new System.EventHandler(this.pnlPermissions_ParentChanged);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private FWBS.OMS.UI.Windows.ResourceLookup resLookup;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Panel pnlPermissions;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label labDeny;
        private System.Windows.Forms.Label labAllow;
        private System.Windows.Forms.Label labDesc;
        private System.Windows.Forms.Label label1;
        private FWBS.Common.UI.Windows.eXPComboBox cmbPolicy;
        private FWBS.Common.UI.Windows.eInformation eInformation1;
        private System.Windows.Forms.Panel pnlFakeScroolBar;
    }
}
