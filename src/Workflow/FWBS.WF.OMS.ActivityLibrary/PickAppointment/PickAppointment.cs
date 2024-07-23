#region References
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
    [ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.PickAppointment.PickAppointmentToolboxIcon.png")]
	[Description("Pick an Appointment")]
	[Designer(typeof(PickAppointmentDesigner))]
	public sealed class PickAppointment : CodeActivity<FWBS.OMS.Appointment>
	{
		#region Arguments
		[RequiredArgument]
		public InArgument<FWBS.OMS.OMSFile> File { get; set; }
		#endregion

		#region Constructor
		public PickAppointment()
		{
			this.DisplayName = "Pick Appointment";
		}
		#endregion

		#region Overrides
        /// <summary>
        /// CacheMetadata
        /// </summary>
        /// <param name="metadata"></param>
        protected override void CacheMetadata(CodeActivityMetadata metadata)
		{
			RuntimeArgument argument = new RuntimeArgument("File", typeof(FWBS.OMS.OMSFile), ArgumentDirection.In, true);
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

			// Validate Arguments
			if (file == null)
			{
				string errMsg = string.Format("Activity Name='{0}' Argument='{1}.File' ID={2}", this.DisplayName, this.GetType().Name, this.Id);
				throw new ArgumentNullException(errMsg);
			}
			else
			{
				return FWBS.OMS.UI.Windows.Services.Searches.PickAppointment(file);
			}
		}
		#endregion
	}
}
