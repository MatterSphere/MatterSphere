#region References
using System;
#endregion

namespace FWBS.OMS.Workflow
{
    #region ContextItemWorkflow
    internal class ContextItemWorkflow : System.Activities.Presentation.ContextItem
	{
		#region Constructors
		public ContextItemWorkflow()
			: base()
		{}

		public ContextItemWorkflow(string code, string xaml)
			: this()
		{
			this.Code = code;
			this.Xaml = xaml;
		}
		#endregion

		#region Properties
		public override Type ItemType
		{
			get { return typeof(ContextItemWorkflow); }
		}

		internal string Code { get; set; }
		internal string Xaml { get; set; }
		#endregion
	}
	#endregion
}
