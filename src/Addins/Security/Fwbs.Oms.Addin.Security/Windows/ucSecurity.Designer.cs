namespace FWBS.OMS.Addin.Security.Windows
{
    partial class ucSecurity
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
            this.ucNavCmdButtons1 = new FWBS.OMS.UI.Windows.ucNavCmdButtons();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ucSearchControl1 = new FWBS.OMS.UI.Windows.ucSearchControl();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.pnlPermissions = new System.Windows.Forms.Panel();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.labDesc = new System.Windows.Forms.Label();
            this.labAllow = new System.Windows.Forms.Label();
            this.labDeny = new System.Windows.Forms.Label();
            this.pnlFakeScroolBar = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbPolicy = new FWBS.Common.UI.Windows.eXPComboBox();
            this.labLocked = new System.Windows.Forms.Label();
            this.eInformation1 = new FWBS.Common.UI.Windows.eInformation();
            this.chkOverright = new System.Windows.Forms.CheckBox();
            this.pnlDesign.SuspendLayout();
            this.pnlActions.SuspendLayout();
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlDesign
            // 
            this.pnlDesign.Size = new System.Drawing.Size(177, 490);
            // 
            // pnlActions
            // 
            this.resourceLookup1.SetLookup(this.pnlActions, new FWBS.OMS.UI.Windows.ResourceLookupItem("Actions", "Actions", ""));
            this.pnlActions.Size = new System.Drawing.Size(161, 31);
            this.pnlActions.Visible = true;
            this.pnlActions.Controls.SetChildIndex(this.navCommands, 0);
            // 
            // navCommands
            // 
            this.navCommands.Resources = FWBS.OMS.UI.Windows.omsImageLists.CoolButtons16;
            this.navCommands.Size = new System.Drawing.Size(161, 0);
            // 
            // ucNavCmdButtons1
            // 
            this.ucNavCmdButtons1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ucNavCmdButtons1.ImageIndex = 51;
            this.ucNavCmdButtons1.Location = new System.Drawing.Point(5, 7);
            this.ucNavCmdButtons1.Name = "ucNavCmdButtons1";
            this.ucNavCmdButtons1.Size = new System.Drawing.Size(151, 22);
            this.ucNavCmdButtons1.TabIndex = 0;
            this.ucNavCmdButtons1.Text = "Add User / Group";
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.splitContainer1);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(177, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(4);
            this.pnlMain.Size = new System.Drawing.Size(663, 490);
            this.pnlMain.TabIndex = 9;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(4, 4);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.ucSearchControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(655, 482);
            this.splitContainer1.SplitterDistance = 216;
            this.splitContainer1.TabIndex = 10;
            // 
            // ucSearchControl1
            // 
            this.ucSearchControl1.BackColor = System.Drawing.Color.White;
            this.ucSearchControl1.BackGroundColor = System.Drawing.Color.White;
            this.ucSearchControl1.ButtonPanelVisible = false;
            this.ucSearchControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucSearchControl1.DoubleClickAction = "None";
            this.ucSearchControl1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ucSearchControl1.Location = new System.Drawing.Point(0, 0);
            this.ucSearchControl1.Name = "ucSearchControl1";
            this.ucSearchControl1.NavCommandPanel = this.navCommands;
            this.ucSearchControl1.Padding = new System.Windows.Forms.Padding(5);
            this.ucSearchControl1.SearchListCode = "";
            this.ucSearchControl1.SearchListType = "";
            this.ucSearchControl1.SearchPanelVisible = false;
            this.ucSearchControl1.Size = new System.Drawing.Size(655, 216);
            this.ucSearchControl1.TabIndex = 10;
            this.ucSearchControl1.ToBeRefreshed = false;
            this.ucSearchControl1.TypeSelectorVisible = false;
            this.ucSearchControl1.ItemHovered += new System.EventHandler(this.ucSearchControl1_ItemHovered);
            this.ucSearchControl1.SearchListLoad += new System.EventHandler(this.ucSearchControl1_SearchListLoad);
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
            this.splitContainer2.Panel2.Controls.Add(this.labLocked);
            this.splitContainer2.Panel2.Controls.Add(this.eInformation1);
            this.splitContainer2.Panel2.Controls.Add(this.chkOverright);
            this.splitContainer2.Panel2.Padding = new System.Windows.Forms.Padding(5);
            this.splitContainer2.Size = new System.Drawing.Size(655, 262);
            this.splitContainer2.SplitterDistance = 325;
            this.splitContainer2.TabIndex = 0;
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
            this.pnlPermissions.Size = new System.Drawing.Size(315, 183);
            this.pnlPermissions.TabIndex = 3;
            this.pnlPermissions.SizeChanged += new System.EventHandler(this.pnlPermissions_SizeChanged);
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
            this.pnlHeader.Size = new System.Drawing.Size(315, 21);
            this.pnlHeader.TabIndex = 4;
            // 
            // labDesc
            // 
            this.labDesc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labDesc.Location = new System.Drawing.Point(0, 0);
            this.resourceLookup1.SetLookup(this.labDesc, new FWBS.OMS.UI.Windows.ResourceLookupItem("DESCRIPTION", "Description", ""));
            this.labDesc.Name = "labDesc";
            this.labDesc.Size = new System.Drawing.Size(173, 19);
            this.labDesc.TabIndex = 0;
            this.labDesc.Text = "Description";
            this.labDesc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labAllow
            // 
            this.labAllow.Dock = System.Windows.Forms.DockStyle.Right;
            this.labAllow.Location = new System.Drawing.Point(173, 0);
            this.resourceLookup1.SetLookup(this.labAllow, new FWBS.OMS.UI.Windows.ResourceLookupItem("ALLOW", "Allow", ""));
            this.labAllow.Name = "labAllow";
            this.labAllow.Size = new System.Drawing.Size(70, 19);
            this.labAllow.TabIndex = 1;
            this.labAllow.Text = "Allow";
            this.labAllow.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labDeny
            // 
            this.labDeny.Dock = System.Windows.Forms.DockStyle.Right;
            this.labDeny.Location = new System.Drawing.Point(243, 0);
            this.resourceLookup1.SetLookup(this.labDeny, new FWBS.OMS.UI.Windows.ResourceLookupItem("DENY", "Deny", ""));
            this.labDeny.Name = "labDeny";
            this.labDeny.Size = new System.Drawing.Size(54, 19);
            this.labDeny.TabIndex = 2;
            this.labDeny.Text = "Deny";
            this.labDeny.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlFakeScroolBar
            // 
            this.pnlFakeScroolBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlFakeScroolBar.Location = new System.Drawing.Point(297, 0);
            this.pnlFakeScroolBar.Name = "pnlFakeScroolBar";
            this.pnlFakeScroolBar.Size = new System.Drawing.Size(16, 19);
            this.pnlFakeScroolBar.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(5, 28);
            this.resourceLookup1.SetLookup(this.label1, new FWBS.OMS.UI.Windows.ResourceLookupItem("AVAILPERMS", "Available Permissions", ""));
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(315, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = " Available Permissions";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbPolicy
            // 
            this.cmbPolicy.ActiveSearchEnabled = true;
            this.cmbPolicy.CaptionWidth = 70;
            this.cmbPolicy.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmbPolicy.IsDirty = false;
            this.cmbPolicy.Location = new System.Drawing.Point(5, 5);
            this.resourceLookup1.SetLookup(this.cmbPolicy, new FWBS.OMS.UI.Windows.ResourceLookupItem("POLICY", "Policy", ""));
            this.cmbPolicy.MaxLength = 0;
            this.cmbPolicy.Name = "cmbPolicy";
            this.cmbPolicy.Size = new System.Drawing.Size(315, 23);
            this.cmbPolicy.TabIndex = 5;
            this.cmbPolicy.Text = "Policy";
            this.cmbPolicy.ActiveChanged += new System.EventHandler(this.cmbPolicy_ActiveChanged);
            // 
            // labLocked
            // 
            this.labLocked.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labLocked.AutoSize = true;
            this.labLocked.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labLocked.Location = new System.Drawing.Point(216, 188);
            this.resourceLookup1.SetLookup(this.labLocked, new FWBS.OMS.UI.Windows.ResourceLookupItem("LOCKED", "LOCKED", ""));
            this.labLocked.Name = "labLocked";
            this.labLocked.Size = new System.Drawing.Size(92, 24);
            this.labLocked.TabIndex = 1;
            this.labLocked.Text = "LOCKED";
            this.labLocked.Visible = false;
            // 
            // eInformation1
            // 
            this.eInformation1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.eInformation1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.eInformation1.Location = new System.Drawing.Point(5, 5);
            this.eInformation1.Name = "eInformation1";
            this.eInformation1.Padding = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.eInformation1.Size = new System.Drawing.Size(316, 221);
            this.eInformation1.TabIndex = 0;
            this.eInformation1.Text = "Context Help connected with the Available Permissions";
            this.eInformation1.Title = "Tip";
            // 
            // chkOverright
            // 
            this.chkOverright.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.chkOverright.Location = new System.Drawing.Point(5, 226);
            this.resourceLookup1.SetLookup(this.chkOverright, new FWBS.OMS.UI.Windows.ResourceLookupItem("OVERWRITEPERM", "Overwrite Child Permissions (Requires Security Admin)", ""));
            this.chkOverright.Name = "chkOverright";
            this.chkOverright.Size = new System.Drawing.Size(316, 31);
            this.chkOverright.TabIndex = 4;
            this.chkOverright.Text = "Overwrite Child Permissions (Requires Security Admin)";
            this.chkOverright.UseVisualStyleBackColor = true;
            this.chkOverright.CheckedChanged += new System.EventHandler(this.chkOverright_CheckedChanged);
            // 
            // ucSecurity
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pnlMain);
            this.Name = "ucSecurity";
            this.SizeChanged += new System.EventHandler(this.ucSecurity_SizeChanged);
            this.Controls.SetChildIndex(this.pnlDesign, 0);
            this.Controls.SetChildIndex(this.pnlMain, 0);
            this.pnlDesign.ResumeLayout(false);
            this.pnlActions.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private FWBS.OMS.UI.Windows.ucNavCmdButtons ucNavCmdButtons1;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private FWBS.OMS.UI.Windows.ucSearchControl ucSearchControl1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private FWBS.Common.UI.Windows.eInformation eInformation1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlPermissions;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label labDesc;
        private System.Windows.Forms.Label labDeny;
        private System.Windows.Forms.Label labAllow;
        private FWBS.Common.UI.Windows.eXPComboBox cmbPolicy;
        private System.Windows.Forms.Label labLocked;
        private System.Windows.Forms.CheckBox chkOverright;
        private System.Windows.Forms.Panel pnlFakeScroolBar;
    }
}
