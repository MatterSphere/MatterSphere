#region References
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
    [ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.FileActivities.AddFileEvent.AddFileEventToolboxIcon.png")]
	[Description("Add a File Event")]
	[Designer(typeof(AddFileEventDesigner))]
	public sealed class AddFileEvent : CodeActivity
	{
		#region Arguments
		[RequiredArgument]
		public InArgument<FWBS.OMS.OMSFile> File { get; set; }

		[RequiredArgument]
		public InArgument<string> EventType { get; set; }

		[RequiredArgument]
		public InArgument<string> EventDescription { get; set; }

		[RequiredArgument]
		public InArgument<string> ExtraInfo { get; set; }
		#endregion

		#region Constructor
		public AddFileEvent()
		{
			this.DisplayName = "Add File Event";
		}
		#endregion

		#region Overrides
        /// <summary>
        /// CacheMetadata
        /// </summary>
        /// <param name="metadata"></param>
        protected override void CacheMetadata(CodeActivityMetadata metadata)
		{
			RuntimeArgument argument = new RuntimeArgument("File", typeof(FWBS.OMS.OMSFile), ArgumentDirection.In, true);
			metadata.Bind(this.File, argument);
			metadata.AddArgument(argument);

			argument = new RuntimeArgument("EventType", typeof(string), ArgumentDirection.In, true);
			metadata.Bind(this.EventType, argument);
			metadata.AddArgument(argument);

			argument = new RuntimeArgument("EventDescription", typeof(string), ArgumentDirection.In, true);
			metadata.Bind(this.EventDescription, argument);
			metadata.AddArgument(argument);

			argument = new RuntimeArgument("ExtraInfo", typeof(string), ArgumentDirection.In);
			metadata.Bind(this.ExtraInfo, argument);
			metadata.AddArgument(argument);
		}

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="context"></param>
		protected override void Execute(CodeActivityContext context)
		{			
			var file = this.File.Get(context);
			var type = this.EventType.Get(context);
			var description = this.EventDescription.Get(context);
			// Validate arguments
			if (string.IsNullOrWhiteSpace(description) || string.IsNullOrEmpty(type) || file == null)
			{
				string argName = "File";
				if (string.IsNullOrWhiteSpace(description))
				{
					argName = "EventDescription";
				}
				else if (string.IsNullOrWhiteSpace(type))
				{
					argName = "EventType";
				}
				string errMsg = string.Format("Activity Name='{0}' Argument='{1}.{2}' ID={3}", this.DisplayName, this.GetType().Name, argName, this.Id);
				throw new ArgumentNullException(errMsg);
			}
			else
			{
				file.AddEvent(type, description, this.ExtraInfo.Get(context));
                file.Update();
			}
		}
		#endregion
	}
}
