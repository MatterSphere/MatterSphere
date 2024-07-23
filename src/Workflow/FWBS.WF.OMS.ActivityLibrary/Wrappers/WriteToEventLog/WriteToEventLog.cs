#region References
using System;
using System.Activities;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
	#region WriteToEventLog
	//
	// THE ICON IN THE TOOLBOX IS DETERMINED BY ToolboxBitmap ATTRIBUTE
	// THE ICON ON THE DESIGNER IS DETERMINED BY Designer ATTRIBUTE
	//
	[ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.Wrappers.WriteToEventLog.WriteToEventLogToolboxIcon.png")]
	[Description("Write to Event Log")]
	[Designer(typeof(WriteToEventLogDesigner))]
	public sealed class WriteToEventLog : Activity
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

		#region Constructors and Destructors
		public WriteToEventLog()
		{
			this.Implementation = this.CreateImplementation;
			this.DisplayName = "Write to EventLog";
		}
		#endregion

		#region Overrides
        /// <summary>
        /// CacheMetadata
        /// </summary>
        /// <param name="metadata"></param>
        protected override void CacheMetadata(ActivityMetadata metadata)
		{
			RuntimeArgument argument = new RuntimeArgument("Message", typeof(string), ArgumentDirection.In, true);
			metadata.Bind(this.Message, argument);
			metadata.AddArgument(argument);

			argument = new RuntimeArgument("EventLogSource", typeof(string), ArgumentDirection.In, true);
			metadata.Bind(this.EventLogSource, argument);
			metadata.AddArgument(argument);

			argument = new RuntimeArgument("EventLogType", typeof(EventLogEntryType), ArgumentDirection.In, true);
			metadata.Bind(this.EventLogType, argument);
			metadata.AddArgument(argument);

			argument = new RuntimeArgument("EventCategory", typeof(short), ArgumentDirection.In);
			metadata.Bind(this.EventCategory, argument);
			metadata.AddArgument(argument);

			argument = new RuntimeArgument("EventId", typeof(int), ArgumentDirection.In);
			metadata.Bind(this.EventId, argument);
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
			return new FWBS.WF.ActivityLibrary.WriteToEventLog
			{
				EventLogSource = new InArgument<string>(ctx => this.EventLogSource.Get(ctx)),
				EventLogType = new InArgument<EventLogEntryType>(ctx => this.EventLogType.Get(ctx)),
				Message = new InArgument<string>(ctx => this.Message.Get(ctx)),
				EventCategory = new InArgument<short>(ctx => this.EventCategory.Get(ctx)),
				EventId = new InArgument<int>(ctx => this.EventId.Get(ctx)),
			};
		}
		#endregion
	}
	#endregion
}
