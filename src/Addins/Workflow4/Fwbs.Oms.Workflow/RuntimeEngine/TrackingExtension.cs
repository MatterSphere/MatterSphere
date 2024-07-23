#region References
using System;
using System.Activities.Tracking;
#endregion

namespace FWBS.OMS.Workflow
{
    #region TrackingExtension
    internal class TrackingExtension : TrackingParticipant
	{
		#region Fields
		// Normal workflow logging
		private FWBS.Logging.TraceLogger logger = FWBS.Logging.TraceSourceFactory.CreateTraceSource(FWBS.OMS.Workflow.Constants.TRACE_SOURCE_NAME, 1000);
		// Activity and workflow instance tracking
		private FWBS.Logging.TraceLogger activityLogger = FWBS.Logging.TraceSourceFactory.CreateTraceSource(FWBS.OMS.Workflow.Constants.TRACE_ACTIVITY_SOURCE_NAME, 1000);
		#endregion

		#region Constructor
		internal TrackingExtension()                                                                                                                    
		{
			logger.TraceVerbose("TrackingExtension()");
			this.TrackingProfile = new DefaultActivityTrackingProfile();
			logger.TraceVerbose("TrackingExtension() - End");
		}
		#endregion

		#region Overrides
		#region Track
		protected override void Track(TrackingRecord record, TimeSpan timeout)
		{
			ActivityStateRecord activityStateRecord = record as ActivityStateRecord;
			if (activityStateRecord != null)
			{
				activityLogger.TraceVerbose("ASR {0}, {1}, {2}, {3}, {4}, {5}",
					new object[] { activityStateRecord.InstanceId, activityStateRecord.Activity.Name, activityStateRecord.Activity.Id, activityStateRecord.RecordNumber, activityStateRecord.State, activityStateRecord.EventTime.ToString() });
			}
			else
			{
				#region Workflow instance tracking
				WorkflowInstanceAbortedRecord wfInstanceAbortedRecord = record as WorkflowInstanceAbortedRecord;
				if (wfInstanceAbortedRecord != null)
				{
					activityLogger.TraceVerbose("WIR {0}, {1}, {2}, {3}", new object[] { wfInstanceAbortedRecord.InstanceId, wfInstanceAbortedRecord.State, wfInstanceAbortedRecord.Reason, wfInstanceAbortedRecord.RecordNumber });
				}
				else
				{
					WorkflowInstanceSuspendedRecord wfInstanceSuspendedRecord = record as WorkflowInstanceSuspendedRecord;
					if (wfInstanceSuspendedRecord != null)
					{
						activityLogger.TraceVerbose("WIR {0}, {1}, {2}, {3}", new object[] { wfInstanceSuspendedRecord.InstanceId, wfInstanceSuspendedRecord.State, wfInstanceSuspendedRecord.Reason, wfInstanceSuspendedRecord.RecordNumber });
					}
					else
					{
						WorkflowInstanceTerminatedRecord wfInstanceTerminatedRecord = record as WorkflowInstanceTerminatedRecord;
						if (wfInstanceTerminatedRecord != null)
						{
							activityLogger.TraceVerbose("WIR {0}, {1}, {2}, {3}", new object[] { wfInstanceTerminatedRecord.InstanceId, wfInstanceTerminatedRecord.State, wfInstanceTerminatedRecord.Reason, wfInstanceTerminatedRecord.RecordNumber });
						}
						else
						{
							WorkflowInstanceUnhandledExceptionRecord wfInstanceUnhandledExceptionRecord = record as WorkflowInstanceUnhandledExceptionRecord;
							if (wfInstanceUnhandledExceptionRecord != null)
							{
								activityLogger.TraceVerbose("WIR {0}, {1}, {2}, {3}, {4}", new object[] { wfInstanceUnhandledExceptionRecord.InstanceId, wfInstanceUnhandledExceptionRecord.State, wfInstanceUnhandledExceptionRecord.FaultSource.Name + wfInstanceUnhandledExceptionRecord.FaultSource.Id, wfInstanceUnhandledExceptionRecord.UnhandledException.Message, wfInstanceUnhandledExceptionRecord.RecordNumber});
							}
							else
							{
								WorkflowInstanceRecord wfInstanceRecord = record as WorkflowInstanceRecord;
								if (wfInstanceRecord != null)
								{
									activityLogger.TraceVerbose("WIR {0}, {1}, {2}", new object[] { wfInstanceRecord.InstanceId, wfInstanceRecord.State, wfInstanceRecord.RecordNumber });
								}
								else
								{
								}
							}
						}
					}
				}
				#endregion
			}
		}
		#endregion
		#endregion
	}
	#endregion

	#region DefaultActivityTrackingProfile
	internal class DefaultActivityTrackingProfile : TrackingProfile
	{
		internal DefaultActivityTrackingProfile()
			: base()
		{
			FWBS.Logging.TraceLogger logger = FWBS.Logging.TraceSourceFactory.CreateTraceSource(FWBS.OMS.Workflow.Constants.TRACE_SOURCE_NAME, 1000);
			logger.TraceVerbose("DefaultActivityTrackingProfile()");

			const string all = "*";												// track all
			this.Name = "FWBSDefaultActivityTrackingProfile";					// profile name
			this.ImplementationVisibility = ImplementationVisibility.All;		// visible to all
			this.Queries.Add(													// track workflow instance events
				new WorkflowInstanceQuery()
				{
					States = { all },
				});

			this.Queries.Add(													// track activity state events
				new ActivityStateQuery()
				{
					ActivityName = all,
					States = { all },
				});

			logger.TraceVerbose("DefaultActivityTrackingProfile() - End");
			logger.Close();
		}
	}
	#endregion
}
