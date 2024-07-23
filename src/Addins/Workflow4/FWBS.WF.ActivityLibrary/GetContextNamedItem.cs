#region References
using System;
using System.Activities;
#endregion

namespace FWBS.WF.ActivityLibrary
{
	public sealed class GetContextNamedItem : CodeActivity
	{
		#region Arguments
		[RequiredArgument]
		public InArgument<string> Key { get; set; }
		[RequiredArgument]
		public OutArgument<object> Value { get; set; }
		#endregion

		#region Overrides
		protected override void Execute(CodeActivityContext context)
		{
			FWBS.OMS.IContext fwbsContext = context.GetExtension<FWBS.OMS.IContext>();

			if (fwbsContext != null)
			{
				string key = this.Key.Get(context);
				this.Value.Set(context, fwbsContext.Get<object>(key));
			}
		}
		#endregion
	}
}
