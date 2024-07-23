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
	[ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.Wrappers.LoadAndRunWorkflow.LoadAndRunWorkflowToolboxIcon.png")]
	[Description("Load and Run a Workflow")]
	[Designer(typeof(LoadAndRunWorkflowDesigner))]
	public sealed class LoadAndRunWorkflow : Activity<IDictionary<string, object>>
	{
		#region Arguments
		[RequiredArgument]
		public InArgument<string> WorkflowCode { get; set; }

		public InArgument<IDictionary<string, object>> Input { get; set; }
		#endregion

		#region Constructors and Destructors
		public LoadAndRunWorkflow()
		{
			this.Implementation = this.CreateImplementation;
			this.DisplayName = "Load and Run Workflow";
		}
		#endregion

		#region Overrides
        /// <summary>
        /// CacheMetadata
        /// </summary>
        /// <param name="metadata"></param>
        protected override void CacheMetadata(ActivityMetadata metadata)
		{
			RuntimeArgument argument = new RuntimeArgument("WorkflowCode", typeof(string), ArgumentDirection.In, true);
			metadata.Bind(this.WorkflowCode, argument);
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
			return new FWBS.WF.ActivityLibrary.LoadAndRunWorkflow
				{
					WorkflowCode = new InArgument<string>(ctx => this.WorkflowCode.Get(ctx)),
					Input = new InArgument<IDictionary<string, object>>(ctx => this.Input.Get(ctx)),
					Result = new ArgumentReference<IDictionary<string, object>>("Result"),
				};
		}
		#endregion
	}
	#endregion
}