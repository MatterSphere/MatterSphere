using System;
using System.Collections;

namespace FWBS.OMS.FileManagement.Design
{
	internal class MilestoneTaskConfigEditor : CollectionEditorEx
    {
		public MilestoneTaskConfigEditor() : base (typeof(ArrayList)) 
		{
		}

		protected override System.Type CreateCollectionItemType ( )
		{
			return typeof (Configuration.MilestoneTaskConfig);
		}

		protected override object CreateInstance(System.Type t)
		{
			FMApplication app = null;
			app = (FMApplication)this.Context.Instance;
			return new Configuration.MilestoneTaskConfig(app);
		}

		protected override Type[] CreateNewItemTypes ( )
		{
			return new Type[] {typeof(Configuration.MilestoneTaskConfig)};
		}
	}
}
