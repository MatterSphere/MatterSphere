#region References
using System.Activities;
using System.ComponentModel;
using System.Drawing;

#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
    [ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.CreateKeyDate.CreateKeyDateToolboxIcon.png")]
	[Description("Creates a new Key Date")]
	[Designer(typeof(CreateKeyDateDesigner))]
	public sealed class CreateKeyDate : CodeActivity<bool>
	{
		#region Constructor
		public CreateKeyDate()
		{
			this.DisplayName = "Create Key Date";
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
		protected override bool Execute(CodeActivityContext context)
		{
			FWBS.OMS.OMSFile file = this.File.Get(context);

			if (file == null)
			{
				return FWBS.OMS.UI.Windows.Services.Wizards.CreateKeyDate();
			}
			else
			{
				return FWBS.OMS.UI.Windows.Services.Wizards.CreateKeyDate(file);
			}
		}
		#endregion
	}
}
