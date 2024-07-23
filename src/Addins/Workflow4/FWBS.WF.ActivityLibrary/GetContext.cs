#region References
using System;
using System.Activities;
#endregion

namespace FWBS.WF.ActivityLibrary
{
	public sealed class GetContext : CodeActivity
	{
		#region Arguments
		[RequiredArgument]
		public InArgument<Type> InType { get; set; }
		[RequiredArgument]
		public OutArgument<object> OutObject { get; set; }
		#endregion

		#region Overrides
		protected override void Execute(CodeActivityContext context)
		{
			FWBS.OMS.IContext fwbsContext = context.GetExtension<FWBS.OMS.IContext>();

			if (fwbsContext != null)
			{
				Type type = InType.Get(context);
				OutObject.Set(context, fwbsContext.Get<object>(type));
			}
		}
		#endregion
	}
}
