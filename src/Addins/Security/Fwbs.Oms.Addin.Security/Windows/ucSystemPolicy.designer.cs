namespace FWBS.OMS.Addin.Security.Windows
{
    partial class ucSystemPolicy
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
            this.pageSetupDialog1 = new System.Windows.Forms.PageSetupDialog();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.pnlPermissions = new System.Windows.Forms.Panel();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.labDesc = new System.Windows.Forms.Label();
            this.labAllow = new System.Windows.Forms.Label();
            this.labDeny = new System.Windows.Forms.Label();
            this.pnlFakeScroolBar = new System.Windows.Forms.Panel();
            this.txtActiveFilter = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.eInformation1 = new FWBS.Common.UI.Windows.eInformation();
            this.label1 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.labDescription = new System.Windows.Forms.Label();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.tpList.SuspendLayout();
            this.tpEdit.SuspendLayout();
            this.pnlEdit.SuspendLayout();
            this.pnlToolbarContainer.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tpEdit
            // 
            this.tpEdit.Controls.Add(this.panel1);
            this.tpEdit.Controls.Add(this.pnlLeft);
            this.BresourceLookup1.SetLookup(this.tpEdit, new FWBS.OMS.UI.Windows.ResourceLookupItem("Edit", "Edit", ""));
            this.tpEdit.Controls.SetChildIndex(this.pnlEdit, 0);
            this.tpEdit.Controls.SetChildIndex(this.pnlLeft, 0);
            this.tpEdit.Controls.SetChildIndex(this.panel1, 0);
            // 
            // labSelectedObject
            // 
            this.labSelectedObject.Text = "%1% - Policy";
            // 
            // tbSave
            // 
            this.BresourceLookup1.SetLookup(this.tbSave, new FWBS.OMS.UI.Windows.ResourceLookupItem("Save", "Save", ""));
            // 
            // tbClose
            // 
            this.BresourceLookup1.SetLookup(this.tbClose, new FWBS.OMS.UI.Windows.ResourceLookupItem("Close", "Close", ""));
            // 
            // tbReturn
            // 
            this.BresourceLookup1.SetLookup(this.tbReturn, new FWBS.OMS.UI.Windows.ResourceLookupItem("Return", "Return", ""));
            // 
            // pnlLeft
            // 
            this.pnlLeft.Controls.Add(this.pnlPermissions);
            this.pnlLeft.Controls.Add(this.pnlHeader);
            this.pnlLeft.Controls.Add(this.txtActiveFilter);
            this.pnlLeft.Controls.Add(this.label2);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 50);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(327, 333);
            this.pnlLeft.TabIndex = 1;
            // 
            // pnlPermissions
            // 
            this.pnlPermissions.AutoScroll = true;
            this.pnlPermissions.BackColor = System.Drawing.SystemColors.Window;
            this.pnlPermissions.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlPermissions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPermissions.Location = new System.Drawing.Point(0, 62);
            this.pnlPermissions.Name = "pnlPermissions";
            this.pnlPermissions.Padding = new System.Windows.Forms.Padding(1);
            this.pnlPermissions.Size = new System.Drawing.Size(327, 271);
            this.pnlPermissions.TabIndex = 6;
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
            this.pnlHeader.Location = new System.Drawing.Point(0, 41);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(327, 21);
            this.pnlHeader.TabIndex = 7;
            // 
            // labDesc
            // 
            this.labDesc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labDesc.Location = new System.Drawing.Point(0, 0);
            this.BresourceLookup1.SetLookup(this.labDesc, new FWBS.OMS.UI.Windows.ResourceLookupItem("DESCRIPTION", "Description", ""));
            this.labDesc.Name = "labDesc";
            this.labDesc.Size = new System.Drawing.Size(185, 19);
            this.labDesc.TabIndex = 0;
            this.labDesc.Text = "Description";
            this.labDesc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labAllow
            // 
            this.labAllow.Dock = System.Windows.Forms.DockStyle.Right;
            this.labAllow.Location = new System.Drawing.Point(185, 0);
            this.BresourceLookup1.SetLookup(this.labAllow, new FWBS.OMS.UI.Windows.ResourceLookupItem("ALLOW", "Allow", ""));
            this.labAllow.Name = "labAllow";
            this.labAllow.Size = new System.Drawing.Size(70, 19);
            this.labAllow.TabIndex = 1;
            this.labAllow.Text = "Allow";
            this.labAllow.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labDeny
            // 
            this.labDeny.Dock = System.Windows.Forms.DockStyle.Right;
            this.labDeny.Location = new System.Drawing.Point(255, 0);
            this.BresourceLookup1.SetLookup(this.labDeny, new FWBS.OMS.UI.Windows.ResourceLookupItem("DENY", "Deny", ""));
            this.labDeny.Name = "labDeny";
            this.labDeny.Size = new System.Drawing.Size(54, 19);
            this.labDeny.TabIndex = 2;
            this.labDeny.Text = "Deny";
            this.labDeny.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlFakeScroolBar
            // 
            this.pnlFakeScroolBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlFakeScroolBar.Location = new System.Drawing.Point(309, 0);
            this.pnlFakeScroolBar.Name = "pnlFakeScroolBar";
            this.pnlFakeScroolBar.Size = new System.Drawing.Size(16, 19);
            this.pnlFakeScroolBar.TabIndex = 3;
            // 
            // txtActiveFilter
            // 
            this.txtActiveFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtActiveFilter.Location = new System.Drawing.Point(0, 18);
            this.txtActiveFilter.Name = "txtActiveFilter";
            this.txtActiveFilter.Size = new System.Drawing.Size(327, 23);
            this.txtActiveFilter.TabIndex = 10;
            this.txtActiveFilter.TextChanged += new System.EventHandler(this.txtActiveFilter_TextChanged);
            this.txtActiveFilter.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.txtActiveFilter_PreviewKeyDown);
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.BresourceLookup1.SetLookup(this.label2, new FWBS.OMS.UI.Windows.ResourceLookupItem("SECFILTERPERM", "Filter Permission", ""));
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(327, 18);
            this.label2.TabIndex = 13;
            this.label2.Text = "Filter Permission";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.eInformation1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtName);
            this.panel1.Controls.Add(this.labDescription);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(327, 50);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(10);
            this.panel1.Size = new System.Drawing.Size(222, 333);
            this.panel1.TabIndex = 2;
            // 
            // eInformation1
            // 
            this.eInformation1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.eInformation1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.eInformation1.Location = new System.Drawing.Point(10, 69);
            this.eInformation1.Margin = new System.Windows.Forms.Padding(10);
            this.eInformation1.Name = "eInformation1";
            this.eInformation1.Padding = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.eInformation1.Size = new System.Drawing.Size(202, 254);
            this.eInformation1.TabIndex = 4;
            this.eInformation1.Text = "Context Help connected with the Available Permissions";
            this.eInformation1.Title = "Tip";
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(10, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(202, 18);
            this.label1.TabIndex = 12;
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtName
            // 
            this.txtName.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtName.Location = new System.Drawing.Point(10, 28);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(202, 23);
            this.txtName.TabIndex = 11;
            // 
            // labDescription
            // 
            this.labDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.labDescription.Location = new System.Drawing.Point(10, 10);
            this.BresourceLookup1.SetLookup(this.labDescription, new FWBS.OMS.UI.Windows.ResourceLookupItem("SECNAME", "Name : ", ""));
            this.labDescription.Name = "labDescription";
            this.labDescription.Size = new System.Drawing.Size(202, 18);
            this.labDescription.TabIndex = 10;
            this.labDescription.Text = "Name : ";
            this.labDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // ucSystemPolicy
            // 
            this.Name = "ucSystemPolicy";
            this.tpList.ResumeLayout(false);
            this.tpEdit.ResumeLayout(false);
            this.pnlEdit.ResumeLayout(false);
            this.pnlToolbarContainer.ResumeLayout(false);
            this.pnlToolbarContainer.PerformLayout();
            this.pnlLeft.ResumeLayout(false);
            this.pnlLeft.PerformLayout();
            this.pnlHeader.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PageSetupDialog pageSetupDialog1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Panel pnlPermissions;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label labDeny;
        private System.Windows.Forms.Label labAllow;
        private System.Windows.Forms.Label labDesc;
        private System.Windows.Forms.TextBox txtActiveFilter;
        private System.Windows.Forms.ImageList imageList1;
        private FWBS.Common.UI.Windows.eInformation eInformation1;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label labDescription;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel pnlFakeScroolBar;
    }
}
