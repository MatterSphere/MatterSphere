#region References
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
    [ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.GetContext.GetContextToolboxIcon.png")]
	[Description("Get Context")]
	[Designer(typeof(GetContextDesigner))]
	public sealed class GetContext : CodeActivity
	{
		#region Arguments
		[RequiredArgument]
		public InArgument<Type> InType { get; set; }

		public bool AutoPrompt { get; set; }

		[RequiredArgument]
		public OutArgument<object> OutObject { get; set; }
		#endregion

		#region Constructor
		public GetContext()
		{
			this.DisplayName = "Get Context";
		}
		#endregion

		#region Overrides
		/// <summary>
		/// Execute
		/// </summary>
		/// <param name="context"></param>
		protected override void Execute(CodeActivityContext context)
		{
			FWBS.OMS.IContext fwbsContext = context.GetExtension<FWBS.OMS.IContext>();

			if (fwbsContext != null)
			{
				Type type = InType.Get(context);
				object outObj = fwbsContext.Get<object>(type);

				// search parent if type is not in child (e.g. searchlist)
				// we do not search parent's parent etc!
				if ((outObj == null) && (fwbsContext.Parent != null))
				{
					outObj = fwbsContext.Parent.Get<object>(type);
				}

				// if no object and auto prompt then get it and set in context
				if (AutoPrompt && outObj == null)
				{
					if (type == typeof(FWBS.OMS.Associate))
					{
						outObj = FWBS.OMS.UI.Windows.Services.SelectAssociate();
					}
					else if (type == typeof(FWBS.OMS.Client))
					{
						outObj = FWBS.OMS.UI.Windows.Services.SelectClient();
					}
					else if (type == typeof(FWBS.OMS.Contact))
					{
						outObj = FWBS.OMS.UI.Windows.Services.Searches.FindContact();
					}
					else if (type == typeof(FWBS.OMS.OMSFile))
					{
						outObj = FWBS.OMS.UI.Windows.Services.SelectFile();
					}
					else if (type == typeof(FWBS.OMS.Precedent))
					{
						outObj = FWBS.OMS.UI.Windows.Services.Searches.FindPrecedent();
					}
					else if (type == typeof(FWBS.OMS.User))
					{
						outObj = FWBS.OMS.UI.Windows.Services.Searches.FindUser();
					}

					// set context
					if (outObj != null)
					{
						fwbsContext.Set(outObj);
					}
				}

				OutObject.Set(context, outObj);
			}
		}
		#endregion
	}
}
