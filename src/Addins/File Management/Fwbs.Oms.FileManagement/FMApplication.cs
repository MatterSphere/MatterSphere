using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Xml;
using FWBS.OMS.Script;

namespace FWBS.OMS.FileManagement
{
    /// <summary>
    /// A FileManagement Application that controls the scripting of tasks, milestones and actions.
    /// </summary>
    public sealed class FMApplication : FWBS.OMS.CommonObject, System.ComponentModel.INotifyPropertyChanged
    {
        #region Constants

        public const string NO_APP = "";

        #endregion

        #region Fields

        /// <summary>
        /// Holds a reference to the script object for performing work flow actions actions.
        /// </summary>
        private Script.ScriptGen _script = null;

        private string _description = String.Empty;

        /// <summary>
        /// Configured actions collection.
        /// </summary>
        private Configuration.ActionConfigCollection _actions = null;

        /// <summary>
        /// Configured task types collection.
        /// </summary>
        private Configuration.TaskTypeConfigCollection _taskTypes = null;

        /// <summary>
        /// Configured milestone tasks collection.
        /// </summary>
        private Configuration.MilestoneTaskConfigCollection _msTasks = null;

        /// <summary>
        /// XML type configuration settings.
        /// </summary>
        internal XmlDocument _config;

        /// <summary>
        /// Xml node for FileManagement header information.
        /// </summary>
        private XmlElement _info;

        /// <summary>
        /// Xml node for actions header information.
        /// </summary>
        private XmlElement _actionsHeader;

        /// <summary>
        /// Xml node for task types header information.
        /// </summary>
        private XmlElement _taskTypesHeader;

        /// <summary>
        /// Xml node for milestone tasks header information.
        /// </summary>
        private XmlElement _msTasksHeader;

        /// <summary>
        /// Holds each instance of file application for each file.
        /// </summary>
        private Dictionary<OMSFile, FMApplicationInstance> _appinstances = new Dictionary<OMSFile, FMApplicationInstance>();

        private static Dictionary<OMSFile, FileExtender> _fileextenders = new Dictionary<OMSFile, FileExtender>();


        #endregion

        #region Constructors

        public FMApplication()
        {
            Session.CurrentSession.CheckLoggedIn();
        }

        private FMApplication(System.IO.FileInfo file)
            : base(file)
        {
            BuildXML();
        }

        internal FMApplication(FMApplication clone, string newCode)
            : this()
        {

            if (clone == null)
                throw new ArgumentNullException("clone");

            DataRow data = clone.GetDataTable().Rows[0];

            foreach (DataColumn cm in _data.Columns)
            {
                if (cm.ColumnName != this.FieldPrimaryKey)
                    _data.Rows[0][cm.ColumnName] = data[cm.ColumnName];
                else
                    _data.Rows[0][cm.ColumnName] = newCode;
            }

            if (_data.Columns.Contains(FieldCreatedBy) && Convert.IsDBNull(this.GetExtraInfo(FieldCreatedBy)))
                this.SetExtraInfo(FieldCreatedBy, Session.CurrentSession.CurrentUser.ID);

            if (_data.Columns.Contains(FieldCreated) && Convert.IsDBNull(this.GetExtraInfo(FieldCreated)))
                this.SetExtraInfo(FieldCreated, DateTime.Now);

            BuildXML();
        }

        #endregion

        #region Properties


        internal Dictionary<OMSFile, FMApplicationInstance> Instances
        {
            get
            {
                return _appinstances;
            }
        }

        [LocCategory("(Details)")]
        public string Code
        {
            get
            {
                return Convert.ToString(GetExtraInfo("appcode"));
            }
            set
            {
                SetExtraInfo("appcode", value);
            }
        }

        [LocCategory("(Details)")]
        public string Description
        {
            get
            {
                return FWBS.OMS.CodeLookup.GetLookup("FMAPPLICATION", Code);
            }
            set
            {
                if (value != Description)
                {
                    FWBS.OMS.CodeLookup.Create("FMAPPLICATION", Code, value, "", CodeLookup.DefaultCulture, true, true, true);
                    OnPropertyChanged("Description");
                }
            }
        }

        public int Version
        {
            get
            {
                if (_data.Columns.Contains("appVer"))
                {
                    return FWBS.Common.ConvertDef.ToInt32(GetExtraInfo("appVer"), 0);
                }
                else
                {
                    return 0;
                }
            }
        }


        /// <summary>
        /// Gets or Sets the task list code for the tasks search list that will be displayed on the tasks tab for matter types using this FM Application. If left blank the default will be used
        /// </summary>
        [Lookup("TSKSLOVERRIDE")]
        public string TasksSearchListOverride
        {
            get
            {
                return Convert.ToString(ReadAttribute(_info, "TasksSearchListOverride", string.Empty)).Trim();
            }            
            set
            {
                if (TasksSearchListOverride != value)
                {
                    if (value == null)
                        value = String.Empty;
                    WriteAttribute(_info, "TasksSearchListOverride", value);
                    OnPropertyChanged("TasksSearchListOverride");
                }
            }
        }

