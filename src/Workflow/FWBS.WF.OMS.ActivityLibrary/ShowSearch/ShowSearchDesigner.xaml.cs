#region References
using System;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
	internal sealed partial class ShowSearchDesigner
	{
		#region Constructor
		public ShowSearchDesigner()
		{
			InitializeComponent();

			this.Loaded += new System.Windows.RoutedEventHandler(ShowSearchDesigner_Loaded);
		}
		#endregion

		#region Properties
        public string InTooltip { get; set; }
        public string InTooltip2 { get; set; }
		public string OutTooltip { get; set; }
		#endregion

		#region 
		void ShowSearchDesigner_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			try
			{
				// set tooltips
				this.ToolTip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTSHWSRHBDTT", "Show Search", "").Text;
                this.InTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTSHWSRHINTT", "The searchlist code as input", "").Text;
                this.InTooltip2 = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTSHWSRHINT2", "The search kind that the code represents", "").Text;
				this.OutTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTSHWSRHTTT", "A KeyValueCollection output result", "").Text;
			}
			catch (Exception)
			{
				// error
				;
			}			
		}
		#endregion

        private void cmbSearchKind_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
        }
	}
}
