namespace FWBS.OMS.UI.Windows.DocumentManagement
{
    partial class DocumentVersions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DocumentVersions));
            this.label4 = new System.Windows.Forms.Label();
            this.treeView1 = new FWBS.OMS.UI.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.infoPanel = new System.Windows.Forms.Panel();
            this.lblComments = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblCheckedOutBy = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblUpdatedBy = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblCreatedBy = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnLatest = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.btnEmail = new System.Windows.Forms.ToolStripButton();
            this.btnViewComments = new System.Windows.Forms.ToolStripButton();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.infoPanel.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(5, 5);
            this.resourceLookup1.SetLookup(this.label4, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblcreatedby", "Created By", ""));
            this.label4.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Created By";
            // 
            // treeView1
            // 
            this.treeView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView1.FullRowSelect = true;
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageList1;
            this.treeView1.Indent = 20;
            this.treeView1.Location = new System.Drawing.Point(0, 27);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(384, 329);
            this.treeView1.TabIndex = 4;
            this.treeView1.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterCheck);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // infoPanel
            // 
            this.infoPanel.Controls.Add(this.lblComments);
            this.infoPanel.Controls.Add(this.label3);
            this.infoPanel.Controls.Add(this.lblStatus);
            this.infoPanel.Controls.Add(this.label6);
            this.infoPanel.Controls.Add(this.lblCheckedOutBy);
            this.infoPanel.Controls.Add(this.label5);
            this.infoPanel.Controls.Add(this.lblUpdatedBy);
            this.infoPanel.Controls.Add(this.label2);
            this.infoPanel.Controls.Add(this.lblCreatedBy);
            this.infoPanel.Controls.Add(this.label4);
            this.infoPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.infoPanel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.infoPanel.Location = new System.Drawing.Point(384, 0);
            this.infoPanel.Name = "infoPanel";
            this.infoPanel.Padding = new System.Windows.Forms.Padding(5);
            this.infoPanel.Size = new System.Drawing.Size(141, 356);
            this.infoPanel.TabIndex = 5;
            // 
            // lblComments
            // 
            this.lblComments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblComments.Location = new System.Drawing.Point(5, 199);
            this.lblComments.Margin = new System.Windows.Forms.Padding(3, 3, 3, 5);
            this.lblComments.Name = "lblComments";
            this.lblComments.Size = new System.Drawing.Size(131, 152);
            this.lblComments.TabIndex = 2;
            this.lblComments.Text = "...";
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(5, 185);
            this.resourceLookup1.SetLookup(this.label3, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblcomments", "Comments", ""));
            this.label3.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(131, 14);
            this.label3.TabIndex = 4;
            this.label3.Text = "Comments";
            // 
            // lblStatus
            // 
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblStatus.Location = new System.Drawing.Point(5, 153);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(3, 3, 3, 5);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(131, 32);
            this.lblStatus.TabIndex = 11;
            this.lblStatus.Text = "...";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Top;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(5, 140);
            this.resourceLookup1.SetLookup(this.label6, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblstatus", "Status", ""));
            this.label6.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(39, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Status";
            // 
            // lblCheckedOutBy
            // 
            this.lblCheckedOutBy.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCheckedOutBy.Location = new System.Drawing.Point(5, 108);
            this.lblCheckedOutBy.Margin = new System.Windows.Forms.Padding(3, 3, 3, 5);
            this.lblCheckedOutBy.Name = "lblCheckedOutBy";
            this.lblCheckedOutBy.Size = new System.Drawing.Size(131, 32);
            this.lblCheckedOutBy.TabIndex = 9;
            this.lblCheckedOutBy.Text = "...";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(5, 95);
            this.resourceLookup1.SetLookup(this.label5, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblcheckedout", "Checked Out By", ""));
            this.label5.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Checked Out By";
            // 
            // lblUpdatedBy
            // 
            this.lblUpdatedBy.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblUpdatedBy.Location = new System.Drawing.Point(5, 63);
            this.lblUpdatedBy.Margin = new System.Windows.Forms.Padding(3, 3, 3, 5);
            this.lblUpdatedBy.Name = "lblUpdatedBy";
            this.lblUpdatedBy.Size = new System.Drawing.Size(131, 32);
            this.lblUpdatedBy.TabIndex = 8;
            this.lblUpdatedBy.Text = "...";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(5, 50);
            this.resourceLookup1.SetLookup(this.label2, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblupdatedby", "Last Updated By", ""));
            this.label2.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Last Updated By";
            // 
            // lblCreatedBy
            // 
            this.lblCreatedBy.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCreatedBy.Location = new System.Drawing.Point(5, 18);
            this.lblCreatedBy.Margin = new System.Windows.Forms.Padding(3, 3, 3, 5);
            this.lblCreatedBy.Name = "lblCreatedBy";
            this.lblCreatedBy.Size = new System.Drawing.Size(131, 32);
            this.lblCreatedBy.TabIndex = 7;
            this.lblCreatedBy.Text = "...";
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnLatest,
            this.btnDelete,
            this.btnEmail,
            this.btnViewComments});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 1, 4);
            this.toolStrip1.Size = new System.Drawing.Size(384, 27);
            this.toolStrip1.TabIndex = 7;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnLatest
            // 
            this.btnLatest.Image = ((System.Drawing.Image)(resources.GetObject("btnLatest.Image")));
            this.btnLatest.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.resourceLookup1.SetLookup(this.btnLatest, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnLatest", "Flag as Latest", ""));
            this.btnLatest.Name = "btnLatest";
            this.btnLatest.Size = new System.Drawing.Size(97, 20);
            this.btnLatest.Text = "Flag as Latest";
            this.btnLatest.Click += new System.EventHandler(this.btnFlagLatest_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.resourceLookup1.SetLookup(this.btnDelete, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnDelete", "Delete", ""));
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(60, 20);
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnEmail
            // 
            this.btnEmail.Image = ((System.Drawing.Image)(resources.GetObject("btnEmail.Image")));
            this.btnEmail.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.resourceLookup1.SetLookup(this.btnEmail, new FWBS.OMS.UI.Windows.ResourceLookupItem("EMAIL", "Email", ""));
            this.btnEmail.Name = "btnEmail";
            this.btnEmail.Size = new System.Drawing.Size(56, 20);
            this.btnEmail.Text = "Email";
            this.btnEmail.Click += new System.EventHandler(this.btnEmail_Click);
            // 
            // btnViewComments
            // 
            this.btnViewComments.Image = global::FWBS.OMS.UI.Properties.Resources._24;
            this.btnViewComments.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.resourceLookup1.SetLookup(this.btnViewComments, new FWBS.OMS.UI.Windows.ResourceLookupItem("BTNVIEWCMTS", "View Comments", ""));
            this.btnViewComments.Name = "btnViewComments";
            this.btnViewComments.Size = new System.Drawing.Size(114, 20);
            this.btnViewComments.Text = "View Comments";
            this.btnViewComments.Click += new System.EventHandler(this.btnViewComments_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // DocumentVersions
            // 
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.infoPanel);
            this.Name = "DocumentVersions";
            this.Size = new System.Drawing.Size(525, 356);
            this.Load += new System.EventHandler(this.DocumentVersions_Load);
            this.infoPanel.ResumeLayout(false);
            this.infoPanel.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        internal FWBS.OMS.UI.TreeView treeView1;
        private System.Windows.Forms.Panel infoPanel;
        private System.Windows.Forms.Label lblComments;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblCheckedOutBy;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblUpdatedBy;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblCreatedBy;
        private System.Windows.Forms.ImageList imageList1;
        internal System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnLatest;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private ResourceLookup resourceLookup1;
        private System.Windows.Forms.ToolStripButton btnEmail;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripButton btnViewComments;
    }
}
