#region References
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
#endregion

namespace FWBS.OMS.Workflow
{
    [Designer(typeof(WorkflowDesignActivityDesigner))]
	public sealed class WorkflowDesignActivity : NativeActivity
	{
		#region Fields
		//
		// IMPORTANT NOTE:
		//	If you change the names of these properties then you MUST also change the 
		//	names in its activity designer since the property names are hard coded in order to
		//	extract them from the properties collection!
		//
		[Browsable(false)]
		public Dictionary<string, Argument> Arguments { get; set; }		// Arguments to the workflow
		[Browsable(false)]
		public string Code { get; set; }								//  Workflow Code

		// Used to invoke child activity
		private ActivityFunc<Dictionary<string, object>, object> Body { get; set; }
		private CompletionCallback<object> onChildComplete = null;
		#endregion

		#region Constructor
		public WorkflowDesignActivity()
		{
			// NOTE: We need to give this a value so that when an existing workflow is being loaded it does not bomb out as the 
			//	Microsoft Xaml reader expects a value - so don't leave it as NULL! I think the issue stems from the fact that
			//	the Dictionary object is not serializable and the workflow xaml engine gets around it in this fashion (TK)
			//	If this is a brand new one then 'Arguments' property will be set by the activity designer before 'CacheMetadata' gets called
			//	after parsing the workflow xaml ...
			this.Arguments = new Dictionary<string, Argument>();
		}
		#endregion

		#region Overrides
		protected override void CacheMetadata(NativeActivityMetadata metadata)
		{
			// Arguments should have been set by now:
			//	if new then the activity designer would have set it
			//	if loaded then the workflow xaml loader would have set it
			foreach (KeyValuePair<string, Argument> kvp in Arguments)
			{
				RuntimeArgument runTimeArg = new RuntimeArgument(kvp.Key, kvp.Value.ArgumentType, kvp.Value.Direction, false);
				metadata.Bind(kvp.Value, runTimeArg);
				metadata.AddArgument(runTimeArg);
			}

			// create the child activity - set various things up
			var arg = new DelegateInArgument<Dictionary<string, object>>();		// delegate to argument i.e. the child will grab it of the parent at execution time, not possible without a delegate
			Body = new ActivityFunc<Dictionary<string, object>, object>
			{
				Argument = arg,
				Handler = new FWBS.WF.ActivityLibrary.LoadAndRunWorkflow()
				{
					WorkflowCode = this.Code,		// we can do this here since the value of Code is known at this point in time
					Input = arg,
				},
			};
			metadata.AddImplementationDelegate(Body);	// add as body of this activity
		}

		protected override void Execute(NativeActivityContext context)
		{
			// get the current values and create the dictionary
			Dictionary<string, object> inArgs = new Dictionary<string, object>();
			foreach (KeyValuePair<string, Argument> kvp in this.Arguments)
			{
				if (kvp.Value.Direction != ArgumentDirection.Out)
				{
					object obj = context.GetValue(kvp.Value);
					inArgs.Add(kvp.Key, obj);
				}
			}

			// invoke the body with the argument
			if (this.onChildComplete == null)
			{
				this.onChildComplete = new CompletionCallback<object>(this.AssignOutArguments);
			}
			context.ScheduleFunc<Dictionary<string, object>, object>(Body, inArgs, this.onChildComplete);
		}

		private void AssignOutArguments(NativeActivityContext context, ActivityInstance instance, object result)
		{
			IDictionary<string, object> outArgs = (result == null ? null : result as IDictionary<string, object>);

			if (outArgs != null)
			{
				foreach (KeyValuePair<string, Argument> kvp in this.Arguments)
				{
					if (kvp.Value.Direction != ArgumentDirection.In)
					{
						object obj = null;
						if (outArgs.TryGetValue(kvp.Key, out obj))
						{
							context.SetValue(kvp.Value, obj);
						}
					}
				}
			}
		}
		#endregion
	}
}
