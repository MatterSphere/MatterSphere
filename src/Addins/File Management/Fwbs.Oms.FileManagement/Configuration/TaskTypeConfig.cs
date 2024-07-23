using System;
using System.ComponentModel;
using System.Xml;

namespace FWBS.OMS.FileManagement.Configuration
{
    public sealed class TaskTypeConfig : System.ComponentModel.INotifyPropertyChanged
	{
		#region Constants

		public const string GLOBAL_TASK_TYPE_SCOPE = "";

		#endregion

		#region Fields

		private XmlElement _info;
		private FMApplication _app;
		private bool _isNew = false;
		
		/// <summary>
		/// Xml node for actions header information.
		/// </summary>
		private XmlElement _actionsHeader;

		/// <summary>
		/// Configured actions collection.
		/// </summary>
		private ActionConfigCollection _actions = null;
	
		#endregion

		#region Constructors

		private TaskTypeConfig (){}

		internal TaskTypeConfig(FMApplication app)
		{
			if (app == null)
				throw new ArgumentNullException("app");

			_app = app;
			_info = _app._config.CreateElement("TaskType");
			_app.WriteAttribute(_info, "code", "");
			_isNew = true;
			BuildXML();
		}

		internal TaskTypeConfig(FMApplication app, XmlElement info) 
		{
			if (app == null)
				throw new ArgumentNullException("app");

			if (info == null)
				throw new ArgumentNullException("info");

			_app = app;
			_info = info;
			BuildXML();
		}

		#endregion

		#region Properties

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




        [System.ComponentModel.Editor(typeof(FWBS.OMS.Design.DataListEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [FWBS.OMS.Design.DataList("DSTASKTYPE", Parse = true)]
        [System.ComponentModel.TypeConverter(typeof(FWBS.OMS.Design.DataListConverter))]
		[LocCategory("TASKS")]
		public string TaskType
		{
			get
			{
				string val = _app.ReadAttribute(_info, "code", "");
                if (val.Length > 15)
                    return val.Substring(0, 15);
                else
                    return val;
            }
            set
            {
                if (value != TaskType)
                {
                    if (value == null)
                        value = String.Empty;

                    if (value.Length > 15)
                        throw Utils.ThrowMaximumLengthExceededException(15);
                    _app.WriteAttribute(_info, "code", value);

                    OnPropertyChanged("TaskType");
                }
			}
		}

		[LocCategory("TASKS")]
        [TypeConverter(typeof(Design.TaskGroupConverter))]
		public string TaskGroup
		{
			get
			{
				string val = _app.ReadAttribute(_info, "group", "");
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
        [TypeConverter(typeof(Design.TaskFilterConverter))]
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

	
		[System.ComponentModel.Browsable(false)]
		public FMApplication Application
		{
			get
			{
				return _app;
			}
		}

		/// <summary>
		/// Gets the Actions collection.
		/// </summary>
		[LocCategory("ACTIONS")]
		[System.ComponentModel.Editor(typeof(Design.ActionConfigEditor), typeof(System.Drawing.Design.UITypeEditor))]
		public ActionConfigCollection Actions
		{
			get
			{
				return _actions;
			}
		}




		#endregion

		#region Methods

		private void BuildXML()
		{
			_actionsHeader = _info.SelectSingleNode("Actions") as XmlElement;
			if (_actionsHeader == null)
			{
				_actionsHeader = _app._config.CreateElement("Actions");
				_info.AppendChild(_actionsHeader);
			}			

			_actions = new ActionConfigCollection(_app, _actionsHeader);
			_actions.Cleared -=new Crownwood.Magic.Collections.CollectionClear(_app.OnDirty);
			_actions.Cleared +=new Crownwood.Magic.Collections.CollectionClear(_app.OnDirty);

		}

		public override string ToString()
		{
            if (!String.IsNullOrEmpty(this.TaskFilter))
                return this.TaskFilter;

            if (!String.IsNullOrEmpty(this.TaskGroup))
                return this.TaskGroup;

            if (!String.IsNullOrEmpty(this.TaskType))
                return this.TaskType;

			return String.Empty ;
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
