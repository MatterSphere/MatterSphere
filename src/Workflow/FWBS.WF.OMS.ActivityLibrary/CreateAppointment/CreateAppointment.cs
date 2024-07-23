#region References
using System.Activities;
using System.ComponentModel;
using System.Drawing;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
    [ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.CreateAppointment.CreateAppointmentToolboxIcon.png")]
	[Description("Creates a new Appointment")]
	[Designer(typeof(CreateAppointmentDesigner))]
	public sealed class CreateAppointment : CodeActivity<FWBS.OMS.Appointment>
	{
		#region Constructor
		public CreateAppointment()
		{
			this.DisplayName = "Create Appointment";
		}
		#endregion

		#region Arguments
		public InArgument<FWBS.OMS.OMSFile> File { get; set; }
		#endregion

		#region Overrides
		/// <summary>
		/// CacheMetadata
		/// </summary>
		/// <param name="metadata"></param>
		protected override void CacheMetadata(CodeActivityMetadata metadata)
		{
			RuntimeArgument argument = new RuntimeArgument("File", typeof(FWBS.OMS.OMSFile), ArgumentDirection.In);
			metadata.Bind(this.File, argument);
			metadata.AddArgument(argument);
		}

		/// <summary>
		/// Execute
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		protected override FWBS.OMS.Appointment Execute(CodeActivityContext context)
		{
			FWBS.OMS.OMSFile file = this.File.Get(context);

			if (file == null)
			{
				return FWBS.OMS.UI.Windows.Services.Wizards.CreateAppointment();
			}
			else
			{
				return FWBS.OMS.UI.Windows.Services.Wizards.CreateAppointment(file);
			}
		}
		#endregion
	}
}
