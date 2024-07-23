#region References
using System;
using System.Windows;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
	internal sealed partial class GetContextNamedItemDesigner
	{
		#region Constructor
		public GetContextNamedItemDesigner()
		{
			InitializeComponent();

			this.Loaded += new RoutedEventHandler(GetContextNamedItemDesigner_Loaded);
		}
		#endregion

		#region GetContextNamedItemDesigner_Loaded
		void GetContextNamedItemDesigner_Loaded(object sender, RoutedEventArgs e)
		{
			try
			{
				// set tooltips
				this.ToolTip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTGETCONNI", "Create a new Get Context Named Item Activity", "").Text;
				this.InTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTGETCONKE", "Key as input", "").Text;
				this.OutTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTGETCONVA", "Value as output", "").Text;
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
