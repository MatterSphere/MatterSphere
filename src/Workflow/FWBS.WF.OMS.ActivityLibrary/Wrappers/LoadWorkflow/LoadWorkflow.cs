#region References
using System.Activities;
using System.Activities.Expressions;
using System.ComponentModel;
using System.Drawing;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
    #region LoadWorkflow class
    //
    // THE ICON IN THE TOOLBOX IS DETERMINED BY ToolboxBitmap ATTRIBUTE
    // THE ICON ON THE DESIGNER IS DETERMINED BY Designer ATTRIBUTE
    //
    [ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.Wrappers.LoadWorkflow.LoadWorkflowToolboxIcon.png")]
	[Description("Load a Workflow")]
	[Designer(typeof(LoadWorkflowDesigner))]
	public sealed class LoadWorkflow : Activity<Activity>
	{
		#region Arguments
		[RequiredArgument]
		public InArgument<string> WorkflowCode { get; set; }
		#endregion

		#region Constructors and Destructors
		public LoadWorkflow()
		{
			this.Implementation = this.CreateImplementation;
			this.DisplayName = "Load Workflow";
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
		}
		#endregion

		#region Methods
        /// <summary>
        /// Create Implementation
        /// </summary>
        /// <returns></returns>
		private Activity CreateImplementation()
		{
			return new FWBS.WF.ActivityLibrary.LoadWorkflow
				{
					WorkflowCode = new InArgument<string>(ctx => this.WorkflowCode.Get(ctx)),
					Result = new ArgumentReference<Activity>("Result"),
				};
		}
		#endregion
	}
	#endregion
}