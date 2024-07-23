using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;

namespace FWBS.OMS.FileManagement.Milestones
{
    public class TaskCollection : IEnumerable, INotifyCollectionChanged
	{
		#region Fields

		private readonly FMApplicationInstance application;
		private readonly DataView tasks;
		private readonly DataTable tasks_raw;
		private readonly Milestones.MilestonePlan plan;
		private readonly Milestones.MilestoneStage stage;

		#endregion

		#region Constructors

		private TaskCollection()
		{
		}

		internal TaskCollection(FMApplicationInstance application, Milestones.MilestonePlan plan)
		{
			if (application == null)
				throw new ArgumentNullException("application");

			if (plan == null)
				throw new ArgumentNullException("plan");

			this.application = application;
			this.plan = plan;
			tasks_raw = new DataTable();
			tasks_raw.Columns.Add("taskid", typeof(long));
			tasks_raw.Columns.Add("stage", typeof(byte));
			tasks_raw.Columns.Add("task", typeof(Task));
            tasks_raw.PrimaryKey = new DataColumn[] { tasks_raw.Columns["taskid"] };
			tasks = new DataView (tasks_raw, "", "", DataViewRowState.CurrentRows);
		}

		internal TaskCollection(FMApplicationInstance application, Milestones.MilestoneStage stage)
		{
			if (application == null)
				throw new ArgumentNullException("application");

			if (stage == null)
				throw new ArgumentNullException("stage");

            if (stage.Plan == null)
                throw new ArgumentNullException("plan");


            this.plan = stage.Plan;
			this.application = application;
			this.stage = stage;
            tasks_raw = stage.Plan.Tasks.tasks_raw;
            tasks = new DataView(tasks_raw, "", "", DataViewRowState.CurrentRows);
            tasks.RowFilter = String.Format("stage={0}", stage.StageNumber);
		}

		#endregion

		#region Methods

		internal void Clear()
		{
			tasks_raw.Rows.Clear();
			tasks_raw.AcceptChanges();
		}

		public void Refresh()
		{
            Refresh(false);
		}

        public void Refresh(bool full)
        {
            if (plan != null)
            {
                if (full)
                {
                    if (plan.CurrentFile != null)
                        plan.CurrentFile.Tasks.Refresh();
                }
                BuildMilestoneTasks();
            }
        }

		private void BuildMilestoneTasks()
		{
            Debug.WriteLine("BuildMilestoneTasks");
			FWBS.OMS.Tasks tks = plan.CurrentFile.Tasks;
			if (tks  != null)
			{
				tks.Updated-=new EventHandler(tks_Updated);
				tks.Updated+=new EventHandler(tks_Updated);
				string filter = String.Format("tskType = '{0}' and tskmsplan = '{1}'", Configuration.MilestoneTaskConfig.MILESTONE_TASK_TYPE, plan.Code);
				System.Data.DataView vw = tks.Find(filter);
                vw.Sort = "tskid";

                for (int ctr = tasks_raw.Rows.Count - 1; ctr >= 0; ctr--)
                {
                    DataRow originaltask = tasks_raw.Rows[ctr];
                    DataRowView drv;

                    int idx = vw.Find(originaltask["taskid"]);
                    if (idx >= 0)
                    {

                        drv = vw[idx];

                        if (drv.Row.RowState == System.Data.DataRowState.Modified || drv.Row.RowState == System.Data.DataRowState.Unchanged)
                        {
                            if (drv["tskmsstage"] != DBNull.Value)
                            {
                                byte stageno = Convert.ToByte(drv["tskmsstage"]);
                                if (stageno < 1) stageno = 1;
                                if (stageno > plan.Stages) stageno = plan.Stages;

                                Task tsk = (Task)originaltask["task"];
                                MilestoneStage st = plan[stageno];
                                tsk.Stage = st;
                                tsk.Row = drv.Row;
                                originaltask["stage"] = stageno;
                                continue;
                            }
                        }
                    }

                    originaltask.Delete();
                }

                

				foreach (System.Data.DataRowView drv in vw)
				{
                    if (drv.Row.RowState == DataRowState.Added || drv.Row.RowState == System.Data.DataRowState.Modified || drv.Row.RowState == System.Data.DataRowState.Unchanged)
					{
						if (drv["tskmsstage"] == DBNull.Value)
						{
						}
						else
						{
                            if (tasks_raw.Rows.Find(drv["tskid"]) == null)
                            {
                                byte stageno = Convert.ToByte(drv["tskmsstage"]);
                                if (stageno < 1) stageno = 1;
                                if (stageno > plan.Stages) stageno = plan.Stages;
                                MilestoneStage st = plan[stageno];
                                Task tsk = new Task(application, drv.Row, st);
                                tasks_raw.Rows.Add(new object[] { drv["tskid"], stageno, tsk });
                            }
						}
					}
				}

                tasks_raw.AcceptChanges();
			}
            Debug.WriteLine("End BuildMilestoneTasks");
            RaiseCollectionChanged(NotifyCollectionChangedAction.Reset);
        }


		#endregion

		#region Properties


		public FMApplicationInstance Application
		{
			get
			{
				return application;
			}
		}

		public int Count
		{
			get
			{
				return tasks.Count;
			}
		}

        public int VisibleCount
        {
            get
            {
                int ctr = 0;
                foreach (Task tsk in this)
                {
                    if (tsk.Visible)
                        ctr++;
                }
                return ctr;
            }
        }

		public Task this[int index]
		{
			get
			{
				return (Task)tasks[index]["task"];
			}
		}

		public Task this [long taskId]
		{
			get
			{
				foreach (DataRowView r in tasks)
				{
					if (Convert.ToInt64(r["taskid"]) == taskId)
						return (Task)r["task"];
				}
				return null;
			}
		}

        public Task this[string filter]
        {
            get
            {
                if (filter == null)
                    filter = String.Empty;

                foreach (DataRowView r in tasks)
                {
                    Task task = (Task)r["task"];
                    if (task.FilterId.ToUpperInvariant().Equals(filter.ToUpperInvariant()))
                        return task;
                }
                return null;
            }
        }

		public bool AreCompleted
		{
			get
			{
				foreach (Task t in this)
				{
					if (t.IsCompleted == false && t.Visible)
						return false;
				}
				return true;
			}
		}

        public bool AreIncomplete
        {
            get
            {
                foreach (Task t in this)
                {
                    if (t.IsCompleted == true && t.Visible)
                        return false;
                }
                return true;
            }
        }

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
            foreach (DataRowView r in tasks)
            {
                //Makes sure deleted tasks are removed.
                Task task = (Task)r["task"];
                if (task.IsDeleted)
                {
                    r.Delete();
                    continue;
                }
                yield return task;
            }

            tasks_raw.AcceptChanges();
	    }

        #endregion

        #region Captured Events

        private void tks_Updated(object sender, EventArgs e)
		{
			Refresh();
		}

		#endregion

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public void RaiseCollectionChanged(NotifyCollectionChangedAction action)
        {
            var ev = CollectionChanged;
            if (ev != null)
                ev(this, new NotifyCollectionChangedEventArgs(action));
        }
    }
}
