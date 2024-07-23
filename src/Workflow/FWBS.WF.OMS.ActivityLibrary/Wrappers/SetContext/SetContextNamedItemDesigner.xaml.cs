#region References
using System;
using System.Windows;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
	internal sealed partial class SetContextNamedItemDesigner
	{
		#region Constructor
		public SetContextNamedItemDesigner()
		{
			InitializeComponent();

			this.Loaded += new RoutedEventHandler(SetContextNamedItemDesigner_Loaded);
		}
		#endregion

		#region SetContextNamedItemDesigner_Loaded
		void SetContextNamedItemDesigner_Loaded(object sender, RoutedEventArgs e)
		{
			try
			{
				// set tooltips
				this.ToolTip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTSETCONNI", "Create a new Set Context Named Item Activity", "").Text;
				this.InTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTSETCONKE", "Key as input", "").Text;
				this.InTooltip2 = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTSETCONVA", "Value as input", "").Text;
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
		public string InTooltip2 { get; set; }
		#endregion
	}
}
