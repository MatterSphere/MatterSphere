#region References
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
    [ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.FileActivities.AddFileNote.AddFileNoteToolboxIcon.png")]
	[Description("Add a File Note")]
	[Designer(typeof(AddFileNoteDesigner))]
	public sealed class AddFileNote : CodeActivity
	{
		#region Arguments
        [RequiredArgument]
		public InArgument<FWBS.OMS.OMSFile> File { get; set; }

		[RequiredArgument]
		public InArgument<string> Note { get; set; }
		#endregion

		#region Constructor
		public AddFileNote()
		{
			this.DisplayName = "Add File Note";
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

			argument = new RuntimeArgument("Note", typeof(string), ArgumentDirection.In, true);
			metadata.Bind(this.Note, argument);
			metadata.AddArgument(argument);
		}

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="context"></param>
		protected override void Execute(CodeActivityContext context)
		{
			var file = this.File.Get(context);
			var note = this.Note.Get(context);
			
			// Validate arguments
			if (string.IsNullOrWhiteSpace(note) || file == null)
			{
				string argName = "File";
				if (string.IsNullOrWhiteSpace(note))
				{
					argName = "Note";
				}
				string errMsg = string.Format("Activity Name='{0}' Argument='{1}.{2}' ID={3}", this.DisplayName, this.GetType().Name, argName, this.Id);
				throw new ArgumentNullException(errMsg);
			}
			else
			{
				file.AppendNoteText(FWBS.OMS.NoteAppendingLocation.Beginning, note);
                file.Update();
			}
		}
		#endregion
	}
}
