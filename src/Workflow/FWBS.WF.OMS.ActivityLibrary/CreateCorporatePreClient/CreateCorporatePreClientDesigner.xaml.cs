#region References
using System;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
	internal sealed partial class CreateCorporatePreClientDesigner
	{
		#region Constructor
        public CreateCorporatePreClientDesigner()
		{
			InitializeComponent();

            this.Loaded += new System.Windows.RoutedEventHandler(CreateCorporatePreClientDesigner_Loaded);
		}
		#endregion

		#region Properties
		public string OutTooltip { get; set; }
		#endregion

        #region CreateCorporatePreClientDesigner_Loaded
        void CreateCorporatePreClientDesigner_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			try
			{
				// set tooltips
				this.ToolTip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTCCPCLBDTT", "Create a new Corporate Pre-Client Activity", "").Text;
				this.OutTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTCCPCLOTTT", "A boolean indicating if the activity was successful", "").Text;
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
