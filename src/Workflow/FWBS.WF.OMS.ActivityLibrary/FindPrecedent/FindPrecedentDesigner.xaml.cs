#region References
using System;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
	internal sealed partial class FindPrecedentDesigner
	{
		#region Constructor
		public FindPrecedentDesigner()
		{
			InitializeComponent();

			this.Loaded += new System.Windows.RoutedEventHandler(FindPrecedentDesigner_Loaded);
		}
		#endregion

		#region Properties
		public string InTooltip { get; set; }
		public string InTooltip2 { get; set; }
		public string OutTooltip { get; set; }
		#endregion

		#region FindPrecedentDesigner_Loaded
		void FindPrecedentDesigner_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			try
			{
				// set tooltips
				this.ToolTip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTFIPRBDTT", "Find Precedent Activity", "").Text;
				this.InTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTFIPRINTT", "A Precedent Type as input", "").Text;
				this.InTooltip2 = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTFIPRINTT2", "An Associate as input", "").Text;
				this.OutTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTFIPROTTT", "A Precedent output result", "").Text;
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
