using System;
using System.ComponentModel;
using System.Xml;

namespace FWBS.OMS.FileManagement.Configuration
{
    public sealed class MilestoneTaskConfig : LookupTypeDescriptor, System.ComponentModel.INotifyPropertyChanged
	{
		#region Constants

		public const string MILESTONE_TASK_TYPE = "MILESTONE";

		#endregion

		#region Fields

		private XmlElement _info;
		private FMApplication _app;
		private bool _isNew = false;

		#endregion

		#region Constructors

		private MilestoneTaskConfig (){}

		internal MilestoneTaskConfig(FMApplication app)
		{
			if (app == null)
				throw new ArgumentNullException("app");

			_app = app;
			_info = _app._config.CreateElement("MilestoneTask");
			this.MilestonePlan = app.DefaultMilestonePlan;
			_app.WriteAttribute(_info, "lookup", "");
			_isNew = true;
		}

		internal MilestoneTaskConfig(FMApplication app, XmlElement info) 
		{
			if (app == null)
				throw new ArgumentNullException("app");

			if (info == null)
				throw new ArgumentNullException("info");

			_app = app;
			_info = info;
			
		}

		#endregion

		#region Properties

		/// <summary>
		/// Specifies the task type.
		/// </summary>
		[LocCategory("TASKS")]
		[Browsable(false)]
		public string TaskType
		{
			get
			{
				return MILESTONE_TASK_TYPE;
			}
		}

		[LocCategory("TASKS")]
        [TypeConverter(typeof(Design.TaskGroupConverter))]
		public string TaskGroup
		{
			get
			{
				string val = _app.ReadAttribute(_info, "group", "") ;
                if (val.Length > 15)
                    return val.Substring(0, 15);
                else
                    return val;
			}
			set
			{
                if (value != TaskGroup)
                {
                    if (value == null)
                        value = String.Empty;

                    if (value.Length > 15)
                        throw Utils.ThrowMaximumLengthExceededException(15);

                    _app.WriteAttribute(_info, "group", value);
                    OnPropertyChanged("TaskGroup");
                }
			}
		}

		[LocCategory("TASKS")]
		public string TaskFilter
		{
			get
			{
				string val = _app.ReadAttribute(_info, "filter", "");
                if (val.Length > 50)
                    return val.Substring(0, 50);
                else
                    return val;
			}
			set
			{
                if (value != TaskFilter)
                {
                    if (value == null)
                        value = String.Empty;

                    if (value.Length > 50)
                        throw Utils.ThrowMaximumLengthExceededException(50);

                    _app.WriteAttribute(_info, "filter", value);
                    OnPropertyChanged("TaskFilter");
                }
			}
		}


