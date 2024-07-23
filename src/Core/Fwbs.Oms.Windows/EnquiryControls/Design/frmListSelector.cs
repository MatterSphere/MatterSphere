using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows.Design
{
    /// <summary>
    /// Summary description for frmListSelector.
    /// </summary>
    public class frmListSelector : BaseForm
	{
		protected FWBS.OMS.UI.Windows.Design.ucSelectionItem ucSelectionItem1;
		private System.Windows.Forms.Panel pnlSearch;
		private System.Windows.Forms.Panel panel1;
		protected System.Windows.Forms.Panel pnlButtons;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label label1;
		public System.Windows.Forms.TextBox txtSearch;
		protected FWBS.OMS.UI.Windows.ResourceLookup resourceLookup1;
		private System.ComponentModel.IContainer components;
        protected System.Windows.Forms.Panel pnlSelection;
        private ListBox orginal;

		public frmListSelector()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

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
            this.ucSelectionItem1 = new FWBS.OMS.UI.Windows.Design.ucSelectionItem();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.pnlSelection = new System.Windows.Forms.Panel();
            this.pnlSearch.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.pnlSelection.SuspendLayout();
            this.SuspendLayout();
            // 
            // ucSelectionItem1
            // 
            this.ucSelectionItem1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucSelectionItem1.Heading = "#Select from list below.";
            this.ucSelectionItem1.Location = new System.Drawing.Point(8, 0);
            this.ucSelectionItem1.Name = "ucSelectionItem1";
            this.ucSelectionItem1.ShowHelp = true;
            this.ucSelectionItem1.Size = new System.Drawing.Size(245, 284);
            this.ucSelectionItem1.TabIndex = 1;
            this.ucSelectionItem1.ListDoubleClick += new System.EventHandler(this.ucSelectionItem1_ListDoubleClick);
            // 
            // pnlSearch
            // 
            this.pnlSearch.Controls.Add(this.panel1);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearch.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Padding = new System.Windows.Forms.Padding(8);
            this.pnlSearch.Size = new System.Drawing.Size(261, 39);
            this.pnlSearch.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtSearch);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(8, 8);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(245, 23);
            this.panel1.TabIndex = 0;
            this.panel1.TabStop = true;
            // 
            // txtSearch
            // 
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtSearch.Location = new System.Drawing.Point(48, 0);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(197, 23);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.resourceLookup1.SetLookup(this.label1, new FWBS.OMS.UI.Windows.ResourceLookupItem("Search", "Search : ", ""));
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Search : ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlButtons
            // 
            this.pnlButtons.BackColor = System.Drawing.Color.Transparent;
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Controls.Add(this.btnOK);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlButtons.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlButtons.Location = new System.Drawing.Point(261, 0);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(83, 331);
            this.pnlButtons.TabIndex = 3;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(0, 39);
            this.resourceLookup1.SetLookup(this.btnCancel, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnCancel", "Cance&l", ""));
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "#Cance&l";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(0, 8);
            this.resourceLookup1.SetLookup(this.btnOK, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnOK", "&OK", ""));
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 25);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "#&OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // pnlSelection
            // 
            this.pnlSelection.Controls.Add(this.ucSelectionItem1);
            this.pnlSelection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSelection.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlSelection.Location = new System.Drawing.Point(0, 39);
            this.pnlSelection.Name = "pnlSelection";
            this.pnlSelection.Padding = new System.Windows.Forms.Padding(8, 0, 8, 8);
            this.pnlSelection.Size = new System.Drawing.Size(261, 292);
            this.pnlSelection.TabIndex = 4;
            // 
            // frmListSelector
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(344, 331);
            this.ControlBox = false;
            this.Controls.Add(this.pnlSelection);
            this.Controls.Add(this.pnlSearch);
            this.Controls.Add(this.pnlButtons);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmListSelector";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "List Selector";
            this.Load += new System.EventHandler(this.frmListSelector_Load);
            this.pnlSearch.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.pnlSelection.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}

		private void txtSearch_TextChanged(object sender, System.EventArgs e)
		{
			if (!DesignMode)
			{
                if (ucSelectionItem1.List.DataSource is System.Data.DataTable)
                {
                    int oldSelIndex = ucSelectionItem1.List.SelectedIndex;
                    DataTable n = (DataTable)ucSelectionItem1.List.DataSource;
                    n.DefaultView.RowFilter = string.Format("{0} Like '{1}%'", ucSelectionItem1.List.DisplayMember, txtSearch.Text.Replace("'", "''").Replace("[", "").Replace("]", "").Replace("%", "[%]"));
                    if (oldSelIndex == -1 && ucSelectionItem1.List.SelectedIndex == 0)
                        ucSelectionItem1.List.SetSelected(0, true); // Workaround: SelectedIndexChanged event is not fired automatically when the index has changed from -1 to 0.
                }
                else
                {
                    if (orginal == null)
                    {
                        orginal = new ListBox();
                        orginal.Items.AddRange(ucSelectionItem1.List.Items);
                    }

                    ucSelectionItem1.List.Items.Clear();
                    foreach (var v in orginal.Items)
                    {
                        if (Convert.ToString(v).ToLower().Contains(txtSearch.Text.ToLower()))
                        {
                            ucSelectionItem1.List.Items.Add(v);
                        }
                    }
                }
			}
		}

		private void frmListSelector_Load(object sender, System.EventArgs e)
		{
			if (!DesignMode)
				ucSelectionItem1.Heading = ResourceLookup.GetLookupText("SelectLBW");
			txtSearch.Focus();
		}

		private void txtSearch_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (!DesignMode)
			{
				if (e.KeyValue == 219 || e.KeyValue == 221) {e.Handled=true;return;}
				if (e.KeyCode == Keys.Up)
				{
					if (ucSelectionItem1.List.SelectedIndex > 0) 
					{
						ucSelectionItem1.List.SelectedIndex--;
					}
					e.Handled=true;
				}
				if (e.KeyCode == Keys.Down)
				{
					try
					{
						ucSelectionItem1.List.SelectedIndex++;
					}
					catch{}
					e.Handled=true;
				}
				if (e.KeyCode == Keys.Return)
					btnOK_Click(sender,EventArgs.Empty);
			}
		}

		private void ucSelectionItem1_ListDoubleClick(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
		}

		[Category("Main")]
		[Description("Gets the List Control")]
		public System.Windows.Forms.ListBox List
		{
			get
			{
				return ucSelectionItem1.List;
			}
		}

		[Category("Main")]
		[DefaultValue("")]
		[Description("Code Lookup Type Code")]
		public string CodeType
		{
			get
			{
				return ucSelectionItem1.CodeType;
			}
			set
			{
				if (!DesignMode)
					if (value != null)
					ucSelectionItem1.CodeType = value;
			}
		}

		[Category("Main")]
		[DefaultValue("")]
		[Description("Show Help")]
		public bool ShowHelp
		{
			get
			{
				return ucSelectionItem1.ShowHelp;
			}
			set
			{
				ucSelectionItem1.ShowHelp = value;
			}
		}
	}
}
