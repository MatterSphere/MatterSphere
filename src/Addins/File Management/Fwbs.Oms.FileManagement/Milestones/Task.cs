using System;
using System.ComponentModel;

namespace FWBS.OMS.FileManagement.Milestones
{
	public sealed class Task: INotifyPropertyChanged
	{
		#region Fields

		private readonly FMApplicationInstance application;
		private Milestones.MilestoneStage _stage;
		private FWBS.OMS.Task task;
		private System.Data.DataRow drv_task;
        private FeeEarner feeearner;
        private User _completedby;
        private User _assignedto;
        private Team _team;

		#endregion

		#region Events

		private void OnTaskChanged()
		{
			if (Stage != null)
			{
				Milestones.TaskChangedEventArgs e = new Milestones.TaskChangedEventArgs(Stage, this);
				Stage.OnTaskChanged(e);
			}
			
		}

		#endregion

		#region Constructors

		private Task()
		{
		}

		internal Task(FMApplicationInstance application, FWBS.OMS.Task task, Milestones.MilestoneStage stage)
		{
			if (application == null)
				throw new ArgumentNullException("application");

			if (task == null)
				throw new ArgumentNullException("task");

			this.application = application;
			this.Stage = stage;
            SetTask(task);
		}

        private void SetTask(FWBS.OMS.Task task)
        {
            this.task = task;
            OnTaskSet();
        }
        private void SetTask(System.Data.DataRow task)
        {
            drv_task = task;
            OnTaskSet();
        }
        private void OnTaskSet()
        {
            RaisePropertyChanged("ID");
            RaisePropertyChanged("CurrentFile");
            RaisePropertyChanged("FeeEarner");
            RaisePropertyChanged("AssignedTeam");
            RaisePropertyChanged("AssignedTo");
            RaisePropertyChanged("RelatedDocument");
            RaisePropertyChanged("Type");
            RaisePropertyChanged("Group");
            RaisePropertyChanged("FilterID");
            RaisePropertyChanged("IsDeleted");
            RaisePropertyChanged("Description");
            RaisePropertyChanged("Due");
            RaisePropertyChanged("Created");
            RaisePropertyChanged("Updated");
            RaisePropertyChanged("CreatedBy");
            RaisePropertyChanged("UpdatedBy");
            RaisePropertyChanged("Notes");
            OnCompletionChanged();

        }

        private void OnCompletionChanged()
        {
            RaisePropertyChanged("IsCompleted"); 
            RaisePropertyChanged("CompletedBy");
            RaisePropertyChanged("Completed");
            RaisePropertyChanged("Status");
        }


