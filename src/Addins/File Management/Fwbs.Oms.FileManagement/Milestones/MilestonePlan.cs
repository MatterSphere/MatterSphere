using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;

namespace FWBS.OMS.FileManagement.Milestones
{
    public sealed class MilestonePlan : IEnumerable, INotifyCollectionChanged
	{
		#region Events

		internal event EventHandler Refreshed;		
		internal event StageChangedEventHandler StageChanged;
        internal event EventHandler Dirty;

		internal void OnStageChanged(StageChangedEventArgs e)
		{
			StageChangedEventHandler ev = StageChanged;
			if (ev != null)
				ev(this, e);

			RaiseCollectionChanged(NotifyCollectionChangedAction.Reset);
		}


        private void OnDirty()
        {
            EventHandler ev = Dirty;
            if (ev != null)
                ev(this, EventArgs.Empty);
        }


		private void OnRefreshed()
		{
			if (needsrefresh && suspendrefresh == false)
			{
				EventHandler ev = Refreshed;
				if (ev!= null)
					ev(this, EventArgs.Empty);

				needsrefresh = false;
			}
		}

		internal void MilestonePlanTasksApplied()
		{
			Debug.WriteLine("OnMilestonePlanTasksApplied", "MilestonePlan");
			if (Application != null)
			{
				Application.ExecuteMilestonePlanTasksApplied();
			}
		}

		#endregion

		#region Fields

		private readonly FMApplicationInstance application;
		private readonly Milestones_OMS2K ms;
		private readonly System.Collections.SortedList stages;
		private readonly TaskCollection tasks;

		private bool needsrefresh = false;
		private bool suspendrefresh = false;
        private bool tasksneedgenerating = true;
        private bool stagesneedbuilding = true;

		#endregion

		#region Constructors

		private MilestonePlan()
		{
		}

		internal MilestonePlan(FMApplicationInstance application, Milestones_OMS2K ms)
		{
			if (application == null)
				throw new ArgumentNullException("application");

			if (ms == null)
				throw new ArgumentNullException("ms");

			this.application = application;
			this.ms = ms;
			this.ms.ValueChanged+=new ValueChangedEventHandler(ms_ValueChanged);
            this.ms.DataChanged += new EventHandler(ms_DataChanged);
			this.ms.Updated-=new EventHandler(ms_Updated);
			this.ms.Updated+=new EventHandler(ms_Updated);
			stages = new System.Collections.SortedList();
			tasks = new TaskCollection(application, this);
			BuildStages();
			tasks.Refresh();

		}

		#endregion

		#region Properties

        public bool IsDirty
        {
            get
            {
                return (ms.IsDirty || CurrentFile.Tasks.IsDirty);
            }
        }

        public bool TasksNeedGenerating
        {
            get
            {
                return tasksneedgenerating;
            }
        }

		public FMApplicationInstance Application
		{
			get
			{
				return application;
			}
		}

        public Milestones_OMS2K InternalPlan
        {
            get
            {
                return ms;
            }
        }

		public string Code
		{
			get
			{
				return ms.MSPlan;
			}
		}

		public string Description
		{
			get
			{
				return ms.MSDescription;
			}
		}

		public OMSFile CurrentFile
		{
			get
			{
				return ms.OMSFile;
			}
		}

		public byte Stages
		{
			get
			{
				return (byte)stages.Count;
			}
		}

		public MilestoneStage this[byte stage]
		{
			get
			{
				return (MilestoneStage)stages[stage];
			}
		}

		public MilestoneStage NextStage
		{
			get
			{
				return (MilestoneStage)stages[NextStageNumber];
			}
		}

		public TaskCollection Tasks
		{
			get
			{
				return tasks;
			}
		}

		private byte NextStageNumber
		{
			get
			{
				try
				{
					byte num = Convert.ToByte(ms.NextStage);
                    if (num < 1)
                        num = 1;
                    else if (num > stages.Count)
                        num = Convert.ToByte(stages.Count);
                    return num;
				}
				catch
				{
					return 1;
				}
			}
		}

		private DateTime? NextStageDue
		{
			get
			{
				try
				{
                    //UTCFIX: DM - 05/12/06 - Return local date
					object date = ms.NextStageDueDate;
                    if (date == null || date == DBNull.Value)
                        return null;
                    else
                        return Convert.ToDateTime(date);
				}
				catch
				{
					return null;
				}
			}
		}

		#endregion

		#region Methods


