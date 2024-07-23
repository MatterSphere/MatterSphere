using System.Windows.Forms;


namespace FWBS.OMS.UI.Windows.Design
{
    public class frmCodeLookupSelector : frmListSelector
	{
		public FWBS.Common.UI.Windows.eInformation ToolTip;
		private System.Windows.Forms.Panel pnlSpace;
		private System.ComponentModel.IContainer components = null;

		public frmCodeLookupSelector() 
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();
			ToolTip.Title = Session.CurrentSession.Resources.GetResource("HELPTIP","Help Tips","").Text;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.ToolTip = new FWBS.Common.UI.Windows.eInformation();
            this.pnlSpace = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // ucSelectionItem1
            // 
            this.ucSelectionItem1.ShowHelp = true;
            this.ucSelectionItem1.Size = new System.Drawing.Size(336, 285);
            // 
            // pnlButtons
            // 
            this.pnlButtons.Location = new System.Drawing.Point(261, 38);
            this.pnlButtons.Size = new System.Drawing.Size(83, 221);
            // 
            // txtSearch
            // 
            this.txtSearch.Size = new System.Drawing.Size(288, 23);
            // 
            // ToolTip
            // 
            this.ToolTip.BackColor = System.Drawing.Color.White;
            this.ToolTip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ToolTip.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ToolTip.Location = new System.Drawing.Point(0, 267);
            this.resourceLookup1.SetLookup(this.ToolTip, new FWBS.OMS.UI.Windows.ResourceLookupItem("CODECANTOC", "If the text you require does not exist then click Cancel for an option to create." +
            "", ""));
            this.ToolTip.Name = "ToolTip";
            this.ToolTip.Padding = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.ToolTip.Size = new System.Drawing.Size(344, 64);
            this.ToolTip.TabIndex = 6;
            this.ToolTip.Text = "One Line Information Line";
            this.ToolTip.Title = "Title Bar";
            this.ToolTip.VisibleChanged += new System.EventHandler(this.ToolTip_VisibleChanged);
            // 
            // pnlSpace
            // 
            this.pnlSpace.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSpace.Location = new System.Drawing.Point(0, 259);
            this.pnlSpace.Name = "pnlSpace";
            this.pnlSpace.Size = new System.Drawing.Size(344, 8);
            this.pnlSpace.TabIndex = 5;
            // 
            // frmCodeLookupSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(344, 331);
            this.Controls.Add(this.pnlSpace);
            this.Controls.Add(this.ToolTip);
            this.Name = "frmCodeLookupSelector";
            this.ShowHelp = true;
            this.Text = "Code Lookup Selector";
            this.Controls.SetChildIndex(this.ToolTip, 0);
            this.Controls.SetChildIndex(this.pnlSpace, 0);
            this.Controls.SetChildIndex(this.pnlButtons, 0);
            this.Controls.SetChildIndex(this.pnlSelection, 0);
            this.ResumeLayout(false);

		}
		#endregion

		public void LoadCodeTypes()
		{
			List.SuspendLayout();
			List.DataSource = FWBS.OMS.CodeLookup.GetLookups(base.CodeType);
            List.ValueMember = "cdcode";
            List.DisplayMember = "cddesc";			
			List.ResumeLayout();
		}

		private void ucSelectionItem1_DoubleClick(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
		}

		private void ToolTip_VisibleChanged(object sender, System.EventArgs e)
		{
			pnlSpace.Visible = ToolTip.Visible;
		}

	}
}