        /// <summary>
        /// The order that the available Actions are displayed in the Context menu
        /// </summary>
        [Lookup("TSKFLWMNUORDER")]
        [CodeLookupSelectorTitle("TSKFLWMNUORDER", "Order that Actions Are Displayed in Context Menu")]
        [System.ComponentModel.Browsable(false)]
        public ActionsOrderType ActionsOrderContextMenu
        {
            get
            {
                return (ActionsOrderType)Common.ConvertDef.ToEnum(ReadAttribute(_info, "ActionsOrderCtx", "0"),ActionsOrderType.FileActions1st);
            }
            set
            {
                if (ActionsOrderContextMenu != value)
                {
                    WriteAttribute(_info, "ActionsOrderCtx", value);
                    OnPropertyChanged("ActionsOrderCtx");
                }
            }
        }

        /// <summary>
        /// The code lookup value of the order that the available Actions are displayed in the Context menu
        /// The code lookup type is TSKFLWMNUORDER
        /// </summary>
        [Lookup("TSKFLWMNUORDER")]
        [CodeLookupSelectorTitle("TSKFLWMNUORDER", "Order that Actions Are Displayed in Context Menu")]
        public CodeLookupDisplayReadOnly ActionsOrderContextMenuCodeLookup
        {
            get
            {
                CodeLookupDisplayReadOnly cl = new CodeLookupDisplayReadOnly("TSKFLWMNUORDER");;
                cl.Code = Convert.ToString((int)ActionsOrderContextMenu);
                return cl;
            }
            set
            {
                ActionsOrderContextMenu = (ActionsOrderType)Common.ConvertDef.ToEnum(value.Code, ActionsOrderType.FileActions1st);
            }
        }

        /// <summary>
        /// The Action types that are displayed in the Context menu
        /// </summary>
        [Lookup("TSKFLWMENUDISP")]
        [CodeLookupSelectorTitle("TSKFLWMENUDISP", "Actions Types To Display in Context Menu")]
        [System.ComponentModel.Browsable(false)]
        public ActionsToDisplay ActionsInContextMenu
        {
            get
            {
                return (ActionsToDisplay)Common.ConvertDef.ToEnum(ReadAttribute(_info, "ActionsInContextMenu", "0"), ActionsToDisplay.FileAndTask);
            }
            set
            {
                if (ActionsInContextMenu != value)
                {
                    WriteAttribute(_info, "ActionsInContextMenu", value);
                    OnPropertyChanged("ActionsInContextMenu");
                }
            }
        }

        /// <summary>
        /// The code lookup value of the Action types that are displayed in the Context menu
        /// The code lookup type is TSKFLWMENUDISP
        /// </summary>
        [Lookup("TSKFLWMENUDISP")]
        [CodeLookupSelectorTitle("TSKFLWMENUDISP", "Actions Types To Display in Context Menu")]
        public CodeLookupDisplayReadOnly ActionsInContextMenuCodeLookup
        {
            get
            {
                CodeLookupDisplayReadOnly cl = new CodeLookupDisplayReadOnly("TSKFLWMENUDISP");
                cl.Code = Convert.ToString((int)ActionsInContextMenu);
                return cl;                
            }
            set
            {
                ActionsInContextMenu = (ActionsToDisplay)Common.ConvertDef.ToEnum(value.Code, ActionsToDisplay.FileAndTask);
            }
        }

