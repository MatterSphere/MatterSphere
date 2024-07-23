using System;
using System.Collections;

namespace FWBS.OMS.FileManagement.Design
{
	internal class ActionConfigEditor : CollectionEditorEx
    {
		public ActionConfigEditor() : base (typeof(ArrayList)) 
		{
		}

		protected override System.Type CreateCollectionItemType ( )
		{
			return typeof (Configuration.ActionConfig);
		}

		protected override object CreateInstance(System.Type t)
		{
			FMApplication app = null;
			if (this.Context.Instance is Configuration.TaskTypeConfig)
				app = ((Configuration.TaskTypeConfig)this.Context.Instance).Application;
			else
				app = (FMApplication)this.Context.Instance;
			return new Configuration.ActionConfig(app);
		}

		protected override Type[] CreateNewItemTypes ( )
		{
			return new Type[] {typeof(Configuration.ActionConfig)};
		}
	}
}
