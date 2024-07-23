#region References
using System;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
	internal sealed partial class RunEnquiryCommandDesigner
	{
		#region Constructor
		public RunEnquiryCommandDesigner()
		{
			InitializeComponent();

			this.Loaded += new System.Windows.RoutedEventHandler(RunEnquiryFormDesigner_Loaded);
		}
		#endregion

		#region Properties
		public string InTooltip { get; set; }
		public string OutTooltip { get; set; }
		#endregion

		#region RunEnquiryFormDesigner_Loaded
		void RunEnquiryFormDesigner_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			try
			{
				// set tooltips
				this.ToolTip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTRECOBDTT", "Run an Enquiry Command Activity", "").Text;
				this.InTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTRECOINTT", "An Enquiry Command code as input", "").Text;
				this.OutTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTRECOOTTT", "An Object output result", "").Text;
			}
			catch (Exception)
			{
				// error
				;
			}
		}
		#endregion
	}
}
