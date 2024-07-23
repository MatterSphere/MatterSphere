#region References
using System;
using System.Activities;
using System.Collections.Generic;
#endregion

namespace FWBS.WF.ActivityLibrary
{
	// The other workflow is subject to the rules of WorkflowInvoker (no bookmarks).
	//   The activity will invoke the other workflow an wait for it to complete before returning.
	//   Persistence is not allowed while the other workflow is invoked
	#region RunWorkflow class
	//public sealed class RunWorkflow : AsyncCodeActivity<IDictionary<string, object>>
	public sealed class RunWorkflow : CodeActivity<IDictionary<string, object>>
	{
		#region Arguments
		public InArgument<Activity> Activity { get; set; }
		public InArgument<IDictionary<string, object>> Input { get; set; }
		#endregion

		#region Overrides
		// Override CacheMetadata() to stop the default reflection way of discovering the arguments
		protected override void CacheMetadata(CodeActivityMetadata metadata)
		{
			System.Collections.ObjectModel.Collection<RuntimeArgument> arguments = new System.Collections.ObjectModel.Collection<RuntimeArgument>();

			RuntimeArgument argument = new RuntimeArgument("Activity", typeof(Activity), ArgumentDirection.In);
			metadata.Bind(this.Activity, argument);
			arguments.Add(argument);

			argument = new RuntimeArgument("Input", typeof(IDictionary<string, object>), ArgumentDirection.In);
			metadata.Bind(this.Input, argument);
			arguments.Add(argument);

			metadata.SetArgumentsCollection(arguments);			
		}

		protected override IDictionary<string, object> Execute(CodeActivityContext context)
		{
			var invoker = new WorkflowInvoker(this.Activity.Get(context));
			// get the tracking participant and add it to this one
			System.Activities.Tracking.TrackingParticipant tp = context.GetExtension<System.Activities.Tracking.TrackingParticipant>();
			if (tp != null)
			{
				invoker.Extensions.Add(tp);
			}
			// get the oms context and add to this one
			FWBS.OMS.IContext fwbsContext = context.GetExtension<FWBS.OMS.IContext>();
			if (fwbsContext != null)
			{
				invoker.Extensions.Add(fwbsContext);
			}

			IDictionary<string, object> args = this.Input.Get(context);
			if (args != null)
			{
				return invoker.Invoke(args);
			}
			else
			{
				return invoker.Invoke();
			}
		}

		#endregion
	}
	#endregion
}