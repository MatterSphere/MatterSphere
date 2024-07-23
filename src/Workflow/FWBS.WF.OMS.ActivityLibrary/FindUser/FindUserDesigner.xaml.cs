#region References
using System;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
    internal sealed partial class FindUserDesigner
	{
		#region Constructor
		public FindUserDesigner()
        {
            InitializeComponent();

			this.Loaded += new System.Windows.RoutedEventHandler(FindUserDesigner_Loaded);
        }
		#endregion

		#region Properties
		public string OutTooltip { get; set; }
		#endregion

		#region FindUserDesigner_Loaded
		void FindUserDesigner_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			try
			{
				// set tooltips
				this.ToolTip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTFIUSBDTT", "Find a User Activity", "").Text;
				this.OutTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTFIUSOTTT", "A User output result", "").Text;
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
