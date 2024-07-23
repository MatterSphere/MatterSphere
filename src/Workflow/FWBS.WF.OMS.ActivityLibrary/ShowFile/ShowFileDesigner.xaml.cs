#region References
using System;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
	public partial class ShowFileDesigner
	{
		#region Constructor
		public ShowFileDesigner()
		{
			InitializeComponent();

			this.Loaded += new System.Windows.RoutedEventHandler(ShowFileDesigner_Loaded);
		}
		#endregion

		#region Properties
		public string InTooltip { get; set; }
		#endregion

		#region ShowFileDesigner_Loaded
		void ShowFileDesigner_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			try
			{
				// set tooltips
				this.ToolTip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTSHWFILBDTT", "Show a File", "").Text;
				this.InTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTSHWFILINTT", "A required File as input", "").Text;
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
