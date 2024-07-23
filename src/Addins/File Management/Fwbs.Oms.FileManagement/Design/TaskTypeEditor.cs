using System;
using System.Collections;

namespace FWBS.OMS.FileManagement.Design
{
    internal class CollectionEditorEx : System.ComponentModel.Design.CollectionEditor
    {
        public CollectionEditorEx(Type type) : base(type)
        {
        }

        /// <summary>
        /// Creates a new form to display and edit the current collection.
        /// Since this form internally has AutoScaleMode.Font and doesn't respect DPI awareness,
        /// we need to change font size in order to scale the form.
        /// </summary>
        /// <returns>
        /// A System.ComponentModel.Design.CollectionEditor.CollectionForm to provide as the user interface for editing the collection.
        /// </returns>
        protected override CollectionForm CreateCollectionForm()
        {
            CollectionForm form = base.CreateCollectionForm();
            if (form.DeviceDpi != 96)
            {
                form.Font = new System.Drawing.Font(form.Font.FontFamily, form.Font.Size * form.DeviceDpi / 96F);
            }
            return form;
        }
    }

    internal class TaskTypeEditor : CollectionEditorEx
    {
		public TaskTypeEditor() : base (typeof(ArrayList)) 
		{
		}

		protected override System.Type CreateCollectionItemType ( )
		{
			return typeof (Configuration.TaskTypeConfig);
		}

		protected override object CreateInstance(System.Type t)
		{
			FMApplication app = (FMApplication)this.Context.Instance;
			return new Configuration.TaskTypeConfig(app);
		}

		protected override Type[] CreateNewItemTypes ( )
		{
			return new Type[] {typeof(Configuration.TaskTypeConfig)};
		}
	}
}
