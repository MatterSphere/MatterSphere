using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows.Admin
{
    /// <summary>
    /// Summary description for frmImportDlg.
    /// </summary>
    public class frmImportDlg : FWBS.OMS.UI.Windows.BaseForm
	{
		#region Fields
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Button btnCancel;
		private FWBS.OMS.UI.TreeView treeView1;
		private FWBS.Common.UI.Windows.eInformation eInformation1;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Button btnUnselectall;
		private System.Windows.Forms.Button btnSelectAll;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Panel panel3;
		public System.Windows.Forms.Button btnFieldReplace;
		private FWBS.OMS.Design.Export.TreeView _treeview;
        private Label labVersion;
        #endregion
        protected ResourceLookup resourceLookup1;
        private Timer timAutoStart;
        private IContainer components;
        private int countdown = 5;

        #region Constructors
        public frmImportDlg(FWBS.OMS.Design.Export.TreeView treeview)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			_treeview = treeview;
            ApplyImages();
            labVersion.Text = string.Empty;
        }

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnFieldReplace = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnUnselectall = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.treeView1 = new FWBS.OMS.UI.TreeView();
            this.eInformation1 = new FWBS.Common.UI.Windows.eInformation();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.labVersion = new System.Windows.Forms.Label();
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.timAutoStart = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnFieldReplace);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.btnSelectAll);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.btnUnselectall);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(459, 6);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(11, 0, 4, 0);
            this.panel1.Size = new System.Drawing.Size(99, 319);
            this.panel1.TabIndex = 0;
            this.panel1.MouseEnter += new System.EventHandler(this.frmImportDlg_MouseEnter);
            // 
            // btnFieldReplace
            // 
            this.btnFieldReplace.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnFieldReplace.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnFieldReplace.Location = new System.Drawing.Point(11, 234);
            this.resourceLookup1.SetLookup(this.btnFieldReplace, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnFieldReplace", "Field Replace", ""));
            this.btnFieldReplace.Name = "btnFieldReplace";
            this.btnFieldReplace.Size = new System.Drawing.Size(84, 25);
            this.btnFieldReplace.TabIndex = 7;
            this.btnFieldReplace.Text = "Field Replace";
            this.btnFieldReplace.Click += new System.EventHandler(this.btnFieldReplace_Click);
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(11, 259);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(84, 5);
            this.panel3.TabIndex = 6;
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnSelectAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSelectAll.Location = new System.Drawing.Point(11, 264);
            this.resourceLookup1.SetLookup(this.btnSelectAll, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnSelectAll", "Select All", ""));
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(84, 25);
            this.btnSelectAll.TabIndex = 3;
            this.btnSelectAll.Text = "Select All";
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(11, 289);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(84, 5);
            this.panel4.TabIndex = 5;
            // 
            // btnUnselectall
            // 
            this.btnUnselectall.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnUnselectall.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnUnselectall.Location = new System.Drawing.Point(11, 294);
            this.resourceLookup1.SetLookup(this.btnUnselectall, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnUnselectall", "Unselect All", ""));
            this.btnUnselectall.Name = "btnUnselectall";
            this.btnUnselectall.Size = new System.Drawing.Size(84, 25);
            this.btnUnselectall.TabIndex = 4;
            this.btnUnselectall.Text = "Unselect All";
            this.btnUnselectall.Click += new System.EventHandler(this.btnUnselectall_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(11, 30);
            this.resourceLookup1.SetLookup(this.btnCancel, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnCancel", "Cance&l", ""));
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(84, 25);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cance&l";
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(11, 25);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(84, 5);
            this.panel2.TabIndex = 1;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(11, 0);
            this.resourceLookup1.SetLookup(this.btnOK, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnOK", "&OK", ""));
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(84, 25);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "&OK";
            this.btnOK.MouseEnter += new System.EventHandler(this.frmImportDlg_MouseEnter);
            // 
            // treeView1
            // 
            this.treeView1.CheckBoxes = true;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Indent = 25;
            this.treeView1.Location = new System.Drawing.Point(227, 6);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(232, 319);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterCheck);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.MouseEnter += new System.EventHandler(this.frmImportDlg_MouseEnter);
            // 
            // eInformation1
            // 
            this.eInformation1.BackColor = System.Drawing.Color.White;
            this.eInformation1.Dock = System.Windows.Forms.DockStyle.Left;
            this.eInformation1.Location = new System.Drawing.Point(7, 6);
            this.eInformation1.Name = "eInformation1";
            this.eInformation1.Padding = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.eInformation1.Size = new System.Drawing.Size(213, 319);
            this.eInformation1.TabIndex = 3;
            this.eInformation1.TabStop = false;
            this.eInformation1.Text = "eInformation1";
            this.eInformation1.Title = "Help Bar";
            this.eInformation1.MouseEnter += new System.EventHandler(this.frmImportDlg_MouseEnter);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(220, 6);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(7, 319);
            this.splitter1.TabIndex = 4;
            this.splitter1.TabStop = false;
            // 
            // labVersion
            // 
            this.labVersion.AutoSize = true;
            this.labVersion.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labVersion.Location = new System.Drawing.Point(7, 325);
            this.resourceLookup1.SetLookup(this.labVersion, new FWBS.OMS.UI.Windows.ResourceLookupItem("Version:", "Version : ", ""));
            this.labVersion.Name = "labVersion";
            this.labVersion.Size = new System.Drawing.Size(54, 15);
            this.labVersion.TabIndex = 5;
            this.labVersion.Text = "Version : ";
            this.labVersion.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // timAutoStart
            // 
            this.timAutoStart.Interval = 1000;
            this.timAutoStart.Tag = "5";
            this.timAutoStart.Tick += new System.EventHandler(this.timAutoStart_Tick);
            // 
            // frmImportDlg
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(565, 346);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.eInformation1);
            this.Controls.Add(this.labVersion);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.resourceLookup1.SetLookup(this, new FWBS.OMS.UI.Windows.ResourceLookupItem("frmImportDlgv8", "3E MatterSphere Package Import", ""));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmImportDlg";
            this.Padding = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "3E MatterSphere Package Import";
            this.Load += new System.EventHandler(this.frmImportDlg_Load);
            this.Shown += new System.EventHandler(this.frmImportDlg_Shown);
            this.MouseEnter += new System.EventHandler(this.frmImportDlg_MouseEnter);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		#region Private
		private void treeView1_AfterCheck(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			System.Data.DataRowView rw = e.Node.Tag as System.Data.DataRowView;
			rw["Active"] = e.Node.Checked;
			if (e.Node.Checked)
			{
				foreach(TreeNode tnn in e.Node.Nodes)
					SelectAll(tnn);
			}
			else
			{
				foreach(TreeNode tnn in e.Node.Nodes)
					UnSelectAll(tnn);
			}

		}

		private void btnSelectAll_Click(object sender, System.EventArgs e)
		{
			foreach(TreeNode tn in treeView1.Nodes)
				SelectAll(tn);
		}

		private void btnUnselectall_Click(object sender, System.EventArgs e)
		{
			foreach(TreeNode tn in treeView1.Nodes)
				UnSelectAll(tn);
		}

		public FWBS.OMS.UI.TreeView TreeView
		{
			get
			{
				return treeView1;
			}
		}

		public void SelectAll(TreeNode tn)
		{
			tn.Checked = true;
			foreach(TreeNode tnn in tn.Nodes)
				SelectAll(tnn);
		}

		public void UnSelectAll(TreeNode tn)
		{
			tn.Checked = false;
			foreach(TreeNode tnn in tn.Nodes)
				UnSelectAll(tnn);
		}

		private void treeView1_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			System.Data.DataRowView rw = e.Node.Tag as System.Data.DataRowView;
			eInformation1.Title = Convert.ToString(rw["Name"]);
			eInformation1.Text = Session.CurrentSession.Terminology.Parse(Convert.ToString(rw["Description"]), true);
		}

		private void frmImportDlg_Load(object sender, System.EventArgs e)
		{
			if (treeView1.Nodes.Count > 0)
				treeView1.SelectedNode = treeView1.Nodes[0];
		}

		private void btnFieldReplace_Click(object sender, System.EventArgs e)
		{
			frmImportReplace frmImpReplace = new frmImportReplace();
			frmImpReplace.FieldReplacer = _treeview.FieldReplacer.Clone();
			frmImpReplace.ShowDialog(this);
			if (frmImpReplace.DialogResult == DialogResult.OK)
			{
				_treeview.FieldReplacer = frmImpReplace.FieldReplacer;
			}

		}

        protected override void OnDpiChanged(DpiChangedEventArgs e)
        {
            base.OnDpiChanged(e);
            ApplyImages();
        }

        private void ApplyImages()
        {
            treeView1.ImageList = FWBS.OMS.UI.Windows.Images.GetAdminMenuList((Images.IconSize)LogicalToDeviceUnits(16));
        }

        #endregion

        #region Public
        private string _version;
        public string Version
        {
            get
            {
                return _version;
            }
            set
            {
                _version = value;
                labVersion.Text = $"{new FWBS.OMS.UI.Windows.ResourceLookupItem("Version:", "Version : ", "")}" + _version;
            }
        }
        #endregion

        private void timAutoStart_Tick(object sender, EventArgs e)
        {
            btnOK.Text = String.Format("OK : {0}", countdown);
            if (countdown == 0)
                btnOK.PerformClick();
            countdown--;
        }

        private void frmImportDlg_Shown(object sender, EventArgs e)
        {
            if (btnFieldReplace.Enabled == false && frmMain.EnabledAutoStart)
            {
                Application.DoEvents();
                timAutoStart.Enabled = true;
            }
        }

        private void frmImportDlg_MouseEnter(object sender, EventArgs e)
        {
            timAutoStart.Enabled = false;
            btnOK.Text = "&OK";
        }
    }
}
