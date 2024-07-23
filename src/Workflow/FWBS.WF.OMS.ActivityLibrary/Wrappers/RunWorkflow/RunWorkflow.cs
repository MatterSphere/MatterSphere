#region References
using System;
using System.Activities;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
	#region LoadAndRunWorkflow class
	//
	// THE ICON IN THE TOOLBOX IS DETERMINED BY ToolboxBitmap ATTRIBUTE
	// THE ICON ON THE DESIGNER IS DETERMINED BY Designer ATTRIBUTE
	//
	[ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.Wrappers.RunWorkflow.RunWorkflowToolboxIcon.png")]
	[Description("Run a Workflow")]
	[Designer(typeof(RunWorkflowDesigner))]
	public sealed class RunWorkflow : Activity<IDictionary<string, object>>
	{
		#region Arguments
		[RequiredArgument]
		public InArgument<Activity> Activity { get; set; }

		public InArgument<IDictionary<string, object>> Input { get; set; }
		#endregion

		#region Constructors and Destructors
		public RunWorkflow()
		{
			this.Implementation = this.CreateImplementation;
			this.DisplayName = "Run Workflow";
		}
		#endregion

		#region Overrides
        /// <summary>
        /// CacheMetadata
        /// </summary>
        /// <param name="metadata"></param>
        protected override void CacheMetadata(ActivityMetadata metadata)
		{
			RuntimeArgument argument = new RuntimeArgument("Activity", typeof(Activity), ArgumentDirection.In, true);
			metadata.Bind(this.Activity, argument);
			metadata.AddArgument(argument);

			argument = new RuntimeArgument("Input", typeof(IDictionary<string, object>), ArgumentDirection.In);
			metadata.Bind(this.Input, argument);
			metadata.AddArgument(argument);
		}
		#endregion

		#region Methods
        /// <summary>
        /// Create Implementation
        /// </summary>
        /// <returns></returns>
		private Activity CreateImplementation()
		{
			return new FWBS.WF.ActivityLibrary.RunWorkflow
				{
					Activity = new InArgument<Activity>(ctx => this.Activity.Get(ctx)),
					Input = new InArgument<IDictionary<string, object>>(ctx => this.Input.Get(ctx)),
					Result = new ArgumentReference<IDictionary<string, object>>("Result"),
				};
		}
		#endregion
	}
	#endregion
}