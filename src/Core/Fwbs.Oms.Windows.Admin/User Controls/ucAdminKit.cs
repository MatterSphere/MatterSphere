namespace FWBS.OMS.UI.Windows.Admin.User_Controls
{
    /// <summary>
    /// Summary description for ucAdminKit.
    /// </summary>
    public class ucAdminKit : System.Windows.Forms.UserControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ucAdminKit()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitForm call

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
            this.SuspendLayout();
            // 
            // ucAdminKit
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Name = "ucAdminKit";
            this.ResumeLayout(false);

		}
		#endregion
	}
}
