#region References
using System;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
	internal sealed partial class FindFileDesigner
	{
		#region Constructor
		public FindFileDesigner()
		{
			InitializeComponent();

			this.Loaded += new System.Windows.RoutedEventHandler(FindFileDesigner_Loaded);
		}
		#endregion

		#region Properties
		public string OutTooltip { get; set; }
		#endregion

		#region FindFileDesigner_Loaded
		void FindFileDesigner_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			try
			{
				// set tooltips
				this.ToolTip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTFIFIBDTT", "Find File Activity", "").Text;
				this.OutTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTFIFIOTTT", "A File output result", "").Text;
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
