#region References
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
#endregion

namespace FWBS.WF.OMS.ActivityLibrary
{
	[ToolboxBitmap(typeof(ResourceFinder), "FWBS.WF.OMS.ActivityLibrary.FindPrecedent.FindPrecedentToolboxIcon.png")]
	[Description("Find a Precedent")]
	[Designer(typeof(FindPrecedentDesigner))]
	public sealed class FindPrecedent : CodeActivity<FWBS.OMS.Precedent>
	{
		#region Arguments
		public InArgument<string> PrecedentType { get; set; }

		public InArgument<FWBS.OMS.Associate> SelectedAssociate { get; set; }
		#endregion

		#region Constructor
		public FindPrecedent()
		{
			this.DisplayName = "Find Precedent";
		}
		#endregion

		#region Overrides
        /// <summary>
        /// CacheMetadata
        /// </summary>
        /// <param name="metadata"></param>
        protected override void CacheMetadata(CodeActivityMetadata metadata)
		{
			RuntimeArgument argument = new RuntimeArgument("PrecedentType", typeof(string), ArgumentDirection.In);
			metadata.Bind(this.PrecedentType, argument);
			metadata.AddArgument(argument);

			argument = new RuntimeArgument("SelectedAssociate", typeof(FWBS.OMS.Associate), ArgumentDirection.In);
			metadata.Bind(this.SelectedAssociate, argument);
			metadata.AddArgument(argument);
		}

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
		protected override FWBS.OMS.Precedent Execute(CodeActivityContext context)
		{
			FWBS.OMS.Associate assoc = this.SelectedAssociate.Get(context);
			string precType = this.PrecedentType.Get(context);

			if ((assoc != null) && (precType != null))
			{
				return FWBS.OMS.Precedent.GetDefaultPrecedent(this.PrecedentType.Get(context), assoc);
			}
			else
			{
				return FWBS.OMS.UI.Windows.Services.Searches.FindPrecedent();
			}
		}
		#endregion
	}
}
