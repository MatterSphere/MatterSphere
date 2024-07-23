using System;
using System.ComponentModel;

namespace FWBS.OMS.UI.Windows.Admin
{
    /// <summary>
    /// Summary description for frmMultiReport.
    /// </summary>
    public class frmMultiReport : FWBS.OMS.UI.Windows.BaseForm
	{
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private FWBS.OMS.UI.Windows.eCLCollectionSelector eCLCollectionSelector1;
        private System.Windows.Forms.TextBox txtSearch;
        protected ResourceLookup resourceLookup1;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.TableLayoutPanel pnlSearch;
        private IContainer components;

		public frmMultiReport()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
            FWBS.OMS.SearchEngine.SearchList _search = new FWBS.OMS.SearchEngine.SearchList(Session.CurrentSession.DefaultSystemSearchList(FWBS.OMS.SystemSearchLists.ReportPicker),null,new Common.KeyValueCollection());
			eCLCollectionSelector1.ValueMember = "schcode";
			eCLCollectionSelector1.DisplayMember = "schdesc";
			eCLCollectionSelector1.ValueSplit = ",";

			eCLCollectionSelector1.SelectionList = (System.Data.DataTable)_search.Run();
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMultiReport));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.eCLCollectionSelector1 = new FWBS.OMS.UI.Windows.eCLCollectionSelector();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.pnlSearch = new System.Windows.Forms.TableLayoutPanel();
            this.pnlButtons.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(8, 0);
            this.resourceLookup1.SetLookup(this.btnOK, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnOK", "&OK", ""));
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(76, 24);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "&OK";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(8, 31);
            this.resourceLookup1.SetLookup(this.btnCancel, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnCancel", "Cance&l", ""));
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(76, 24);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cance&l";
            // 
            // eCLCollectionSelector1
            // 
            this.eCLCollectionSelector1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.eCLCollectionSelector1.IsDirty = true;
            this.eCLCollectionSelector1.Location = new System.Drawing.Point(8, 38);
            this.eCLCollectionSelector1.Name = "eCLCollectionSelector1";
            this.eCLCollectionSelector1.Size = new System.Drawing.Size(540, 340);
            this.eCLCollectionSelector1.TabIndex = 2;
            this.eCLCollectionSelector1.Value = "";
            // 
            // txtSearch
            // 
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtSearch.Location = new System.Drawing.Point(0, 0);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(246, 22);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // pnlButtons
            // 
            this.pnlButtons.BackColor = System.Drawing.Color.Transparent;
            this.pnlButtons.Controls.Add(this.btnOK);
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlButtons.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.pnlButtons.Location = new System.Drawing.Point(548, 8);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(84, 370);
            this.pnlButtons.TabIndex = 3;
            // 
            // pnlSearch
            // 
            this.pnlSearch.ColumnCount = 3;
            this.pnlSearch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlSearch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.pnlSearch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlSearch.Controls.Add(this.txtSearch, 0, 0);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearch.Location = new System.Drawing.Point(8, 8);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.RowCount = 1;
            this.pnlSearch.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlSearch.Size = new System.Drawing.Size(540, 30);
            this.pnlSearch.TabIndex = 1;
            // 
            // frmMultiReport
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(640, 386);
            this.Controls.Add(this.eCLCollectionSelector1);
            this.Controls.Add(this.pnlSearch);
            this.Controls.Add(this.pnlButtons);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.resourceLookup1.SetLookup(this, new FWBS.OMS.UI.Windows.ResourceLookupItem("frmMultiReport", "Multi Report Selector", ""));
            this.Name = "frmMultiReport";
            this.Padding = new System.Windows.Forms.Padding(8);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Multi Report Selector";
            this.pnlButtons.ResumeLayout(false);
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		private void txtSearch_TextChanged(object sender, System.EventArgs e)
		{
			eCLCollectionSelector1.SelectionList.DefaultView.RowFilter ="schdesc Like '%" + txtSearch.Text.Replace("'","''").Replace("[","").Replace("]","") + "%'";
		}

		public string Value
		{
			get
			{
				string _codes = "";
				eCLCollectionSelector1.SelectedItems.DefaultView.RowStateFilter = System.Data.DataViewRowState.OriginalRows;
				eCLCollectionSelector1.SelectedItems.DefaultView.RowStateFilter = System.Data.DataViewRowState.CurrentRows;
				foreach (System.Data.DataRowView rw in eCLCollectionSelector1.SelectedItems.DefaultView)
				{
					_codes += Convert.ToString(rw["schcode"]).ToUpper() + ",";
				}
				_codes = _codes.Trim(',');
				return _codes;
			}
			set
			{
				eCLCollectionSelector1.Value = value;
			}
		}
	}
}
