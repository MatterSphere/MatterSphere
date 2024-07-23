#region References
using System;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
	public partial class ShowCurrentJobsDesigner
	{
		#region Constructor
		public ShowCurrentJobsDesigner()
		{
			InitializeComponent();

			this.Loaded += new System.Windows.RoutedEventHandler(ShowCurrentJobsDesigner_Loaded);
		}
		#endregion

		#region ShowCurrentJobsDesigner_Loaded
		void ShowCurrentJobsDesigner_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			try
			{
				// set tooltips
				this.ToolTip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTSHWCURBDTT", "Show current jobs", "").Text;
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
