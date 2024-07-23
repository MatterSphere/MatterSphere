#region References
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
    [ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.SaveAppointment.SaveAppointmentToolboxIcon.png")]
	[Description("Saves an Appointment")]
	[Designer(typeof(SaveAppointmentDesigner))]
	public sealed class SaveAppointment : CodeActivity<bool>
	{
		#region Arguments
		[RequiredArgument]
		public InArgument<FWBS.OMS.Appointment> Appointment { get; set; }
		#endregion

		#region Constructor
		public SaveAppointment()
		{
			this.DisplayName = "Save Appointment";
		}
		#endregion

		#region Overrides
        /// <summary>
        /// CacheMetadata
        /// </summary>
        /// <param name="metadata"></param>
        protected override void CacheMetadata(CodeActivityMetadata metadata)
		{
			RuntimeArgument argument = new RuntimeArgument("Appointment", typeof(FWBS.OMS.Appointment), ArgumentDirection.In, true);
			metadata.Bind(this.Appointment, argument);
			metadata.AddArgument(argument);
		}

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
		protected override bool Execute(CodeActivityContext context)
		{
			FWBS.OMS.Appointment appointment = this.Appointment.Get(context);

			// Validate Arguments
			if (appointment == null)
			{
				string errMsg = string.Format("Activity Name='{0}' Argument='{1}.Appointment' ID={2}", this.DisplayName, this.GetType().Name, this.Id);
				throw new ArgumentNullException(errMsg);
			}
			else
			{
				return FWBS.OMS.UI.Windows.Services.Wizards.SaveAppointment(ref appointment);
			}
		}
		#endregion
	}
}
