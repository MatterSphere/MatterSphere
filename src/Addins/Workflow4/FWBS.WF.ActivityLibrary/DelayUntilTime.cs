#region References
using System;
using System.Activities;
using System.Activities.Statements;
using System.Collections.Generic;
#endregion

namespace FWBS.WF.ActivityLibrary
{
	#region DelayUntilTime
	/// <summary>
	///   The DelayUntilTime activity
	/// </summary>
	/// <remarks>
	///   An activity that will delay until the next occurance of a time on an allowed day.
	///   If no days of the week are specified, any day is allowed
	/// </remarks>
	public sealed class DelayUntilTime : NativeActivity
	{
		#region Fields
		private readonly Delay delay;
		private readonly Variable<TimeSpan> delayInterval = new Variable<TimeSpan>("delayInterval");
		private List<DayOfWeek> occuranceDays;
		#endregion

		#region Arguments
		// Time to wait until
		[RequiredArgument]
		public InArgument<TimeSpan> Time { get; set; }
		#endregion

		#region Constructor
		public DelayUntilTime()
		{
			this.delay = new Delay { Duration = new InArgument<TimeSpan>(this.delayInterval) };
		}
		#endregion

		#region Properties
		//	The list of days where you want the event to occur
		public List<DayOfWeek> OccuranceDays
		{
			get
			{
				if (this.occuranceDays == null)
				{
					this.occuranceDays = new List<DayOfWeek>();
					foreach (var day in Enum.GetValues(typeof(DayOfWeek)))
					{
						this.occuranceDays.Add((DayOfWeek)day);
					}
				}

				return this.occuranceDays;
			}

			set
			{
				this.occuranceDays = value;
			}
		}

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
			metadata.AddArgument(new RuntimeArgument("Time", typeof(TimeSpan), ArgumentDirection.In, true));
			metadata.AddImplementationChild(this.delay);
			metadata.AddImplementationVariable(this.delayInterval);
		}

		protected override void Execute(NativeActivityContext context)
		{
			// Get the next occurance of the time on an allowed day
			var allowedDays = this.OccuranceDays;
			var interval = Occurance.Interval(this.Time.Get(context), allowedDays);

			// If the delay is in the future
			if (interval > TimeSpan.Zero)
			{
				this.delayInterval.Set(context, interval);
				context.ScheduleActivity(this.delay);
			}
		}
		#endregion
	}
	#endregion
}
