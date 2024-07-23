using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows.Design
{
    /// <summary>
    /// Summary description for frmDataDirectory.
    /// </summary>
    public class frmDataDirectory : FWBS.OMS.UI.Windows.BaseForm
	{
		private DataSet _dataobjects = null;
		public FWBS.OMS.UI.TabControl tbcDataType;
		public System.Windows.Forms.TabPage tpTables;
		public System.Windows.Forms.ListBox lstTables;
		public System.Windows.Forms.TabPage tpQueries;
		public System.Windows.Forms.ListBox lstQueries;
		public System.Windows.Forms.TabPage tpStoredProcs;
		public System.Windows.Forms.ListBox lstStoredProcs;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btnHelp;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Panel pnlSearch;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.TextBox txtSearchT;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.TextBox txtSearchQ;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Panel panel5;
		private System.Windows.Forms.Panel panel6;
		private System.Windows.Forms.TextBox txtSearchSP;
        protected ResourceLookup resourceLookup1;
        private IContainer components;
		private System.Windows.Forms.Label label3;

		private frmDataDirectory()
		{
		}

		public frmDataDirectory(DataSet DataObjects)
		{
			//
			// Required for Windows Form Designer support
			//
			_dataobjects = DataObjects;
			InitializeComponent();
			if (_dataobjects != null)
			{
				lstTables.DataSource = _dataobjects.Tables["TABLES"];
				lstTables.DisplayMember = "TABLE_NAME";
				lstTables.ValueMember = "TABLE_NAME";
				lstQueries.DataSource = _dataobjects.Tables["VIEWS"];
				lstQueries.DisplayMember = "TABLE_NAME";
				lstQueries.ValueMember = "TABLE_NAME";
				lstStoredProcs.DataSource = _dataobjects.Tables["PROCEDURES"];
				lstStoredProcs.DisplayMember = "PROCEDURE_NAME";
				lstStoredProcs.ValueMember = "PROCEDURE_NAME";
			}
		}


		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.tbcDataType = new FWBS.OMS.UI.TabControl();
            this.tpTables = new System.Windows.Forms.TabPage();
            this.lstTables = new System.Windows.Forms.ListBox();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtSearchT = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tpQueries = new System.Windows.Forms.TabPage();
            this.lstQueries = new System.Windows.Forms.ListBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.txtSearchQ = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tpStoredProcs = new System.Windows.Forms.TabPage();
            this.lstStoredProcs = new System.Windows.Forms.ListBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.txtSearchSP = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnHelp = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.tbcDataType.SuspendLayout();
            this.tpTables.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tpQueries.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.tpStoredProcs.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbcDataType
            // 
            this.tbcDataType.Controls.Add(this.tpTables);
            this.tbcDataType.Controls.Add(this.tpQueries);
            this.tbcDataType.Controls.Add(this.tpStoredProcs);
            this.tbcDataType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbcDataType.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tbcDataType.Location = new System.Drawing.Point(4, 4);
            this.tbcDataType.Name = "tbcDataType";
            this.tbcDataType.SelectedIndex = 0;
            this.tbcDataType.Size = new System.Drawing.Size(316, 301);
            this.tbcDataType.TabIndex = 2;
            this.tbcDataType.DoubleClick += new System.EventHandler(this.btnOK_Click);
            // 
            // tpTables
            // 
            this.tpTables.BackColor = System.Drawing.Color.White;
            this.tpTables.Controls.Add(this.lstTables);
            this.tpTables.Controls.Add(this.pnlSearch);
            this.tpTables.Location = new System.Drawing.Point(4, 24);
            this.resourceLookup1.SetLookup(this.tpTables, new FWBS.OMS.UI.Windows.ResourceLookupItem("Tables", "Tables", ""));
            this.tpTables.Name = "tpTables";
            this.tpTables.Padding = new System.Windows.Forms.Padding(8);
            this.tpTables.Size = new System.Drawing.Size(308, 273);
            this.tpTables.TabIndex = 0;
            this.tpTables.Text = "Tables";
            // 
            // lstTables
            // 
            this.lstTables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstTables.Location = new System.Drawing.Point(8, 35);
            this.lstTables.Name = "lstTables";
            this.lstTables.Size = new System.Drawing.Size(292, 230);
            this.lstTables.TabIndex = 1;
            // 
            // pnlSearch
            // 
            this.pnlSearch.Controls.Add(this.panel2);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearch.Location = new System.Drawing.Point(8, 8);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(292, 27);
            this.pnlSearch.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.txtSearchT);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(292, 23);
            this.panel2.TabIndex = 0;
            // 
            // txtSearchT
            // 
            this.txtSearchT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSearchT.Location = new System.Drawing.Point(48, 0);
            this.txtSearchT.Name = "txtSearchT";
            this.txtSearchT.Size = new System.Drawing.Size(244, 23);
            this.txtSearchT.TabIndex = 0;
            this.txtSearchT.TextChanged += new System.EventHandler(this.txtSearchT_TextChanged);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.resourceLookup1.SetLookup(this.label1, new FWBS.OMS.UI.Windows.ResourceLookupItem("Search:", "Search : ", ""));
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Search : ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tpQueries
            // 
            this.tpQueries.BackColor = System.Drawing.Color.White;
            this.tpQueries.Controls.Add(this.lstQueries);
            this.tpQueries.Controls.Add(this.panel3);
            this.tpQueries.Location = new System.Drawing.Point(4, 24);
            this.resourceLookup1.SetLookup(this.tpQueries, new FWBS.OMS.UI.Windows.ResourceLookupItem("Queries", "Queries", ""));
            this.tpQueries.Name = "tpQueries";
            this.tpQueries.Padding = new System.Windows.Forms.Padding(8);
            this.tpQueries.Size = new System.Drawing.Size(308, 273);
            this.tpQueries.TabIndex = 1;
            this.tpQueries.Text = "Queries";
            this.tpQueries.Visible = false;
            // 
            // lstQueries
            // 
            this.lstQueries.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstQueries.Location = new System.Drawing.Point(8, 35);
            this.lstQueries.Name = "lstQueries";
            this.lstQueries.Size = new System.Drawing.Size(292, 230);
            this.lstQueries.TabIndex = 1;
            this.lstQueries.DoubleClick += new System.EventHandler(this.btnOK_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(8, 8);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(292, 27);
            this.panel3.TabIndex = 3;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.txtSearchQ);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(292, 23);
            this.panel4.TabIndex = 0;
            // 
            // txtSearchQ
            // 
            this.txtSearchQ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSearchQ.Location = new System.Drawing.Point(48, 0);
            this.txtSearchQ.Name = "txtSearchQ";
            this.txtSearchQ.Size = new System.Drawing.Size(244, 23);
            this.txtSearchQ.TabIndex = 0;
            this.txtSearchQ.TextChanged += new System.EventHandler(this.txtSearchQ_TextChanged);
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.resourceLookup1.SetLookup(this.label2, new FWBS.OMS.UI.Windows.ResourceLookupItem("Search:", "Search : ", ""));
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 23);
            this.label2.TabIndex = 0;
            this.label2.Text = "Search : ";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tpStoredProcs
            // 
            this.tpStoredProcs.BackColor = System.Drawing.Color.White;
            this.tpStoredProcs.Controls.Add(this.lstStoredProcs);
            this.tpStoredProcs.Controls.Add(this.panel5);
            this.tpStoredProcs.Location = new System.Drawing.Point(4, 24);
            this.resourceLookup1.SetLookup(this.tpStoredProcs, new FWBS.OMS.UI.Windows.ResourceLookupItem("StoredProcs", "Stored Procedures", ""));
            this.tpStoredProcs.Name = "tpStoredProcs";
            this.tpStoredProcs.Padding = new System.Windows.Forms.Padding(8);
            this.tpStoredProcs.Size = new System.Drawing.Size(308, 273);
            this.tpStoredProcs.TabIndex = 2;
            this.tpStoredProcs.Text = "Stored Procedures";
            this.tpStoredProcs.Visible = false;
            // 
            // lstStoredProcs
            // 
            this.lstStoredProcs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstStoredProcs.Location = new System.Drawing.Point(8, 35);
            this.lstStoredProcs.Name = "lstStoredProcs";
            this.lstStoredProcs.Size = new System.Drawing.Size(292, 230);
            this.lstStoredProcs.TabIndex = 0;
            this.lstStoredProcs.DoubleClick += new System.EventHandler(this.btnOK_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.panel6);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(8, 8);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(292, 27);
            this.panel5.TabIndex = 3;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.txtSearchSP);
            this.panel6.Controls.Add(this.label3);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(292, 23);
            this.panel6.TabIndex = 0;
            // 
            // txtSearchSP
            // 
            this.txtSearchSP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSearchSP.Location = new System.Drawing.Point(48, 0);
            this.txtSearchSP.Name = "txtSearchSP";
            this.txtSearchSP.Size = new System.Drawing.Size(244, 23);
            this.txtSearchSP.TabIndex = 0;
            this.txtSearchSP.TextChanged += new System.EventHandler(this.txtSearchSP_TextChanged);
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Left;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.resourceLookup1.SetLookup(this.label3, new FWBS.OMS.UI.Windows.ResourceLookupItem("Search:", "Search : ", ""));
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 23);
            this.label3.TabIndex = 0;
            this.label3.Text = "Search : ";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnHelp);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.panel1.Location = new System.Drawing.Point(4, 305);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(316, 32);
            this.panel1.TabIndex = 3;
            // 
            // btnHelp
            // 
            this.btnHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHelp.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnHelp.Location = new System.Drawing.Point(239, 4);
            this.resourceLookup1.SetLookup(this.btnHelp, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnHelp", "&Help", ""));
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(75, 24);
            this.btnHelp.TabIndex = 2;
            this.btnHelp.Text = "&Help";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(159, 4);
            this.resourceLookup1.SetLookup(this.btnCancel, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnCancel", "Cance&l", ""));
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 24);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cance&l";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(79, 4);
            this.resourceLookup1.SetLookup(this.btnOK, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnOK", "&OK", ""));
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 24);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "&OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // frmDataDirectory
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(324, 341);
            this.ControlBox = false;
            this.Controls.Add(this.tbcDataType);
            this.Controls.Add(this.panel1);
            this.resourceLookup1.SetLookup(this, new FWBS.OMS.UI.Windows.ResourceLookupItem("DataDirectory", "Data Directory", ""));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDataDirectory";
            this.Padding = new System.Windows.Forms.Padding(4);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Data Directory";
            this.tbcDataType.ResumeLayout(false);
            this.tpTables.ResumeLayout(false);
            this.pnlSearch.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tpQueries.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.tpStoredProcs.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void txtSearchT_TextChanged(object sender, System.EventArgs e)
		{
			if (lstTables.DataSource is System.Data.DataTable)
			{
				DataTable n = (DataTable)lstTables.DataSource;
				n.DefaultView.RowFilter = lstTables.DisplayMember + " Like '" + txtSearchT.Text.Replace("'","''").Replace("[","").Replace("]","") + "%'";
			}

		}

		private void txtSearchQ_TextChanged(object sender, System.EventArgs e)
		{
			if (lstQueries.DataSource is System.Data.DataTable)
			{
				DataTable n = (DataTable)lstQueries.DataSource;
				n.DefaultView.RowFilter = lstQueries.DisplayMember + " Like '" + txtSearchQ.Text.Replace("'","''") + "%'";
			}

		}

		private void txtSearchSP_TextChanged(object sender, System.EventArgs e)
		{
			if (lstStoredProcs.DataSource is System.Data.DataTable)
			{
				DataTable n = (DataTable)lstStoredProcs.DataSource;
				n.DefaultView.RowFilter = lstStoredProcs.DisplayMember + " Like '" + txtSearchSP.Text.Replace("'","''").Replace("[","").Replace("]","") + "%'";
			}
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
		}

	}
}
