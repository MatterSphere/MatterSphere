#region References
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
    [ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.ProcessStart.ProcessStartToolboxIcon.png")]
	[Description("Start a Process")]
	[Designer(typeof(ProcessStartDesigner))]
	public sealed class ProcessStart : CodeActivity
	{
		#region Constructor
		public ProcessStart()
		{
			this.DisplayName = "Process Start";
		}
		#endregion

		#region Arguments
		// Define an activity input argument of type string
		[RequiredArgument]
		public InArgument<string> ProcessName { get; set; }

		public InArgument<string> ProcessArguments { get; set; }
		#endregion

		#region Overrides
        /// <summary>
        /// CacheMetadata
        /// </summary>
        /// <param name="metadata"></param>
        protected override void CacheMetadata(CodeActivityMetadata metadata)
		{
			RuntimeArgument argument = new RuntimeArgument("ProcessName", typeof(string), ArgumentDirection.In, true);
			metadata.Bind(this.ProcessName, argument);
			metadata.AddArgument(argument);

			argument = new RuntimeArgument("ProcessArguments", typeof(string), ArgumentDirection.In);
			metadata.Bind(this.ProcessArguments, argument);
			metadata.AddArgument(argument);
		}

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="context"></param>
		protected override void Execute(CodeActivityContext context)
		{
			string name = this.ProcessName.Get(context);
			string arguments = this.ProcessArguments.Get(context);

			if (string.IsNullOrEmpty(name))
			{
				string errMsg = string.Format("Activity Name='{0}' Argument='{1}.ProcessName' ID={2}", this.DisplayName, this.GetType().Name, this.Id);
				throw new ArgumentNullException(errMsg);
			}
			else
			{
				FWBS.OMS.UI.Windows.Services.ProcessStart(name, arguments);
			}
		}
		#endregion
	}
}
