namespace FWBS.OMS.FileManagement.Design
{
    internal class TaskTypeCollection
    {
        private Configuration.TaskTypeConfigCollection _taskTypes = null;

        /// <summary>
        /// Gets the Task Types and Task Actions configuration collection.
        /// </summary>
        [LocCategory("ACTIONS")]
        [System.ComponentModel.Editor(typeof(Design.TaskTypeEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public Configuration.TaskTypeConfigCollection TaskTypes
        {
            get
            {
                return _taskTypes;
            }
        }

        public TaskTypeCollection(FMApplication app)
        {
            this._taskTypes = app.TaskTypes;
        }

    }
}