		/// <summary>
		/// Specifies the milestone plan.
		/// </summary>
		[LocCategory("MILESTONES")]
		[Lookup("PLAN")]
		[RefreshProperties(RefreshProperties.All)]
        [System.ComponentModel.Editor(typeof(FWBS.OMS.Design.DataListEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [FWBS.OMS.Design.DataList("DSMSPLANS", UseNull = true, NullValue = "")]
        [System.ComponentModel.TypeConverter(typeof(FWBS.OMS.Design.DataListConverter))]
		public string MilestonePlan
		{
			get
			{
				string val = _app.ReadAttribute(_info, "plan", String.Empty).Trim();
                if (val.Length > 15)
                    return val.Substring(0, 15);
                else
                    return val;
			}
			set
			{
                if (value != MilestonePlan)
                {
                    if (value == null)
                        value = String.Empty;

                    if (value.Length > 15)
                        throw Utils.ThrowMaximumLengthExceededException(15);

                    _app.WriteAttribute(_info, "plan", value);
                    _app.WriteAttribute(_info, "stage", 0);
                    OnPropertyChanged("MilestonePlan");
                }
			}
		}

		/// <summary>
		/// Specifies the milestone stage scope.
		/// </summary>
		[LocCategory("MILESTONES")]
		[Lookup("STAGE")]
        [System.ComponentModel.Editor(typeof(FWBS.OMS.Design.DataListEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [FWBS.OMS.Design.DataList("DSMSPLANSTAGES", UseNull = true, NullValue = 0, OrderBy = "stage")]
		[Parameter("PLAN", "~OBJ.MilestonePlan", Parse=true)]
        [System.ComponentModel.TypeConverter(typeof(FWBS.OMS.Design.DataListConverter))]
		public byte MilestoneStage
		{
			get
			{
				try
				{
					return Convert.ToByte(_app.ReadAttribute(_info, "stage", ActionConfig.GLOBAL_MILESTONE_STAGE.ToString()));
				}
				catch
				{
					return ActionConfig.GLOBAL_MILESTONE_STAGE;
				}
			}
			set
			{
                if (value != MilestoneStage)
                {
                    _app.WriteAttribute(_info, "stage", value);
                    OnPropertyChanged("MilestoneStage");
                }
			}
		}

        /// <summary>
        /// Specifies the allocated team.
        /// </summary>
        [LocCategory("ASSIGNMENT")]
        [Lookup("ASSIGNEDTEAM")]
        [RefreshProperties(RefreshProperties.All)]
        [System.ComponentModel.Editor(typeof(FWBS.OMS.Design.DataListEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [FWBS.OMS.Design.DataList("DSTEAMS", UseNull = true, NullValue = -1)]
        [System.ComponentModel.TypeConverter(typeof(FWBS.OMS.Design.DataListConverter))]
        public int AssignedTeam
        {
            get
            {
                return Common.ConvDef.ToInt32(_app.ReadAttribute(_info, "team", "-1"), -1);
            }
            set
            {
                if (value != AssignedTeam)
                {
                    if (value == 0 || value == -1)
                        _app.WriteAttribute(_info, "team", "");
                    else
                        _app.WriteAttribute(_info, "team", value);
                    OnPropertyChanged("AssignedTeam");
                }
            }
        }


		[System.ComponentModel.Browsable(false)]
		public bool IsNew
		{
			get
			{
				return _isNew;
			}
		}

		[System.ComponentModel.Browsable(false)]
		internal XmlElement Element
		{
			get
			{
				return _info;
			}
		}

		[LocCategory("TASKS")]
		public string Description
		{
			get
			{
				string val = _app.ReadAttribute(_info, "description", "");
                if (val.Length > 100)
                    return val.Substring(0, 100);
                else
                    return val;
			}
			set
			{
                if (value != Description)
                {
                    if (value == null) value = String.Empty;
                    if (value.Length > 100)
                        throw Utils.ThrowMaximumLengthExceededException(100);

                    _app.WriteAttribute(_info, "description", value);
                    OnPropertyChanged("Description");
                }
			}
		}

		[LocCategory("TASKS")]
		[Lookup("DUEDATEOFFSET")]
		public int DueDateOffset
		{
			get
			{
				return Common.ConvertDef.ToInt16( _app.ReadAttribute(_info, "duedateoffset", ""), 0);				
			}
			set
			{
				if (value != DueDateOffset)
				{					
					_app.WriteAttribute(_info, "duedateoffset", value);
					OnPropertyChanged("DueDateOffset");
				}
			}
		}

		[LocCategory("TASKS")]
		public bool Manual
		{
			get
			{
				return Common.ConvertDef.ToBoolean(_app.ReadAttribute(_info, "manual", "false"), false);
			}
			set
			{
                if (value != Manual)
                {
                    _app.WriteAttribute(_info, "manual", value);
                    OnPropertyChanged("Manual");
                }
			}
		}


		[System.ComponentModel.Browsable(false)]
		public FMApplication Application
		{
			get
			{
				return _app;
			}
		}

		#endregion

		#region Methods

		public override string ToString()
		{
			return Description;
		}

		#endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            var ev = PropertyChanged;
            if (ev != null)
                ev(this, new PropertyChangedEventArgs(propertyName));
        }


        #endregion
	}
}
