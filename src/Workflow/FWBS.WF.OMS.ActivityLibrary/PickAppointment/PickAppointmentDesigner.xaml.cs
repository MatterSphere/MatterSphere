#region References
using System;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
    internal sealed partial class PickAppointmentDesigner
	{
		#region Constructor
		public PickAppointmentDesigner()
        {
            InitializeComponent();

			this.Loaded += new System.Windows.RoutedEventHandler(PickAppointmentDesigner_Loaded);
		}
		#endregion

		#region Properties
		public string InTooltip { get; set; }
		public string OutTooltip { get; set; }
		#endregion

		#region PickAppointmentDesigner_Loaded
		void PickAppointmentDesigner_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			try
			{
				// set tooltips
				this.ToolTip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTPIAPBDTT", "Pick an Appointment Activity", "").Text;
				this.InTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTPIAPINTT", "An optional File as input", "").Text;
				this.OutTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTPIAPOTTT", "An Appointment output result", "").Text;
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
