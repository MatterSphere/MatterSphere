#region References
using System.Activities;
using System.ComponentModel;
using System.Drawing;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
    [ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.GetContext.SetContextToolboxIcon.png")]
	[Description("Set Context")]
	[Designer(typeof(SetContextDesigner))]
	public sealed class SetContext : Activity
	{
		#region Arguments
		[RequiredArgument]
		public InArgument<object> Item { get; set; }
		#endregion

		#region Constructor
		public SetContext()
		{
			this.Implementation = this.CreateImplementation;
			this.DisplayName = "Set Context";
		}

		#endregion

		#region Methods
        /// <summary>
        /// Create Implementation
        /// </summary>
        /// <returns></returns>
		private Activity CreateImplementation()
		{
			return new FWBS.WF.ActivityLibrary.SetContext
			{
				Item = new InArgument<object>(ctx => this.Item.Get(ctx)),
			};
		}
		#endregion
	}
}
