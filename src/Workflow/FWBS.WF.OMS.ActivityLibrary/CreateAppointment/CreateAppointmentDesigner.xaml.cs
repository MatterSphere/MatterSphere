#region References
using System;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
	// Interaction logic for CreateAppointmentDesigner.xaml
	internal sealed partial class CreateAppointmentDesigner
	{
		#region Properties
		public string InTooltip { get; set; }
		public string OutTooltip { get; set; }
		#endregion

		#region Constructor
		public CreateAppointmentDesigner()
		{
			InitializeComponent();

			this.Loaded += new System.Windows.RoutedEventHandler(CreateAppointmentDesigner_Loaded);
		}
		#endregion

		#region CreateAppointmentDesigner_Loaded
		void CreateAppointmentDesigner_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			try
			{
				// set tooltips
				this.ToolTip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTCAPPBDTT", "Create a new Appointment Activity", "").Text;
				this.InTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTCAPPINTT", "An optional File as input", "").Text;
				this.OutTooltip = FWBS.OMS.Session.CurrentSession.Resources.GetResource("WFACTCAPPOTTT", "An Appointment output result", "").Text;
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
