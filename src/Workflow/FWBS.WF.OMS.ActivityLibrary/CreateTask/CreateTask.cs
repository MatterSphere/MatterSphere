#region References
using System.Activities;
using System.ComponentModel;
using System.Drawing;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
    [ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.CreateTask.CreateTaskToolboxIcon.bmp")]
	[Description("Creates a new Task")]
	[Designer(typeof(CreateTaskDesigner))]
	public sealed class CreateTask : CodeActivity<FWBS.OMS.Task>
	{
		#region Constructor
		public CreateTask()
		{
			this.DisplayName = "Create Task";
		}
		#endregion

		#region Arguments
		public InArgument<FWBS.OMS.OMSFile> File { get; set; }
		#endregion

		#region Overrides
        /// <summary>
        /// CacheMetadata
        /// </summary>
        /// <param name="metadata"></param>
        protected override void CacheMetadata(CodeActivityMetadata metadata)
		{
			RuntimeArgument argument = new RuntimeArgument("File", typeof(FWBS.OMS.OMSFile), ArgumentDirection.In);
			metadata.Bind(this.File, argument);
			metadata.AddArgument(argument);
		}

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
		protected override FWBS.OMS.Task Execute(CodeActivityContext context)
		{
			FWBS.OMS.OMSFile file = this.File.Get(context);

			if (file == null)
			{
				return FWBS.OMS.UI.Windows.Services.Wizards.CreateTask();
			}
			else
			{
				return FWBS.OMS.UI.Windows.Services.Wizards.CreateTask(file);
			}
		}
		#endregion
	}
}