        /// <summary>
        /// Gets the Actions collection.
        /// </summary>
        [LocCategory("ACTIONS")]
        [System.ComponentModel.Editor(typeof(Design.ActionConfigEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public Configuration.ActionConfigCollection FileActions
        {
            get
            {
                return _actions;
            }
        }

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


        /// <summary>
        /// Specifies the milestone plan.
        /// </summary>
        [LocCategory("MILESTONES")]
        [Lookup("PLAN")]
        [RefreshProperties(RefreshProperties.All)]
        [System.ComponentModel.Editor(typeof(FWBS.OMS.Design.DataListEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [FWBS.OMS.Design.DataList("DSMSPLANS", UseNull = false)]
        [System.ComponentModel.TypeConverter(typeof(FWBS.OMS.Design.DataListConverter))]
        public string DefaultMilestonePlan
        {
            get
            {
                return ReadAttribute(_info, "plan", String.Empty).Trim();
            }
            set
            {
                if (value != DefaultMilestonePlan)
                {
                    if (value == null)
                        value = String.Empty;
                    WriteAttribute(_info, "plan", value);
                    OnPropertyChanged("DefaultMilestonePlan");
                }
            }
        }

        [LocCategory("APPEARANCE")]
        [Lookup("TASKCOMPCOLOUR")]
        public System.Drawing.Color TaskCompletedColour
        {
            get
            {
                try
                {
                    return System.Drawing.Color.FromArgb(Convert.ToInt32(ReadAttribute(_info, "taskCompletedColour", System.Drawing.Color.FromArgb(210, 252, 210).ToArgb().ToString())));
                }
                catch
                {
                    return System.Drawing.Color.FromArgb(210, 252, 210);
                }
            }
            set
            {
                WriteAttribute(_info, "taskCompletedColour", value.ToArgb());
            }
        }

        [LocCategory("APPEARANCE")]
        [Lookup("TASKOVERDUECLR")]
        public System.Drawing.Color TaskOverdueColour
        {
            get
            {
                try
                {
                    return System.Drawing.Color.FromArgb(Convert.ToInt32(ReadAttribute(_info, "taskOverdueColour", System.Drawing.Color.FromArgb(255, 210, 210).ToArgb().ToString())));
                }
                catch
                {
                    return System.Drawing.Color.FromArgb(255, 210, 210);
                }
            }
            set
            {
                WriteAttribute(_info, "taskOverdueColour", value.ToArgb());
            }
        }

        [LocCategory("APPEARANCE")]
        [Lookup("TASKDUECOLOUR")]
        public System.Drawing.Color TaskDueColour
        {
            get
            {
                try
                {
                    return System.Drawing.Color.FromArgb(Convert.ToInt32(ReadAttribute(_info, "taskDueColour", System.Drawing.Color.FromKnownColor(KnownColor.Control).ToArgb().ToString())));
                }
                catch
                {
                    return System.Drawing.Color.FromKnownColor(KnownColor.Control);
                }
            }
            set
            {
                WriteAttribute(_info, "taskDueColour", value.ToArgb());
            }
        }

        [LocCategory("MILESTONES")]
        [Lookup("ALLOWMANUALTASK")]
        public bool AllowManualTasks
        {
            get
            {
                try
                {
                    return Convert.ToBoolean(ReadAttribute(_info, "AllowManualTask", "false"));
                }
                catch
                {
                    return false;
                }
            }
            set
            {
                WriteAttribute(_info, "AllowManualTask", value);
            }
        }

        [Lookup("FMSHOWALLTFCOLS")]
        public bool ShowAllTaskFlowColumns
        {
            get
            {
                try
                {
                    return Convert.ToBoolean(ReadAttribute(_info, "ShowAllTaskFlowColumns", "true"));
                }
                catch
                {
                    return false;
                }
            }
            set
            {
                WriteAttribute(_info, "ShowAllTaskFlowColumns", value);
            }
        }
        
        [Lookup("FMSHOWTFCHKBOX")]
        public bool ShowTaskFlowCheckboxes
        {
            get
            {
                try
                {
                    return Convert.ToBoolean(ReadAttribute(_info, "ShowTaskFlowCheckboxes", "true"));
                }
                catch
                {
                    return false;
                }
            }
            set
            {
                WriteAttribute(_info, "ShowTaskFlowCheckboxes", value);
            }
        }

        /// <summary>
        /// Allow Null Due Date, if the DueDate is set to Null don't update to Due date of Stage
        /// </summary>
        [LocCategory("MILESTONES")]
        [Lookup("ALLOWNULLDUED")]
        public bool AllowNullDueDate
        {
            get
            {
                try
                {
                    return Convert.ToBoolean(ReadAttribute(_info, "AllowNullDueDate", "false"));
                }
                catch
                {
                    return false;
                }
            }
            set
            {
                WriteAttribute(_info, "AllowNullDueDate", value);
            }
        }

        #endregion

        #region Methods

        private void UnloadInstances()
        {
            foreach (FMApplicationInstance appinst in _appinstances.Values)
            {
                appinst.Dispose();
            }

            _appinstances.Clear();
        }

        internal string[] GetActionMethodNames()
        {
            List<string> actions = new List<string>();

            foreach (Configuration.ActionConfig action in FileActions)
            {
                string name = action.Method.Trim();

                if (name != string.Empty)
                {
                    if (actions.Contains(name) == false)
                    {
                        actions.Add(name);
                    }
                }
            }

            foreach (Configuration.TaskTypeConfig tt in TaskTypes)
            {
                foreach (Configuration.ActionConfig action in tt.Actions)
                {
                    string name = action.Method.Trim();

                    if (name != string.Empty)
                    {
                        if (actions.Contains(name) == false)
                        {
                            actions.Add(name);
                        }
                    }
                }
            }

            if (_script != null)
            {
                FWBS.Common.ConfigSettingItem[] procs = _script.GetMethodProcedures();
                foreach (FWBS.Common.ConfigSettingItem itm in procs)
                {
                    string name = itm.GetString("name", "");
                    string code = itm.GetString("", "").Trim();
                    if (name != String.Empty && code != String.Empty)
                    {
                        if (actions.Contains(name) == false)
                        {
                            actions.Add(name);
                        }
                    }
                }
            }

            actions.Sort();

            return actions.ToArray();
        }


        /// <summary>
        /// Initialises the FileManagement application instance with a OMSFile instance.
        /// </summary>
        private FMApplicationInstance Initialise(OMSFile file)
        {
            if (file == null)
                throw new ArgumentNullException("file");

            if (_appinstances.ContainsKey(file))
            {
                _appinstances[file].Initialise(file);
                return _appinstances[file];
            }
            else
            {
                FMApplicationInstance appinst = new FMApplicationInstance(this);
                appinst.Initialise(file);
                _appinstances.Add(file, appinst);
                return appinst;
            }
        }

        internal void RemoveInstance(OMSFile file)
        {
            if (file == null)
                throw new ArgumentNullException("file");

            FMApplicationInstance appinst = null;
            if (_appinstances.ContainsKey(file))
            {
                appinst = _appinstances[file];
                _appinstances.Remove(file);
            }

            if (appinst != null)
                appinst.Dispose();
        }

        internal void ValidateAction(Configuration.ActionConfig action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            if (action.Application != this)
                throw new FMException("FMACTIONBADPAR", "The specified action '%1%' does not belong to the current File Management application.", null, true, action.Code);
        }


        public override string ToString()
        {
            return Description;
        }

        #endregion

        #region XML

        //Clears the xml object so it has to be rebuilt.
        private void ClearXML()
        {
            _config = null;
        }

        /// <summary>
        /// Builds a schema of a default type.
        /// </summary>
        private void BuildXML()
        {
            //Create the document if it does not already exist.
            if (_config == null)
                _config = new XmlDocument();

            _config.PreserveWhitespace = false;
            _config.LoadXml(Convert.ToString(GetExtraInfo("appXML")));

            System.Xml.XmlElement root = _config.SelectSingleNode("/Config") as System.Xml.XmlElement;
            if (root == null)
                root = _config.SelectSingleNode("/config") as System.Xml.XmlElement;

            if (root == null)
            {
                root = _config.CreateElement("Config");
                _config.AppendChild(root);
            }

            _info = root.SelectSingleNode("FileManagementApplication") as XmlElement;
            if (_info == null)
            {
                _info = _config.CreateElement("FileManagementApplication");
                root.AppendChild(_info);
            }


            _actionsHeader = _info.SelectSingleNode("Actions") as XmlElement;
            if (_actionsHeader == null)
            {
                _actionsHeader = _config.CreateElement("Actions");
                _info.AppendChild(_actionsHeader);
            }

            _taskTypesHeader = _info.SelectSingleNode("TaskTypes") as XmlElement;
            if (_taskTypesHeader == null)
            {
                _taskTypesHeader = _config.CreateElement("TaskTypes");
                _info.AppendChild(_taskTypesHeader);
            }

            _msTasksHeader = _info.SelectSingleNode("MilestoneTasks") as XmlElement;
            if (_msTasksHeader == null)
            {
                _msTasksHeader = _config.CreateElement("MilestoneTasks");
                _info.AppendChild(_msTasksHeader);
            }

            _actions = new Configuration.ActionConfigCollection(this, _actionsHeader);
            _actions.Cleared -= new Crownwood.Magic.Collections.CollectionClear(OnDirty);
            _actions.Cleared += new Crownwood.Magic.Collections.CollectionClear(OnDirty);

            _taskTypes = new Configuration.TaskTypeConfigCollection(this, _taskTypesHeader);
            _taskTypes.Cleared -= new Crownwood.Magic.Collections.CollectionClear(OnDirty);
            _taskTypes.Cleared += new Crownwood.Magic.Collections.CollectionClear(OnDirty);

            _msTasks = new Configuration.MilestoneTaskConfigCollection(this, _msTasksHeader);
            _msTasks.Cleared -= new Crownwood.Magic.Collections.CollectionClear(OnDirty);
            _msTasks.Cleared += new Crownwood.Magic.Collections.CollectionClear(OnDirty);
        }

        internal XmlElement GetConfigRoot()
        {
            return (XmlElement)_config.SelectSingleNode("/Config");
        }

        internal string ReadAttribute(XmlElement element, string name, string defaultValue)
        {
            XmlNode att = element.SelectSingleNode(name);
            if (att == null)
                att = element.GetAttributeNode(name);

            string ret = String.Empty;

            if (att != null)
            {
                if (att.NodeType == XmlNodeType.Element)
                    ret = att.InnerText;
                else
                    ret = att.Value;
            }

            if (ret == String.Empty)
                ret = defaultValue;

            return ret;
        }

        internal void WriteAttribute(XmlElement element, string name, object val)
        {
            if (val == null) return;

            string orig = ReadAttribute(element, name, "");
            if (orig == String.Empty || orig != Convert.ToString(val))
            {
                XmlNode att = element.SelectSingleNode(name);
                if (att == null)
                    att = element.GetAttributeNode(name);

                if (att != null)
                {
                    if (att.NodeType == XmlNodeType.Element)
                        att.InnerText = Convert.ToString(val);
                    else
                        att.Value = Convert.ToString(val);
                }
                else
                {
                    if (name == "")
                        element.InnerText = Convert.ToString(val);
                    else
                    {
                        XmlAttribute a = _config.CreateAttribute(name);
                        a.Value = Convert.ToString(val);
                        element.Attributes.Append(a);
                    }

                }


                OnDirty();
            }
        }


        #endregion

        #region CommonObject Implementation

        protected override string PartialSelectStatement
        {
            get
            {
                if (_data.Columns.Contains("appVer"))
                    return "select appCode, appVer from dbFileManagementApplication";
                else
                    return base.PartialSelectStatement;
            }
        }
        protected override string SelectExistsStatement
        {
            get
            {
                return "select appCode from dbFileManagementApplication";
            }
        }


        protected override string PrimaryTableName
        {
            get
            {
                return "dbFileManagementApplication";
            }
        }

        public override string FieldPrimaryKey
        {
            get
            {
                return "appCode";
            }
        }

        protected override string SelectStatement
        {
            get
            {
                return "select * from dbFileManagementApplication";
            }
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    UnloadScript();
                    UnloadInstances();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }


        #endregion

        #region Manipulation Methods


        public void Fetch(string code)
        {
            Fetch(code, false);
        }
        public void Fetch(string code, bool partial)
        {
            Cancel();
            if (partial)
                base.PartialFetch(code);
            else
            {
                base.Fetch(code);
                BuildXML();
            }
        }
        public override void Create()
        {
            base.Create();
            SetExtraInfo("appXML", "<Config/>");
            BuildXML();
        }

        public override void Cancel()
        {
            ClearXML();
            _actions = null;
            _taskTypes = null;
            _msTasks = null;
            IsDirty = false;
        }

        public override void Update()
        {
            //Check each milestone task for an empty description and prevent save if that exists to prevent issues at runtime
            foreach (Configuration.TaskTypeConfig task in TaskTypes)
            {
                if (task.ToString() == "")
                {
                    FWBS.OMS.UI.Windows.MessageBox.ShowInformation("FMTASKNODESC", "One or more tasks does not have a description.  Please correct before saving the task flow.", new string[0]);
                    return;
                }
            }
            //Check each action for an empty description and prevent save if that exists to prevent issues at runtime
            foreach (Configuration.ActionConfig action in FileActions)
            {
                if (Convert.ToString(action.Description) == "")
                {
                    FWBS.OMS.UI.Windows.MessageBox.ShowInformation("FMACTIONNODESC", "One or more actions does not have a description.  Please correct before saving the task flow.", new string[0]);
                    return;
                }
            }


            for (int ctr = _actionsHeader.ChildNodes.Count - 1; ctr >= 0; ctr--)
            {
                XmlNode nd = _actionsHeader.ChildNodes[ctr];
                if (nd is XmlElement)
                    _actionsHeader.RemoveChild(nd);
            }

            for (int ctr = _taskTypesHeader.ChildNodes.Count - 1; ctr >= 0; ctr--)
            {
                XmlNode nd = _taskTypesHeader.ChildNodes[ctr];
                if (nd is XmlElement)
                    _taskTypesHeader.RemoveChild(nd);
            }

            for (int ctr = _msTasksHeader.ChildNodes.Count - 1; ctr >= 0; ctr--)
            {
                XmlNode nd = _msTasksHeader.ChildNodes[ctr];
                if (nd is XmlElement)
                    _msTasksHeader.RemoveChild(nd);
            }

            foreach (Configuration.ActionConfig action in FileActions)
            {
                _actionsHeader.AppendChild(action.Element.CloneNode(false));
            }

            foreach (Configuration.TaskTypeConfig task in TaskTypes)
            {
                System.Xml.XmlElement tt = (System.Xml.XmlElement)_taskTypesHeader.AppendChild(task.Element.CloneNode(false));
                System.Xml.XmlElement tta = (System.Xml.XmlElement)tt.AppendChild(_config.CreateElement("Actions"));
                foreach (Configuration.ActionConfig action in task.Actions)
                {
                    tta.AppendChild(action.Element.CloneNode(false));
                }
            }

            foreach (Configuration.MilestoneTaskConfig mstask in MilestoneTasks)
            {
                if (mstask.Description.ToString() == "")
                {
                    FWBS.OMS.UI.Windows.MessageBox.ShowInformation("FMTASKNODESC", "One or more tasks does not have a description.  Please correct before saving the task flow.", new string[0]);
                    return;
                }

                if (mstask.TaskFilter.ToString() == "")
                {
                    FWBS.OMS.UI.Windows.MessageBox.ShowInformation("FMTASKNOFLTRID", "One or more tasks does not have an unique filter / id.  Please correct before saving the task flow.", new string[0]);
                    return;
                }

                _msTasksHeader.AppendChild(mstask.Element.CloneNode(false));
            }

            if (_data.Columns.Contains("appVer"))
            {
                // Increment the appVer column if exists.
                SetExtraInfo("appVer", Version + 1);
            }

            SetExtraInfo("appXML", _config.OuterXml);
            base.Update();

        }

        #endregion

        #region Script Related

        /// <summary>
        /// An event that gets raised when the script has be changed within the object.
        /// </summary>
        public event EventHandler ScriptChanged = null;

        /// <summary>
        /// Calls the script changed event.
        /// </summary>
        private void OnScriptChanged()
        {
            if (ScriptChanged != null)
                ScriptChanged(this, EventArgs.Empty);
        }


        /// <summary>
        /// Create a New Script object for the application.
        /// </summary>
        public void NewScript()
        {
            if (_script != null)
            {
                UnloadScript();
            }
            _script = null;
        }

        /// <summary>
        /// Gets or Sets the script name.
        /// </summary>
        [TypeConverter("FWBS.OMS.UI.Windows.Design.ScriptLister, omsadmin")]
        [ScriptTypeParam("FILEMANAGEMENT")]
        [LocCategory("SCRIPTING")]
        public string ScriptName
        {
            get
            {
                return Convert.ToString(GetExtraInfo("appScript"));
            }
            set
            {
                SetExtraInfo("appScript", value);
                this.Script.Code = value;
                OnScriptChanged();
            }
        }

        /// <summary>
        /// Gets a string value that indicates whether the application has an attached script.
        /// </summary>
        [Browsable(false)]
        public bool HasScript
        {
            get
            {
                try
                {
                    return !string.IsNullOrEmpty(Convert.ToString(GetExtraInfo("appScript")));
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets the scriptlet of the current FileManagement application.
        /// </summary>
        [Browsable(false)]
        public ScriptGen Script
        {
            get
            {
                if (_script == null)
                {
                    if (HasScript && ScriptGen.Exists(Convert.ToString(GetExtraInfo("appScript"))))
                    {
                        _script = ScriptGen.GetScript(Convert.ToString(GetExtraInfo("appScript")));

                    }
                    else
                    {
                        _script = new ScriptGen(Convert.ToString(GetExtraInfo("appScript")), "FILEMANAGEMENT");
                    }

                    if (_script != null)
                        _script.CompileFinished += new EventHandler(_script_CompileFinished);

                    ApplicationScriptType st = _script.ScriptType as ApplicationScriptType;
                    if (st != null)
                        st.SetAppObject(this);
                }

                return _script;
            }
        }

        private void _script_CompileFinished(object sender, EventArgs e)
        {
            foreach (FMApplicationInstance appinst in _appinstances.Values)
            {
                appinst.LoadScript(true);
            }
        }

        /// <summary>
        /// Gets the currently loaded scriptlet.
        /// </summary>
        internal ApplicationScriptType Scriptlet
        {
            get
            {
                if (_script == null)
                    return null;
                else
                    return _script.Scriptlet as ApplicationScriptType;
            }
        }

        private void UnloadScript()
        {
            if (_script != null)
            {
                _script.CompileFinished -= new EventHandler(_script_CompileFinished);
                _script.Dispose();
                _script = null;
            }
        }

        /// <summary>
        /// Loads the scriptlet into memory.
        /// </summary>
        /// <param name="designMode">Specifies whether the script should be loaded if it isin design mode.</param>
        private void LoadScript(bool designMode)
        {
            if (HasScript)
            {
                Script.Load(designMode);
                ApplicationScriptType scr = _script.Scriptlet as ApplicationScriptType;
                if (scr != null)
                {
                    scr.SetAppObject(this);
                }
            }
        }

        #endregion

        #region Dirty

        public event EventHandler Dirty;

        // Invoke the Dirty event; when changed
        internal void OnDirty()
        {
            if (Dirty != null)
            {
                IsDirty = true;
                Dirty(this, EventArgs.Empty);
                OnDataChanged();
            }
        }

        #endregion

        #region Static Methods

        internal static byte GetStageFromName(string name)
        {
            byte stage = 0;

            if (name.StartsWith("msstage0"))
                stage = 20;
            else if (name.StartsWith("msstage21"))
                stage = 21;
            else if (name.StartsWith("msstage22"))
                stage = 22;
            else if (name.StartsWith("msstage23"))
                stage = 23;
            else if (name.StartsWith("msstage24"))
                stage = 24;
            else if (name.StartsWith("msstage25"))
                stage = 25;
            else if (name.StartsWith("msstage26"))
                stage = 26;
            else if (name.StartsWith("msstage27"))
                stage = 27;
            else if (name.StartsWith("msstage28"))
                stage = 28;
            else if (name.StartsWith("msstage29"))
                stage = 29;
            else if (name.StartsWith("msstage30"))
                stage = 30;
            else if (name.StartsWith("msstage10"))
                stage = 10;
            else if (name.StartsWith("msstage11"))
                stage = 11;
            else if (name.StartsWith("msstage12"))
                stage = 12;
            else if (name.StartsWith("msstage13"))
                stage = 13;
            else if (name.StartsWith("msstage14"))
                stage = 14;
            else if (name.StartsWith("msstage15"))
                stage = 15;
            else if (name.StartsWith("msstage16"))
                stage = 16;
            else if (name.StartsWith("msstage17"))
                stage = 17;
            else if (name.StartsWith("msstage18"))
                stage = 18;
            else if (name.StartsWith("msstage19"))
                stage = 19;
            else if (name.StartsWith("msstage20"))
                stage = 20;
            else if (name.StartsWith("msstage1"))
                stage = 1;
            else if (name.StartsWith("msstage2"))
                stage = 2;
            else if (name.StartsWith("msstage3"))
                stage = 3;
            else if (name.StartsWith("msstage4"))
                stage = 4;
            else if (name.StartsWith("msstage5"))
                stage = 5;
            else if (name.StartsWith("msstage6"))
                stage = 6;
            else if (name.StartsWith("msstage7"))
                stage = 7;
            else if (name.StartsWith("msstage8"))
                stage = 8;
            else if (name.StartsWith("msstage9"))
                stage = 9;


            return stage;
        }

        public static FMApplication Clone(string code)
        {
            if (String.IsNullOrEmpty(code))
                throw new ArgumentNullException("code");

            FMApplication app = GetApplication(code);

            return new FMApplication(app, String.Empty);
        }

        internal static FileExtender GetFileExtender(OMSFile file)
        {
            if (file == null)
                throw new ArgumentNullException("file");

            lock (_fileextenders)
            {
                //Make sure that the file extenders that are no longer in the cached
                //list of file s is also removed.
                foreach (OMSFile fef in _fileextenders.Keys.ToArray())
                {  
                    //If the file is unitialised it must be removed from the list as this will cause errors later on.
                    if (fef.State == ObjectState.Unitialised)
                    {
                        _fileextenders.Remove(fef);
                        continue;
                    }

                    //If the file is in an added state then it must remain in the extenders list as is will not have a file id
                    //thus not having an entry in the CurrentSession.CurrentFile memory cache.  This file could be being worked on.
                    if (fef.State != ObjectState.Added && !Session.CurrentSession.CurrentFiles.Contains(fef.ID.ToString()))
                    {
                        _fileextenders.Remove(fef);
                    }
                }

                if (_fileextenders.ContainsKey(file))
                    return _fileextenders[file];
                else
                {
                    try
                    {
                        FileExtender ext = new FileExtender(file);
                        _fileextenders.Add(file, ext);
                        return ext;
                    }
                    catch (NotSupportedException)
                    {
                        _fileextenders.Add(file, null);
                        return null;
                    }
                }
            }
        }

        public static FMApplicationInstance GetApplicationInstance(OMSFile file)
        {
            if (file == null)
                throw new ArgumentNullException("file");


            string code = String.Empty;


            FMApplication fmapp = null;


            FileExtender fileext = GetFileExtender(file);

            if (fileext == null)
            {
                //Backward compatible.
                code = GetApplicationCode(file.CurrentFileType);
                fmapp = GetApplication(code);
            }
            else
            {
                code = fileext.FileManagementApplication;

                if (code == NO_APP)
                {
                    code = GetApplicationCode(file.CurrentFileType);

                    if (code != NO_APP)
                    {
                        fileext.FileManagementApplication = code;
                        fileext.Update();
                    }
                }

                fmapp = GetApplication(code);
            }



            System.Diagnostics.Debug.Assert(fmapp != null, "File Management Application does not exist.");

            if (fmapp == null)
                return null;

            if (!file.IsNew)
            {
                foreach (OMSFile fef in fmapp.Instances.Keys.ToArray())
                {
                    //If the file is unitialised it must be removed from the list as this will cause errors later on.
                    if (fef.State == ObjectState.Unitialised)
                    {
                        fmapp.Instances.Remove(fef);
                        continue;
                    }

                    //If the file is in an added state then it must remain in the  list as is will not have a file id
                    //thus not having an entry in the CurrentSession.CurrentFile memory cache.  This file could be being worked on.
                    if (fef.State != ObjectState.Added && !Session.CurrentSession.CurrentFiles.Contains(fef.ID.ToString()))
                    {
                        fmapp.Instances.Remove(fef);
                    }

                }
            }
            return fmapp.Initialise(file);
        }

        public static FMApplication GetApplication(FileType type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            string code = GetApplicationCode(type);
            return GetApplication(code);
        }

        public static string GetApplicationCode(FileType type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            string code = type.GetSetting("fileManagementApplication", NO_APP).Trim();
            if (code == NO_APP)
            {
                code = Convert.ToString(Session.CurrentSession.GetXmlProperty("DefaultFileManagementApplication", NO_APP));
            }

            return code;
        }

        public static FMApplication GetApplication(string code)
        {
            if (code == null)
                throw new ArgumentNullException("code");


            FMApplication wfapp = (FMApplication)Addins.Addin.LoadedFMApps[code];
            if (wfapp == null)
            {
                wfapp = new FMApplication();
                if (code == NO_APP)
                {
                    Addins.Addin.LoadedFMApps.Add(NO_APP, wfapp);
                }
                else
                {
                    FMApplication fromfile = Load(code);

                    try
                    {
                        if (fromfile == null)
                            wfapp.Fetch(code);
                        else
                        {
                            wfapp.Fetch(code, true);
                            if (wfapp.Version > fromfile.Version || wfapp.Version == 0)  //Version == 0 when the field does not exist.
                                wfapp.Fetch(code);
                            else
                            {
                                wfapp.Dispose();
                                wfapp = fromfile;
                            }
                        }

                        Addins.Addin.LoadedFMApps.Add(code, wfapp);
                        wfapp.Save();
                    }
                    catch (MissingCommonObjectException)
                    {
                        Addins.Addin.LoadedFMApps.Add(NO_APP, wfapp);
                    }

                }
            }

            return wfapp;
        }

        internal static FMApplicationInstance ResetApplication(OMSFile file, FMApplicationInstance original)
        {
            if (file == null)
                throw new ArgumentNullException("file");
            if (original == null)
                throw new ArgumentNullException("original");

            original.Parent.RemoveInstance(file);

            FileExtender fileext = GetFileExtender(file);

            if (fileext != null)
            {
                fileext.FileManagementApplication = NO_APP;
                fileext.Update();
            }

            return GetApplicationInstance(file);

        }

        /// <summary>
        /// Gets the relevant actions to display in the context menu
        /// If a User object is passed in then it looks at the Users Setting then the Users works for default department's setting
        /// If a OMSFile object is passed then it looks at the settings on the FMApplication app
        /// </summary>
        /// <param name="obj">IOMSType User or OMSFile</param>
        /// <param name="app">The FMApplication</param>
        /// <returns>ActionsToDisplay setting that corresponds to the obj and app parameters</returns>
        internal static ActionsToDisplay GetActionsToDisplaySetting(Interfaces.IOMSType obj,FMApplication app)
        {
            try
            {
                if (obj is User)
                {
                    User user = (User)obj;
                    int option = Convert.ToInt32(user.GetXmlProperty("ActionsInContextMenu", -1));

                    if (option == -1)
                        option = new Department(user.WorksFor.DefaultDepartment).ActionsInContextMenu;

                    return (ActionsToDisplay)Common.ConvertDef.ToEnum(option,ActionsToDisplay.FileAndTask);
                }

                if (obj is OMSFile)
                {
                    return app.ActionsInContextMenu;
                }
            }
            //If an error occurs continue and display the default
            catch { }

            return ActionsToDisplay.FileAndTask;
        }

        /// <summary>
        /// Gets the relevant action ordering setting to display in the context menu
        /// If a User object is passed in then it looks at the Users Setting then the Users works for default department's setting
        /// If a OMSFile object is passed then it looks at the settings on the FMApplication app
        /// </summary>
        /// <param name="obj">IOMSType User or OMSFile</param>
        /// <param name="app">The FMApplication</param>
        /// <returns>ActionOrderType setting that corresponds to the obj and app parameters</returns>
        internal static ActionsOrderType GetActionsOrderSetting(Interfaces.IOMSType obj,FMApplication app)
        {
            try
            {
                if (obj is User)
                {
                    User user = (User)obj;
                    int option = Convert.ToInt32(user.GetXmlProperty("ActionsOrderContextMenu", -1));

                    if (option == -1)
                        option = new Department(user.WorksFor.DefaultDepartment).ActionsOrderContextMenu;

                    return (ActionsOrderType)Common.ConvertDef.ToEnum(option, ActionsOrderType.FileActions1st);
                }

                if (obj is OMSFile)
                {
                    return app.ActionsOrderContextMenu;
                }
            }
            //If an error occurs continue and display the default
            catch { }

            return ActionsOrderType.FileActions1st;
        }

        /// <summary>
        /// Get the searchlist override for the tasks tab
        /// If a user object is passed in then it looks at the Users Setting then the Users works for default department's setting
        /// If a OMSFile object is passed then it looks at the settings on the FMApplication app
        /// </summary>
        /// <param name="obj">IOMSType User or OMSFile</param>
        /// <returns>The searchlist code or empty string if there is no override</returns>
        internal static string GetTasksSearchListOverride(Interfaces.IOMSType obj)
        {
            string Override = string.Empty;
            try
            {
                if (obj is User)
                {
                    User user = (User)obj;
                    Override = user.TasksAddinCommandCentreSearchListOverride;

                    if (string.IsNullOrEmpty(Override))
                    {
                        Override = new Department(user.WorksFor.DefaultDepartment).TasksAddinCommandCentreSearchListOverride;
                    }

                }
            }
            catch { }

            return Override;
        }
           
        #endregion

        #region Serialisation

        public void Save()
        {
            string f = FWBS.OMS.Global.GetCachePath();
            f = System.IO.Path.Combine(f, "File Management");
            f = System.IO.Path.Combine(f, "Applications");
            f = System.IO.Path.Combine(f, System.Threading.Thread.CurrentThread.CurrentUICulture.Name);
            f = System.IO.Path.Combine(f, Code);
            f = System.IO.Path.ChangeExtension(f, Global.CacheExt);
            base.Save(new System.IO.FileInfo(f));
        }

        public static FMApplication Load(string code)
        {
            code = code ?? String.Empty;

            string f = FWBS.OMS.Global.GetCachePath();
            f = System.IO.Path.Combine(f, "File Management");
            f = System.IO.Path.Combine(f, "Applications");
            f = System.IO.Path.Combine(f, System.Threading.Thread.CurrentThread.CurrentUICulture.Name);
            f = System.IO.Path.Combine(f, code);
            f = System.IO.Path.ChangeExtension(f, Global.CacheExt);

            System.IO.FileInfo file = new System.IO.FileInfo(f);

            try
            {
                if (file.Exists)
                    return new FMApplication(file);
                else
                    return null;
            }
            catch (OMSException2)
            {
                return null;
            }
        }


        #endregion

        #region INotifyPropertyChanged Members

        public new event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            var ev = PropertyChanged;
            if (ev != null)
                ev(this, new PropertyChangedEventArgs(propertyName));
        }


        #endregion
    }
}