		internal static void Assign(FMApplicationInstance app, string code, bool fromToday)
		{
			if (app == null)
				throw new ArgumentNullException("app");

			if (code.Length == 0)
				throw new ArgumentNullException("code");

			Milestones_OMS2K milestone = new Milestones_OMS2K(app.CurrentFile);
			milestone.SetMileStonePlan(code,fromToday);
			app.CurrentFile.MilestonePlan = milestone;			
		}

		internal void Remove()
		{
			bool suspend = suspendrefresh;

			try
			{
				suspendrefresh= true;

				tasks.Clear();
				stages.Clear();
				ms.RemoveMileStonePlan();
				RemoveTasks();
				UpdateTasks();
				UpdatePlan();
				CurrentFile.MilestonePlan = null;
                needsrefresh = true;
			}
			finally
			{
				suspendrefresh = suspend;
			}

			OnRefreshed();
		}

		internal void Reset()
		{
			bool suspend = suspendrefresh;

			try
			{
				suspendrefresh= true;

				string code = Code;
				Remove();
				ms.SetMileStonePlan(code, false);
				CurrentFile.MilestonePlan = ms;
				UpdatePlan();
				BuildStages();
				GenerateTasks();
				Tasks.Refresh();
				UpdateTasks();
			}
			finally
			{
				suspendrefresh = suspend;
			}

			OnRefreshed();
		}



		public void Refresh()
		{
            Refresh(false);
		}

        public void Refresh(bool full)
        {

            if (full)
                ms.Refresh();
            BuildStages();
            Tasks.Refresh(full);

            needsrefresh = true;
            OnRefreshed();
        }

        public void Cancel()
        {            
            ms.Cancel();
            CurrentFile.Tasks.Cancel();
            Refresh();
        }

        private void BuildStages()
        {
            stagesneedbuilding = Code == String.Empty;


            for (int i = stages.Count; i > ms.Stages; i--)
            {
                stages.RemoveAt(i - 1);
            }

            for (int ctr = stages.Count + 1; ctr <= ms.Stages; ctr++)
            {
                byte sn = (byte)ctr;
                stages.Add(sn, new MilestoneStage(application, this, sn));
            }

            RaiseCollectionChanged(NotifyCollectionChangedAction.Reset);
        }

		private void RemoveTasks()
		{
			OMSFile file = CurrentFile;
			FWBS.OMS.Tasks tasks = file.Tasks;
			System.Data.DataView vw = tasks.Find(String.Format("tsktype = '{0}'", Configuration.MilestoneTaskConfig.MILESTONE_TASK_TYPE));
			for (int ctr = vw.Count -1; ctr >= 0; ctr--)
			{
                vw[ctr].Delete();
			}
		}

		public void GenerateTasks()
		{
            Debug.WriteLine("Generating Tasks");
			OMSFile file = CurrentFile;
			FWBS.OMS.Tasks tasks = file.Tasks;
			bool taskschanged = false;
			

			System.Text.StringBuilder filter = new System.Text.StringBuilder();
			filter.Append(String.Format("(tskType = '{0}' and tskmsplan = '{1}')", Configuration.MilestoneTaskConfig.MILESTONE_TASK_TYPE, Code));
			System.Data.DataView vw = tasks.Find(filter.ToString());

			bool milestoneTasksAdded = vw.Count == 0;

            //for a split second this method gets ran when a new milestone plan is assigned
            //but the files ms code is set to blank.
            tasksneedgenerating = (Code == String.Empty);

            if (!tasksneedgenerating)
            {
                Debug.WriteLine("Tasks Need Generating");

                //Same reason for the tasks rebuild the stages if needed to do so as the mileston plan code
                //has not been assigned properly yet.
                if (stagesneedbuilding)
                    BuildStages();

                System.Collections.Generic.List<Milestones.MilestoneStage> stages = new System.Collections.Generic.List<MilestoneStage>();

                foreach (Configuration.MilestoneTaskConfig tsk in application.Parent.MilestoneTasks)
                {
                    if (tsk.MilestonePlan.ToUpper() == Code.ToUpper())
                    {
                        byte stage = application.ValidateStage(tsk.MilestoneStage);

                        Milestones.MilestoneStage st = this[stage];
                        if (st == null) continue; // Not a valid stage.
                        DateTime? stagedue = null;
                        if (st.IsNull(st.Due) == false && !application.Parent.AllowNullDueDate)
                            stagedue = st.Due.Value;

                        Debug.WriteLine("Adding Task : " + tsk.Description + " (" + stage.ToString() + ")");						
						application.AddTask(file, Code, stage, tasks, tsk.Description, tsk.TaskGroup, tsk.TaskFilter, stagedue, ref taskschanged, tsk.Manual, (tsk.AssignedTeam == -1 ? null : (int?)tsk.AssignedTeam), null, false, false);						
                        // MNW CODE PERF
                        if ((!stages.Contains(st)) && (taskschanged))
                            stages.Add(st);
                        Debug.WriteLine("Done Adding Task : " + tsk.Description + " (" + stage.ToString() + ")");
                    }
                }

                foreach (MilestoneStage stge in stages)
                    stge.RefreshTasks();

				if (milestoneTasksAdded)
					MilestonePlanTasksApplied();
				
                Debug.WriteLine("Application Refresh GENERATE TASKS!");
                application.CurrentPlan.Tasks.Refresh();
                Debug.WriteLine("End Application Refresh GENERATE TASKS!"); 
                if (taskschanged)
                {
                    Debug.WriteLine("Update Tasks");
                    UpdateTasks();
                    Debug.WriteLine("Done Update Tasks");
                }
            }
		}

