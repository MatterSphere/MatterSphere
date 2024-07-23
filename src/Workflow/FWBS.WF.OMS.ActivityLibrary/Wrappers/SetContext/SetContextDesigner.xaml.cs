#region References
using System;
using System.Windows;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
	internal sealed partial class SetContextDesigner
	{
		#region Constructor
		public SetContextDesigner()
		{
			InitializeComponent();

			this.Loaded += new RoutedEventHandler(SetContextDesigner_Loaded);
		}
		#endregion

		#region SetContextDesigner_Loaded
		void SetContextDesigner_Loaded(object sender, RoutedEventArgs e)
		{
			try
			{
				// set tooltips
				this.ToolTip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTSETCONTT", "Create a new Set Context Activity", "").Text;
				this.InTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTSETCONKE", "Object as input", "").Text;
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
		#endregion
	}
}
