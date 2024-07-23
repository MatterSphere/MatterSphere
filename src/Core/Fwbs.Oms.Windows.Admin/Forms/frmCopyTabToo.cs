using System;
using System.ComponentModel;

namespace FWBS.OMS.UI.Windows.Admin
{
    /// <summary>
    /// Summary description for frmCopyTabToo.
    /// </summary>
    public class frmCopyTabToo : FWBS.OMS.UI.Windows.BaseForm
	{
		public FWBS.OMS.UI.Windows.eCLCollectionSelector eCLCollectionSelector1;
		private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        protected ResourceLookup resourceLookup1;
        private System.Windows.Forms.Panel pnlButtons;
        private IContainer components;

		public frmCopyTabToo(string SearchListCode, string currentcode)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			FWBS.OMS.SearchEngine.SearchList _search = new FWBS.OMS.SearchEngine.SearchList(SearchListCode,null,new Common.KeyValueCollection());
			eCLCollectionSelector1.ValueMember = "typeCode";
			eCLCollectionSelector1.DisplayMember = "typeCombinded";
			eCLCollectionSelector1.ValueSplit = ",";
			System.Data.DataTable _data = (System.Data.DataTable)_search.Run();
			_data.DefaultView.RowFilter = "typeActive = true AND typecode <> '" + currentcode + "'";
			eCLCollectionSelector1.SelectionList = _data;
            eCLCollectionSelector1.ActiveChanged -= ECLCollectionSelector1_ActiveChanged;
            eCLCollectionSelector1.ActiveChanged += ECLCollectionSelector1_ActiveChanged;
		}

        private void ECLCollectionSelector1_ActiveChanged(object sender, EventArgs e)
        {
            this.btnOK.Enabled = (eCLCollectionSelector1.SelectedItems != null && eCLCollectionSelector1.SelectedItems.Rows.Count > 0);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCopyTabToo));
            this.eCLCollectionSelector1 = new FWBS.OMS.UI.Windows.eCLCollectionSelector();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // eCLCollectionSelector1
            // 
            this.eCLCollectionSelector1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.eCLCollectionSelector1.IsDirty = true;
            this.eCLCollectionSelector1.Location = new System.Drawing.Point(8, 8);
            this.eCLCollectionSelector1.Margin = new System.Windows.Forms.Padding(2);
            this.eCLCollectionSelector1.Name = "eCLCollectionSelector1";
            this.eCLCollectionSelector1.Size = new System.Drawing.Size(384, 245);
            this.eCLCollectionSelector1.TabIndex = 1;
            this.eCLCollectionSelector1.Value = "";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(8, 30);
            this.resourceLookup1.SetLookup(this.btnCancel, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnCancel", "Cance&l", ""));
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(76, 24);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cance&l";
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Enabled = false;
            this.btnOK.Location = new System.Drawing.Point(8, 0);
            this.resourceLookup1.SetLookup(this.btnOK, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnOK", "&OK", ""));
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(76, 24);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "&OK";
            // 
            // pnlButtons
            // 
            this.pnlButtons.BackColor = System.Drawing.Color.Transparent;
            this.pnlButtons.Controls.Add(this.btnOK);
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlButtons.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlButtons.Location = new System.Drawing.Point(392, 8);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(84, 245);
            this.pnlButtons.TabIndex = 2;
            // 
            // frmCopyTabToo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(484, 261);
            this.Controls.Add(this.eCLCollectionSelector1);
            this.Controls.Add(this.pnlButtons);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.resourceLookup1.SetLookup(this, new FWBS.OMS.UI.Windows.ResourceLookupItem("SelectTypes", "Select Types", ""));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCopyTabToo";
            this.Padding = new System.Windows.Forms.Padding(8);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Types";
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion
	}
}
