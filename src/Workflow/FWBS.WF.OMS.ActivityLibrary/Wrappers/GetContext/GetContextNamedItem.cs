#region References
using System;
using System.Activities;
using System.Activities.Expressions;
using System.ComponentModel;
using System.Drawing;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
    [ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.GetContext.GetContextToolboxIcon.png")]
	[Description("Get Context Named Item")]
	[Designer(typeof(GetContextNamedItemDesigner))]
	public sealed class GetContextNamedItem : Activity
	{
		#region Arguments
		[RequiredArgument]
		public InArgument<string> Key { get; set; }
		[RequiredArgument]
		public OutArgument<object> Value { get; set; }
		#endregion

		#region Constructor
		public GetContextNamedItem()
		{
			this.Implementation = this.CreateImplementation;
			this.DisplayName = "Get Context Named Item";
		}
		#endregion

		#region Methods
		private Activity CreateImplementation()
		{
			return new FWBS.WF.ActivityLibrary.GetContextNamedItem
			{
				Key = new InArgument<string>(ctx => this.Key.Get(ctx)),
				Value = new ArgumentReference<Object>("Value"),
			};
		}
		#endregion
	}
}
