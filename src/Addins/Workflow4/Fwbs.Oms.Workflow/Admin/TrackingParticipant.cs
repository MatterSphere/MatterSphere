using System;
using System.Activities;
using System.Activities.Tracking;

namespace FWBS.OMS.Workflow.Admin
{
    #region TrackingEventArgs class - custom
    internal class TrackingEventArgs : EventArgs
	{
		internal TrackingRecord Record { get; set; }
		internal TimeSpan Timeout { get; set; }
		internal Activity Activity { get; set; }

		internal TrackingEventArgs(TrackingRecord trackingRecord, TimeSpan timeout, Activity activity)
		{
			this.Record = trackingRecord;
			this.Timeout = timeout;
			this.Activity = activity;
		}
	}
	#endregion
}
