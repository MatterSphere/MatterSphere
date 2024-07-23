namespace FWBS.Common.UI.Windows
{
    /// <summary>
    /// A group box enquiry control which generally holds a other controls.
    /// </summary>
    public class eGroupBox : System.Windows.Forms.GroupBox, IContainerEnquiryControl
	{
		public eGroupBox()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

		}


		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// eGroupBox
			// 
			this.FlatStyle = System.Windows.Forms.FlatStyle.System;

		}
		#endregion
	}
}