		internal Task(FMApplicationInstance application, System.Data.DataRow task, Milestones.MilestoneStage stage)
		{
			if (application == null)
				throw new ArgumentNullException("application");

			if (task == null)
				throw new ArgumentNullException("task");

			this.application = application;
			this.Stage = stage;
            SetTask(task);
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

		public long ID
		{
			get
			{
				if (task == null)
					return Convert.ToInt64(GetValue("tskid"));
				else
					return task.ID;
			}
		}

		public OMSFile CurrentFile
		{
			get
			{
				if (task == null)
					return OMSFile.GetFile(Convert.ToInt64(GetValue("fileid")));
				else
					return task.File;
			}
		}

		public FeeEarner FeeEarner
		{
			get
			{
                if (feeearner == null)
                {
                    if (task == null)
                        feeearner = FeeEarner.GetFeeEarner(Convert.ToInt32(GetValue("feeusrid")));
                    else
                        feeearner = task.FeeEarner;
                }
                return feeearner;
			}
			set
			{
				if (IsCompleted)
					throw GetEditCompletedTaskException();

				if (value == null)
					throw new ArgumentNullException("FeeEarner");

                if (FeeEarner != value)
                {
                    if (task == null)
                        SetValue("feeusrid", value.ID);
                    else
                        task.FeeEarner = value;
                    feeearner = value;
                }

                RaisePropertyChanged("FeeEarner");
			}
		}

        public Team AssignedTeam
        {
            get
            {
                if (_team == null)
                {
                    if (task == null)
                    {
                        object id = GetValue("tmid");
                        if (id == DBNull.Value)
                            _team = null;
                        else
                            _team = Team.GetTeam(Convert.ToInt32(id));
                    }
                    else
                    {
                        _team = task.AssignedTeam;
                    }
                }

                return _team;
            }
            set
            {
                if (IsCompleted)
                    throw GetEditCompletedTaskException();

                if (AssignedTeam != value)
                {
                    if (value == null)
                    {
                        if (task == null)
                            SetValue("tmid", DBNull.Value);
                        else
                            task.SetExtraInfo("tmid", DBNull.Value);
                    }
                    else
                    {
                        if (task == null)
                            SetValue("tmid", value.ID);
                        else
                            task.SetExtraInfo("tmid", value.ID);
                    }

                    _team = value;
                    RaisePropertyChanged("AssignedTeam");
                }

               
            }
        }

        public User AssignedTo
        {
            get
            {

                if (_assignedto == null)
                {
                    if (task == null)
                    {
                        object userid = GetValue("usrid");
                        if (userid == DBNull.Value)
                            return null;
                        else
                            return User.GetUser(Convert.ToInt32(userid));
                    }
                    else
                    {
                        return task.AssignedTo;
                    }
                }

                return _assignedto;
            }
            set
            {
                if (IsCompleted)
                    throw GetEditCompletedTaskException();

                if (AssignedTo != value)
                {
                    if (value == null)
                    {
                        if (task == null)
                            SetValue("usrid", DBNull.Value);
                        else
                            task.SetExtraInfo("usrid", DBNull.Value);
                    }
                    else
                    {
                        if (task == null)
                            SetValue("usrid", value.ID);
                        else
                            task.SetExtraInfo("usrid", value.ID);
                    }

                    _assignedto = value;
                    RaisePropertyChanged("AssignedTo");
                }
            }
        }

		public OMSDocument RelatedDocument
		{
			get
			{
				object docid = DBNull.Value;
				if (task == null)
				{
					docid = GetValue("docid");		
				}
				else
				{
					docid = task.GetExtraInfo("docid");
				}

				if (docid == DBNull.Value)
					return null;
				else
					return OMSDocument.GetDocument(Convert.ToInt64(docid));

			}
		}

		public string Type
		{
			get
			{
				if (task == null)
					return Convert.ToString(GetValue("tsktype"));
				else
					return task.Type;
			}
			set
			{
                if (value == null) value = String.Empty;
                if (value.Length > 15)
                    throw Utils.ThrowMaximumLengthExceededException(15);

				if (IsCompleted)
					throw GetEditCompletedTaskException();

                if (Type != value)
                {
                    if (task == null)
                    {
                        if (value == null || value.Trim() == String.Empty)
                            SetValue("tsktype", DBNull.Value);
                        else
                            SetValue("tsktype", value);

                    }
                    else
                        task.Type = value;

                    RaisePropertyChanged("Type");
                }
			}
		}

		public string Group
		{
			get
			{
				if (task == null)
					return Convert.ToString(GetValue("tskgroup"));
				else
					return Convert.ToString(task.GetExtraInfo("tskgroup"));
			}
			set
			{
                if (value == null) value = String.Empty;
                if (value.Length > 15)
                    throw Utils.ThrowMaximumLengthExceededException(15);

				if (IsCompleted)
					throw GetEditCompletedTaskException();

                if (Group != value)
                {
                    if (task == null)
                    {
                        if (value == null || value.Trim() == String.Empty)
                            SetValue("tskgroup", DBNull.Value);
                        else
                            SetValue("tskgroup", value);

                    }
                    else
                        task.Group = value;

                    RaisePropertyChanged("Group");
                }
			}
		}

		public string FilterId
		{
			get
			{
				if (task == null)
					return Convert.ToString(GetValue("tskfilter"));
				else
					return Convert.ToString(task.GetExtraInfo("tskfilter"));
			}
            set
            {
                if (value == null) value = String.Empty;
                if (value.Length > 50)
                    throw Utils.ThrowMaximumLengthExceededException(50);

                if (value != FilterId)
                {
                    if (IsCompleted)
                        throw GetEditCompletedTaskException();


                    System.Data.DataView vw = CurrentFile.Tasks.Find(Application.GetTaskFilter(new string[] { value }, Common.TriState.Null, Common.TriState.Null));

                    if (vw.Count > 0)
                        throw new Exception(String.Format("Task with filter id '{0}' cannot be added as it already exists.", value));


                    if (task == null)
                    {
                        if (value == null || value.Trim() == String.Empty)
                            SetValue("tskfilter", DBNull.Value);
                        else
                            SetValue("tskfilter", value);

                    }
                    else
                    {
                        if (value == null || value.Trim() == String.Empty)
                            task.SetExtraInfo("tskfilter", DBNull.Value);
                        else
                            task.SetExtraInfo("tskfilter", value);
                    }

                    RaisePropertyChanged("FilterID");
                }
            }
		}

        internal bool IsDeleted
        {
            get
            {
                if (task == null)
                    return (drv_task.RowState == System.Data.DataRowState.Deleted || drv_task.RowState == System.Data.DataRowState.Detached);
                else
                    return false;
            }
        }

		public string Description
		{
			get
			{
				if (task == null)
					return Convert.ToString(GetValue("tskdesc"));
				else
					return task.Description;
			}
			set
			{
                if (value == null) value = String.Empty;
                if (value.Length > 100)
                    throw Utils.ThrowMaximumLengthExceededException(100);

				if (IsCompleted)
					throw GetEditCompletedTaskException();

                if (Description != value)
                {
                    if (task == null)
                    {
                        if (value == null || value.Trim() == String.Empty)
                            SetValue("tskdesc", DBNull.Value);
                        else
                            SetValue("tskdesc", value);

                    }
                    else
                    {
                        task.Description = value;
                    }

                    RaisePropertyChanged("Description");
                }
			}
		}

		public DateTime? Due
		{
			get
			{
				try
				{
                    //UTCFIX: DM - 05/12/06 - Return local date.
					if (task == null)
					{
						object dte = GetValue("tskdue");
						if (dte == null || dte == DBNull.Value)
							return null;
						else
							return Convert.ToDateTime(dte).ToLocalTime();
					}
					else
					{
						Common.DateTimeNULL dtn = task.Due;
						if (dtn.IsNull)
							return null;
						else
							return dtn.ToDateTime(null);
					}
				}
				catch
				{
					return null;
				}
			}
			set
			{
				if (IsCompleted)
					throw GetEditCompletedTaskException();

                if (Due != value)
                {
                    if (IsNull(value))
                    {
                        if (task == null)
                            SetValue("tskdue", DBNull.Value);
                        else
                            task.Due = DBNull.Value;
                    }
                    else
                    {
                        if (task == null)
                            SetValue("tskdue", value);
                        else
                            task.Due = value.Value;
                    }

                    RaisePropertyChanged("Due");
                    RaisePropertyChanged("DaysDue");
                    RaisePropertyChanged("IsOverdue");
                    RaisePropertyChanged("Status");
                }
			}
		}

		public DateTime? Completed
		{
			get
			{
				try
				{
                    //UTCFIX: DM - 05/12/06 - Return local date.
					if (task == null)
					{
						object dte = GetValue("tskcompleted");
						if (dte == null || dte == DBNull.Value)
							return null;
						else
							return Convert.ToDateTime(dte).ToLocalTime();
					}
					else
					{
						Common.DateTimeNULL dtn = task.Completed;
						if (dtn.IsNull)
							return null;
						else
							return dtn.ToDateTime(null);
					}
				}
				catch
				{
					return null;
				}
			}
		}

		public DateTime? Created
		{
			get
			{
				try
				{
                    //UTCFIX: DM - 05/12/06 - Return local date.
					object dte;
					if (task == null)
						dte = GetValue("Created");
					else
						dte = task.GetExtraInfo("Created");

					if (dte == null || dte == DBNull.Value)
						return null;
					else
						return Convert.ToDateTime(dte).ToLocalTime();
				}
				catch
				{
					return null;
				}
			}
		}

		public DateTime? Updated
		{
			get
			{
				try
				{
                    //UTCFIX: DM - 05/12/06 - Return local date.
					object dte;
					if (task == null)
						dte = GetValue("Updated");
					else
						dte = task.GetExtraInfo("Updated");

					if (dte == null || dte == DBNull.Value)
						return null;
					else
						return Convert.ToDateTime(dte).ToLocalTime();
				}
				catch
				{
					return null;
				}
			}
		}

		public bool IsCompleted
		{
			get
			{
				bool completed = false;

				if (task == null)
					completed = Common.ConvertDef.ToBoolean(GetValue("tskcomplete"), false);
				else
					completed = task.IsCompleted;

				if (IsNull(Completed) == false && completed)
					return true;
				else
					return false;
			}
		}

		public User CompletedBy
		{
			get
			{
                if (_completedby == null)
                {
                    object id;

                    if (task == null)
                        id = GetValue("tskcompletedby");
                    else
                        id = task.GetExtraInfo("tskcompletedby");

                    if (id == DBNull.Value || IsCompleted == false)
                        _completedby = null;
                    else
                        _completedby = User.GetUser(Convert.ToInt32(id));
                }

                return _completedby;

			}
            private set
            {
                if (_completedby == value)
                    return;

                _completedby = value;
                RaisePropertyChanged("CompletedBy");
            }

		}

		public User CreatedBy
		{
			get
			{
				object id;

				if (task == null)
					id = GetValue("createdby");
				else
					id = task.GetExtraInfo("createdby");

				if (id == DBNull.Value)
					return null;
				else
					return User.GetUser(Convert.ToInt32(id));

			}
		}

		public User UpdatedBy
		{
			get
			{
				object id;

				if (task == null)
					id = GetValue("updatedby");
				else
					id = task.GetExtraInfo("updatedby");

				if (id == DBNull.Value)
					return null;
				else
					return User.GetUser(Convert.ToInt32(id));

			}
		}

		public string Notes
		{
			get
			{
				if (task == null)
				{
					return Convert.ToString(GetValue("tsknotes"));
				}
				else
				{
					return task.Notes;
				}
			}
			set
			{
                if (Notes != value)
                {
                    if (value == null || value.Trim() == String.Empty) return;

                    if (task == null)
                    {
                        SetValue("tsknotes", value);
                    }
                    else
                    {
                        task.Notes = value;
                    }

                    RaisePropertyChanged("Notes");
                }
			}
		}

		public bool IsOverdue
		{
			get
			{
				return (DaysDue < 0);
			}
		}

		public int DaysDue
		{
			get
			{
                //UTCFIX: DM - 05/12/06 - Make sure date subtraction is of same kind.
				if (IsCompleted || IsNull(Due))
					return 0;
				else
				{
					return Due.Value.ToLocalTime().Subtract(DateTime.Now).Days;
				}
			}
		}

		public TaskStatus Status
		{
			get
			{
                if (Visible)
                {
                    if (IsCompleted)
                        return TaskStatus.Completed;
                    else if (IsOverdue)
                        return TaskStatus.Overdue;
                    else if (IsNull(Due))
                        return TaskStatus.Unspecified;
                    else
                        return TaskStatus.Due;
                }
                else
                    return TaskStatus.Unspecified;
			}
		}

		public bool IsMilestoneStageTask
		{
			get
			{
				return (Stage != null);
			}
		}


		public Milestones.MilestoneStage Stage
		{
			get
			{
				return _stage;
			}
            internal set
            {
                if (_stage == value)
                    return;
                _stage = value;
                RaisePropertyChanged("Stage");
                RaisePropertyChanged("IsMilestoneStageTask");
            }
		}


        internal System.Data.DataRow Row
        {
            get
            {
                return drv_task;
            }
            set
            {
                drv_task = value;
            }
        }

		public bool Manual
		{
			get
			{
				return Common.ConvertDef.ToBoolean(GetValue("tskManual"), true);
			}
            set
            {
                if (Manual != value)
                {
                    if (value != this.Manual)
                        SetValue("tskManual", value);
                }
            }
		}

        public bool Visible
        {
            get
            {
                return Common.ConvertDef.ToBoolean(GetValue("tskactive"), true);
            }
        }

		#endregion

		#region Methods

		private FMException GetEditCompletedTaskException()
		{
			return new FMException("EXEDITCOMPTASK", "Cannot edit task '%1%' as it is currently completed.", null, false, Description);
		}

		public bool IsNull(DateTime? date)
		{
			return (date.HasValue == false);
		}

		internal void Complete()
		{
			if (IsCompleted == false)
			{
				if (task == null)
				{
					SetValue("tskcompleted", System.DateTime.Now);
					SetValue("tskcomplete", true);
					SetValue("tskcompletedby", Session.CurrentSession.CurrentUser.ID);
                    CompletedBy = Session.CurrentSession.CurrentUser;
				}
				else
				{
					task.IsCompleted = true;
				}

                OnCompletionChanged();
			}
		}

		internal void UnComplete()
		{
			if (IsCompleted)
			{
				if (task == null)
				{
					SetValue("tskcompleted", DBNull.Value);
					SetValue("tskcomplete", false);
					SetValue("tskcompletedby", DBNull.Value);
                    CompletedBy = null;
				}
				else
				{
					task.IsCompleted = false;
				}

                OnCompletionChanged();
			}
		}



		public void AppendNote(NoteAppendingLocation location,  string note)
		{
			if (note == null || note.Trim() == String.Empty) return;

			System.Text.StringBuilder notes = new System.Text.StringBuilder(Notes);
			switch(location)
			{
				case NoteAppendingLocation.Beginning:
					if (notes.Length > 0)
					{
						notes.Insert(0, Environment.NewLine);
						notes.Insert(0, Environment.NewLine);
					}
					notes.Insert(0, note);
					break;
				default:
					if (notes.Length > 0)
					{
						notes.Append(Environment.NewLine);
						notes.Append(Environment.NewLine);
					}
					notes.Append(note);
					break;
			}

			Notes = notes.ToString();
		}

		private void SetValue(string name, object value)
		{
			if (drv_task == null)
				return;

			if (value == null) value = DBNull.Value;

            //UTCFIX: DM - 05/12/06 - Make unspecified dates default to local;
            if (value is DateTime)
            {
                DateTime dteval = (DateTime)value;
                if (dteval.Kind == DateTimeKind.Unspecified)
                    value = DateTime.SpecifyKind(dteval, DateTimeKind.Local);
            }

			object original = GetValue(name);
			object proposed = value;

			if (original == proposed)
				return;

			if (original != null)
			{
				if (original.Equals(proposed))
				{
					return;
				}
			}

			ValueChangingEventArgs changing = new ValueChangingEventArgs(name, original, proposed);
			Extensibility.ObjectEventArgs oea = new Extensibility.ObjectEventArgs(this, Extensibility.ObjectEvent.ValueChanging, true, changing); 
			application.ExecuteTaskEvent(this, oea);
            if (!changing.Cancel)
            {
                drv_task[name] = proposed;
                drv_task["updated"] = DateTime.Now;
                drv_task["updatedby"] = Session.CurrentSession.CurrentUser.ID;

                ValueChangedEventArgs changed = new ValueChangedEventArgs(name, original, proposed);
                Extensibility.ObjectEventArgs oea2 = new Extensibility.ObjectEventArgs(this, Extensibility.ObjectEvent.ValueChanged, true, changed);
                application.ExecuteTaskEvent(this, oea2);

                OnTaskChanged();
            }
		}

		private object  GetValue(string name)
		{
			if (drv_task == null)
				return DBNull.Value;

            //UTCFIX: DM - 05/12/06 - Return local date.
            object val = null;
            
            if (IsDeleted)
                val = drv_task[name, System.Data.DataRowVersion.Original];
            else
                val = drv_task[name];

            if (val is DateTime)
                return ((DateTime)val).ToLocalTime();
            else
                return val;
		}

		public override string ToString()
		{
			return String.Format("{0}", Description);
		}

        internal void Remove()
        {
           if (task == null)
                SetValue("tskactive", false);
            else
                task.SetExtraInfo("tskactive", false);
        }

        internal void Delete()
        {
            if (task == null)
            {
                this.drv_task.Delete();
            }
            else
                task.Delete();
        }


        internal void Update()
        {

            if (task == null)
            {
                CurrentFile.Tasks.Update();
            }
            else
                task.Update();

            OnTaskSet();
        }

		#endregion

        public event PropertyChangedEventHandler PropertyChanged;
        internal void RaisePropertyChanged(string property)
        {
            var ev = PropertyChanged;
            if (ev != null)
                ev(this, new PropertyChangedEventArgs(property));
        }
    }
}
