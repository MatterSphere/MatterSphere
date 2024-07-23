using System;
using System.Collections;
using System.ComponentModel;

namespace FWBS.OMS.FileManagement.Milestones
{
    public sealed class MilestoneStage : IComparable, INotifyPropertyChanged
	{
		#region Fields

		private readonly FMApplicationInstance application;
		private readonly Milestones_OMS2K ms;
		private readonly byte number = 0;
		private readonly Milestones.TaskCollection tasks;
		private readonly Milestones.MilestonePlan plan;

		#endregion

		#region Events

		internal event TaskChangedEventHandler TaskChanged;

		internal void OnTaskChanged(TaskChangedEventArgs e)
		{
  			TaskChangedEventHandler ev = TaskChanged;
			if (ev != null)
				ev(this, e);

            RaisePropertyChanged("Status");
		}

		#endregion

		#region Constructors

		private MilestoneStage()
		{
		}

		internal MilestoneStage(FMApplicationInstance application, MilestonePlan plan, byte number)
		{
			if (application == null)
				throw new ArgumentNullException("application");

			if (plan== null)
				throw new ArgumentNullException("plan");

			if (number < 0 || number > plan.InternalPlan.Stages)
				throw new ArgumentException(String.Format("Stage must be greater than 1 and less than or equal to {0}", ms.Stages), "stage");
			
			this.application = application;
			this.ms = plan.InternalPlan;
			this.plan = plan;
			this.number = number;
			tasks = new Milestones.TaskCollection(application, this);
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

		internal MilestonePlan Plan
		{
			get
			{
				return plan;
			}
		}

		public OMSFile CurrentFile
		{
			get
			{
				return ms.OMSFile;
			}
		}

		public byte StageNumber
		{
			get
			{
				return number;
			}
		}

		public string Description
		{
			get
			{
				System.Reflection.PropertyInfo prop = ms.GetType().GetProperty(String.Format("MSStage{0}Desc", StageNumber));
				object val = prop.GetValue(ms, null);
				if (val == null) val = String.Empty;
				return Convert.ToString(val);
			}
		}

		
		public DateTime? Due
		{
			get
			{
				try
				{
                    //UTCFIX: DM - 05/12/06 - Return local date
					System.Reflection.PropertyInfo prop = ms.GetType().GetProperty(String.Format("MSStage{0}Due", StageNumber));
					object val = prop.GetValue(ms, null);
					if (val == null || val == DBNull.Value)
						return null;
					else
						return Convert.ToDateTime(val);
				}
				catch
				{
					return null;
				}
			}
            set
            {
                if (IsCompleted)
                    throw GetEditCompletedStageException();

                System.Reflection.PropertyInfo prop = ms.GetType().GetProperty(String.Format("MSStage{0}Due", StageNumber));
                DateTime? due = Due;
                if (due != value)
                {

					ArrayList reasons = new ArrayList();
					try
					{
						if (Application.CanChangeStageDue(this, reasons))
						{
							if (IsNull(value))
								prop.SetValue(ms, DBNull.Value, null);
							else
								prop.SetValue(ms, value, null);

							RaiseDuePropertyChanged();
						}
						else
						{
							throw Utils.ThrowCantChangeStageDue(this, reasons);
						}
					}
					catch (Exception ex)
					{
						FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
					}
					finally
					{
						RaiseDuePropertyChanged();
					}
                    
                }

                RaiseDuePropertyChanged();
                RaisePropertyChanged("DaysDue");
                RaisePropertyChanged("Status");

            }
		}

        private void RaiseDuePropertyChanged()
        {
            RaisePropertyChanged("Due");
            RaisePropertyChanged("_DueUI");
        }

		[Obsolete("Please use Due")]
		public DateTime? _DueUI
		{
			get
			{
				return Due;								
			}
			set
			{
				if (value == Due)
				{
					return;
				}

				ArrayList reasons = new ArrayList();
				try
				{
					if (Application.CanChangeStageDueUI(this, reasons))
					{
						Due = value;
						RaiseDuePropertyChanged();
					}
					else
					{						
						throw Utils.ThrowCantChangeStageDue(this, reasons);
					}
				}
				catch (Exception ex)
				{
					FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
				}
				finally
				{ 
					RaiseDuePropertyChanged(); 
				}
				                                
			}
		}

		public bool IsCompleted
		{
			get
			{
				return (IsNull(Achieved) == false);
			}
		}
		
		public DateTime? Achieved
		{
			get
			{
				try
				{
                    //UTCFIX: DM - 05/12/06 - Return local date.
					System.Reflection.PropertyInfo prop = ms.GetType().GetProperty(String.Format("MSStage{0}Achieved", StageNumber));
					object val = prop.GetValue(ms, null);
					if (val == null || val == DBNull.Value) 
						return null;
					else
						return Convert.ToDateTime(val);
				}
				catch
				{
					return null;
				}
			}
			set
			{
				if (IsCompleted)
					throw GetEditCompletedStageException();

				System.Reflection.PropertyInfo prop = ms.GetType().GetProperty(String.Format("MSStage{0}Achieved", StageNumber));
				DateTime? achieved = Achieved;
				if (achieved != value)
				{
					if (IsNull(value))
						prop.SetValue(ms, DBNull.Value, null);			
					else
						prop.SetValue(ms, value ,null);
	
				}

                RaisePropertyChanged("Achieved");
                RaisePropertyChanged("IsCompleted");
                RaisePropertyChanged("Status");
			}
		}

		public Milestones.TaskCollection Tasks
		{
			get
			{
				return tasks;
			}
		}

        public void RefreshTasks()
        {
            Tasks.Refresh();
            RaisePropertyChanged("Tasks");
        }

		private int CalculateUnits
		{
			get
			{
				System.Reflection.PropertyInfo prop = ms.GetType().GetProperty(String.Format("MSStage{0}CalcDays", StageNumber));
				object val = prop.GetValue(ms, null);
				if (val == null || val == DBNull.Value) val = 1;
				return Convert.ToInt32(val);
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
                //UTCFIX: DM - 05/12/06 - Make sure subtraction has dates with same kind
				if (IsCompleted || IsNull(Due))
					return 0;
				else
				{
					return Due.Value.ToLocalTime().Subtract(DateTime.Now).Days;
				}
			}
		}

		public StageStatus Status
		{
			get
			{
				if (IsCompleted)
					return StageStatus.Completed;
				else if (IsOverdue)
					return StageStatus.Overdue;
				else if (IsNextDue)
					return StageStatus.NextDue;
				else if (IsNull(Due))
					return StageStatus.Unspecified;
				else
					return StageStatus.Due;
			}
		}

		public bool IsNextDue
		{
			get
			{
				try
				{
					return (StageNumber == Convert.ToByte(ms.NextStage));
				}
				catch
				{
					return false;
				}
			}
		}

		#endregion

		#region Methods

		private FMException GetEditCompletedStageException()
		{
			return new FMException("EXEDITCOMPSTAGE", "Cannot edit stage '%1%' as it is currently completed.", null, false, StageNumber.ToString());
		}

		public bool IsNull(DateTime? date)
		{
			return (date.HasValue == false);
		}

		internal void Complete()
		{
			System.Reflection.PropertyInfo prop = ms.GetType().GetProperty(String.Format("MSStage{0}Checked", StageNumber));
			bool val = Common.ConvertDef.ToBoolean(prop.GetValue(ms, null), false);
			if (val == false)
				prop.SetValue(ms, true,null);
		}

		internal void UnComplete()
		{
			System.Reflection.PropertyInfo prop = ms.GetType().GetProperty(String.Format("MSStage{0}Checked", StageNumber));
			bool val = Common.ConvertDef.ToBoolean(prop.GetValue(ms, null), false);
			if (val)
				prop.SetValue(ms, false, null);
		}

		public override string ToString()
		{
			return String.Format("{0}.{1}", StageNumber, Description);
		}


		#endregion

		#region IComparable Members

		int IComparable.CompareTo(object obj)
		{
			MilestoneStage stage = obj as MilestoneStage;
			if (stage == null)
				return 0;
		
			return stage.StageNumber.CompareTo(this.StageNumber);
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
