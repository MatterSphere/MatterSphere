#region References
using System;
using System.Activities;
using System.Activities.Statements;
#endregion

namespace FWBS.WF.ActivityLibrary
{
	#region DelayUntilDateTime class
	// The DelayUntilDateTime activity will delay until the specified date and time if it is in the future.
	//	If the date and time is in the past, the activity will not delay
	public sealed class DelayUntilDateTime : NativeActivity
	{
		#region Fields
		private readonly Delay delay;																	// The delay activity
		private readonly Variable<TimeSpan> delayInterval = new Variable<TimeSpan>("delayInterval");	// The delay interval
		#endregion

		#region Arguments
		[RequiredArgument]
		public InArgument<DateTime> UntilDate { get; set; }
		#endregion

		#region Constructor
		public DelayUntilDateTime()
		{
			this.delay = new Delay { Duration = new InArgument<TimeSpan>(this.delayInterval) };
		}
		#endregion

		#region Properties
		protected override bool CanInduceIdle
		{
			get
			{
				return true;
			}
		}
		#endregion

		#region Overrides
		protected override void CacheMetadata(NativeActivityMetadata metadata)
		{
			metadata.AddArgument(new RuntimeArgument("UntilDate", typeof(DateTime), ArgumentDirection.In, true));
			metadata.AddImplementationChild(this.delay);
			metadata.AddImplementationVariable(this.delayInterval);
		}

		protected override void Execute(NativeActivityContext context)
		{
			this.delayInterval.Set(context, this.UntilDate.Get(context).Subtract(DateTime.Now));

			if (this.delayInterval.Get(context) > TimeSpan.Zero)
			{
				context.ScheduleActivity(this.delay);
			}
		}
		#endregion
	}
	#endregion
}