		public bool IsNull(DateTime? date)
		{
			return (date.HasValue == false);
		}

		public void Update()
		{
            bool tasksdirty = CurrentFile.Tasks.IsDirty;
            bool plandirty = ms.IsDirty;
            bool filenew = CurrentFile.IsNew;

            if (filenew == false)
            {
                UpdateTasks();
                UpdatePlan();
            }

            if (filenew || plandirty || tasksdirty)
            {
                BuildStages();
                Tasks.Refresh();
                OnRefreshed();
            }
		}

		private void UpdateTasks()
		{
			if (CurrentFile.Tasks.IsDirty && CurrentFile.IsNew == false)
			{
				CurrentFile.Tasks.Update();
				needsrefresh=true;
			}
		}

		private void UpdatePlan()
		{
            if (ms.IsDirty && CurrentFile.IsNew == false)
			{
				ms.Update();
				needsrefresh =true;
			}
		}

		internal MilestoneStage this [FWBS.OMS.Task task]
		{
			get
			{
				if (task.Type.ToUpper() == Configuration.MilestoneTaskConfig.MILESTONE_TASK_TYPE)
				{
					string msplan = Convert.ToString(task.GetExtraInfo("tskmsplan"));
					if (msplan.ToUpper() == Code.ToUpper())
					{
						string msstage = Convert.ToString(task.GetExtraInfo("tskmsstage"));
						try
						{
							byte stage = byte.Parse(msstage);
							return this[stage];
						}
						catch
						{}
					}
				}
				return null;
			}
		}

		internal MilestoneStage this [System.Data.DataRowView task]
		{
			get
			{
				if (Convert.ToString(task["tsktype"]).ToUpper() == Configuration.MilestoneTaskConfig.MILESTONE_TASK_TYPE)
				{
					string msplan = Convert.ToString(task["tskmsplan"]);
					if (msplan.ToUpper() == Code.ToUpper())
					{
						string msstage = Convert.ToString(task["tskmsstage"]);
						try
						{
							byte stage = byte.Parse(msstage);
							return this[stage];
						}
						catch
						{}
					}
				}
				return null;
			}
		}

		public override string ToString()
		{
			return Description;
		}

		#endregion

		#region Captured Events

		private void ms_Updated(object sender, EventArgs e)
		{
		}

		private void ms_ValueChanged(object sender, ValueChangedEventArgs e)
		{
			bool raise = false;
			StageData sd = StageData.DueDate;
			string name = e.Name.ToLower();

			byte stage = FMApplication.GetStageFromName(name);
			
			if (name.EndsWith("due"))
			{
				raise = true;
				sd = StageData.DueDate;
			}
			else if (name.EndsWith("achieved"))
			{
				raise = true;
				sd = StageData.AchievedDate;
			}
			else if (name.EndsWith("calcdays"))
			{
				raise = true;
				sd = StageData.CalculatedDays;
			}

			if (raise)
			{
				StageChangedEventArgs ea = new StageChangedEventArgs(this[stage], sd);
				OnStageChanged(ea);
			}

        }


        private void ms_DataChanged(object sender, EventArgs e)
        {
            OnDirty();
        }


        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
		{
			return stages.Values.GetEnumerator();
		}

		#endregion



        public event NotifyCollectionChangedEventHandler CollectionChanged;
        private void RaiseCollectionChanged(NotifyCollectionChangedAction action)
        {
            var ev = CollectionChanged;
            if (ev != null)
                ev(this, new NotifyCollectionChangedEventArgs(action));
        }
    }
}
