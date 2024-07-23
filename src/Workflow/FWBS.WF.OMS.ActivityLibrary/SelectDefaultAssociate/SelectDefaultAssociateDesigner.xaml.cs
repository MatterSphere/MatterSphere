#region References
using System;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
	internal sealed partial class SelectDefaultAssociateDesigner
	{
		#region Constructor
		public SelectDefaultAssociateDesigner()
		{
			InitializeComponent();

			this.Loaded += new System.Windows.RoutedEventHandler(SelectDefaultAssociateDesigner_Loaded);
		}
		#endregion

		#region Properties
		public string OutTooltip { get; set; }
		#endregion

		#region SelectDefaultAssociateDesigner_Loaded
		void SelectDefaultAssociateDesigner_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			try
			{
				// set tooltips
				this.ToolTip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTSDASSBDTT", "Select the Default Associate Activity", "").Text;
				this.OutTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTSDASSOTTT", "An Associate output result", "").Text;
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
