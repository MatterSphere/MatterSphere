#region References
using System;
using System.Windows;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
	internal sealed partial class GetContextDesigner
	{
		#region Constructor
		public GetContextDesigner()
		{
			InitializeComponent();

			this.Loaded += new RoutedEventHandler(GetContextDesigner_Loaded);
		}
		#endregion

		#region GetContextDesigner_Loaded
		void GetContextDesigner_Loaded(object sender, RoutedEventArgs e)
		{
			try
			{
				// set tooltips
				this.ToolTip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTGETCONTT", "Create a new Get Context Named Item Activity", "").Text;
				this.InTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTGETCONTY", "Object type required as input", "").Text;
				this.OutTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTGETCONOU", "Object as output", "").Text;
			}
			catch (Exception)
			{
				// error
				;
			}
		}
		#endregion

		#region Properties
		public string InTooltip { get; set; }
		public string OutTooltip { get; set; }
		#endregion
	}
}
