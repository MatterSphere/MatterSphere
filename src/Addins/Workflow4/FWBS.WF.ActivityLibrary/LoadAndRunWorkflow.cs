#region References
using System.Activities;
using System.Activities.Expressions;
using System.Activities.Statements;
using System.Collections.Generic;
#endregion

namespace FWBS.WF.ActivityLibrary
{
	#region LoadAndRunWorkflow class
	public sealed class LoadAndRunWorkflow : Activity<IDictionary<string, object>>
	{
		#region Constructors and Destructors
		public LoadAndRunWorkflow()
		{
			this.Implementation = this.CreateImplementation;
		}
		#endregion

		#region Arguments
		[RequiredArgument]
		public InArgument<string> WorkflowCode { get; set; }
		public InArgument<IDictionary<string, object>> Input { get; set; }
		#endregion

		#region Overrides
		// Override CacheMetadata() to stop the default reflection way of discovering the arguments
		protected override void CacheMetadata(ActivityMetadata metadata)
		{
			System.Collections.ObjectModel.Collection<RuntimeArgument> arguments = new System.Collections.ObjectModel.Collection<RuntimeArgument>();

			RuntimeArgument argument = new RuntimeArgument("WorkflowCode", typeof(string), ArgumentDirection.In, true);
			metadata.Bind(this.WorkflowCode, argument);
			arguments.Add(argument);

			argument = new RuntimeArgument("Input", typeof(IDictionary<string, object>), ArgumentDirection.In);
			metadata.Bind(this.Input, argument);
			arguments.Add(argument);

			metadata.SetArgumentsCollection(arguments);
		}
		#endregion

		#region Methods
		private Activity CreateImplementation()
		{
			var activity = new Variable<Activity>();
			return new Sequence
			{
				Variables = 
					{
						activity
					},
				Activities =
					{
						new LoadWorkflow
						{
							WorkflowCode = new InArgument<string>(ctx => this.WorkflowCode.Get(ctx)), 
							Result = new VariableReference<Activity>(activity)
						}, 
						new RunWorkflow
						{
							Activity = new InArgument<Activity>(activity), 
							Input = new InArgument<IDictionary<string, object>>(ctx => this.Input.Get(ctx)), 
							Result = new ArgumentReference<IDictionary<string, object>>("Result"), 
						}
					}
			};
		}
		#endregion
	}
	#endregion
}