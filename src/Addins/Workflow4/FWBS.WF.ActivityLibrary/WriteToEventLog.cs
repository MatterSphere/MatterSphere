#region References
using System;
using System.Activities;
using System.Diagnostics;
#endregion

namespace FWBS.WF.ActivityLibrary
{
	public sealed class WriteToEventLog : CodeActivity
	{
		#region Arguments
		[RequiredArgument]
		public InArgument<string> Message { get; set; }
		[RequiredArgument]
		public InArgument<string> EventLogSource { get; set; }
		[RequiredArgument]
		public InArgument<EventLogEntryType> EventLogType { get; set; }
		public InArgument<short> EventCategory { get; set; }				// if 0 then ignore
		public InArgument<int> EventId { get; set; }
		#endregion

		#region Overrides
		// Override CacheMetadata() to stop the default reflection way of discovering the arguments
		protected override void CacheMetadata(CodeActivityMetadata metadata)
		{
			System.Collections.ObjectModel.Collection<RuntimeArgument> arguments = new System.Collections.ObjectModel.Collection<RuntimeArgument>();

			RuntimeArgument argument = new RuntimeArgument("Message", typeof(string), ArgumentDirection.In, true);
			metadata.Bind(this.Message, argument);
			arguments.Add(argument);

			argument = new RuntimeArgument("EventLogSource", typeof(string), ArgumentDirection.In, true);
			metadata.Bind(this.EventLogSource, argument);
			arguments.Add(argument);

			argument = new RuntimeArgument("EventLogType", typeof(EventLogEntryType), ArgumentDirection.In, true);
			metadata.Bind(this.EventLogType, argument);
			arguments.Add(argument);

			argument = new RuntimeArgument("EventCategory", typeof(short), ArgumentDirection.In);
			metadata.Bind(this.EventCategory, argument);
			arguments.Add(argument);

			argument = new RuntimeArgument("EventId", typeof(int), ArgumentDirection.In);
			metadata.Bind(this.EventId, argument);
			arguments.Add(argument);

			metadata.SetArgumentsCollection(arguments);
		}

		protected override void Execute(CodeActivityContext context)
		{
			string eventSource = this.EventLogSource.Get(context);
			string message = this.Message.Get(context);
			short eventCategory = this.EventCategory.Get(context);
			int eventId = this.EventId.Get(context);
			EventLogEntryType eventLogType = this.EventLogType.Get(context);

			if (eventCategory == 0)
			{
				if (eventId == 0)
				{
					EventLog.WriteEntry(eventSource, message, eventLogType);
				}
				else
				{
					EventLog.WriteEntry(eventSource, message, eventLogType, eventId);
				}
			}
			else
			{
				EventLog.WriteEntry(eventSource, message, eventLogType, eventId, eventCategory);
			}
		}
		#endregion
	}
}
