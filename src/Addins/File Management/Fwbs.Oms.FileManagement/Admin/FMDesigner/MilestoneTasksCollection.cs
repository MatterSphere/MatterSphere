namespace FWBS.OMS.FileManagement.Design
{
    internal class MilestoneTasksCollection
    {
        private Configuration.MilestoneTaskConfigCollection _msTasks = null;

        /// <summary>
        /// Gets the milestone tasks collection.
        /// </summary>
        [LocCategory("TASKS")]
        [Lookup("MSTASKS")]
        [System.ComponentModel.Editor(typeof(Design.MilestoneTaskConfigEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public Configuration.MilestoneTaskConfigCollection MilestoneTasks
        {
            get
            {
                return _msTasks;
            }
        }

        public MilestoneTasksCollection(FMApplication app)
        {
            this._msTasks = app.MilestoneTasks;
        }

    }
}

