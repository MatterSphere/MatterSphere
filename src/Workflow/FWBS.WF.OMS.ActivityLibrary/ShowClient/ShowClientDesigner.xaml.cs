#region References
using System;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
	public partial class ShowClientDesigner
	{
		#region Constructor
		public ShowClientDesigner()
		{
			InitializeComponent();

			this.Loaded += new System.Windows.RoutedEventHandler(ShowClientDesigner_Loaded);
		}
		#endregion

		#region Properties
		public string InTooltip { get; set; }
		#endregion

		#region 
		void ShowClientDesigner_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			try
			{
				// set tooltips
				this.ToolTip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTSHWCLIBDTT", "Show a Client Activity", "").Text;
				this.InTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTSHWCLIINTT", "A required Client as input", "").Text;
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
