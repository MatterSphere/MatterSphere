namespace FWBS.OMS.UI.Windows.Admin
{
    /// <summary>
    /// Summary description for ucCodeLookups2.
    /// </summary>
    public class ucCodeLookups2 : FWBS.OMS.UI.Windows.Admin.ucEditBase2
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ucCodeLookups2()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		public ucCodeLookups2(frmMain mainparent, frmAdminDesktop editparent, FWBS.Common.KeyValueCollection parent) : base(mainparent,editparent,parent)
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();
		}

		protected override string SearchListName
		{
			get
			{
				return "ADMDCODELOOKUPS2";
			}
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.tpList.SuspendLayout();
            this.tpEdit.SuspendLayout();
            this.pnlEdit.SuspendLayout();
            this.SuspendLayout();
            // 
            // tpEdit
            // 
            this.tpEdit.BackColor = System.Drawing.Color.White;
            this.BresourceLookup1.SetLookup(this.tpEdit, new FWBS.OMS.UI.Windows.ResourceLookupItem("Edit", "Edit", ""));
            // 
            // tbSave
            // 
            this.BresourceLookup1.SetLookup(this.tbSave, new FWBS.OMS.UI.Windows.ResourceLookupItem("Save", "Save", ""));
            // 
            // tbReturn
            // 
            this.BresourceLookup1.SetLookup(this.tbReturn, new FWBS.OMS.UI.Windows.ResourceLookupItem("Return", "Return", ""));
            // 
            // ucCodeLookups2
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Name = "ucCodeLookups2";
            this.tpList.ResumeLayout(false);
            this.tpEdit.ResumeLayout(false);
            this.pnlEdit.ResumeLayout(false);
            this.pnlEdit.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion
	}
}
