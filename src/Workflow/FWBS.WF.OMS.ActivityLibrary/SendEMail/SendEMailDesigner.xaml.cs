#region References
using System;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
	internal sealed partial class SendEMailDesigner
	{
		#region Constructor
		public SendEMailDesigner()
		{
			InitializeComponent();

			this.Loaded += new System.Windows.RoutedEventHandler(SendEMailDesigner_Loaded);
		}
		#endregion

		#region Properties
		public string InTooltip { get; set; }
		public string InTooltip2 { get; set; }
		public string InTooltip3 { get; set; }
		public string InTooltip4 { get; set; }
		#endregion

		#region SendEMailDesigner_Loaded
		void SendEMailDesigner_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			try
			{
				// set tooltips
				this.ToolTip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTSNDEMBDTT", "Send an Email", "").Text;
				this.InTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTSNDEMNINTT", "Sending email address", "").Text;
				this.InTooltip2 = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTSNDEMINTT2", "Recipient email address", "").Text;
				this.InTooltip3 = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTSNDEMINTT3", "Subject of email", "").Text;
				this.InTooltip4 = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTSNDEMINTT4", "Body of email", "").Text;
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
